using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Der_Bayer_und_der_Würschtlmann
{
    /// <summary>
    ///     A list view of the dbudwm app.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        ///     Contains the selected item in cas eof a user interaction
        /// </summary>
        private FeedItem selectedFeedItem;

        public MainPage()
        {
            InitializeComponent();
            LoadFeed();

            // Init share feature
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += OnDataRequested;
        }

        /// <summary>
        ///     Loads the feed and sets it as the list view item source
        /// </summary>
        private async void LoadFeed()
        {
            ArticleListView.ItemsSource = await TSUtils.getFeedsAsync("https://dbudwm.wordpress.com/feed");
        }

        /// <summary>
        ///     Required by system, formats the output string to:
        ///         "My new article headline > http://myblog.tld/myFoo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (selectedFeedItem == null) return;

            DataRequest request = args.Request;
            var message = string.Format("{0} > {1}", selectedFeedItem.Title, selectedFeedItem.Link.AbsoluteUri);
            request.Data.SetText(message);
        }

        #region User event handlers

        /// <summary>
        ///     Triggred in case of an article's click.
        ///     Navigates to the detail view aka browser view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemClicked(object sender, RoutedEventArgs e)
        {
            var selectedItem = GetFeedItemByButtonSender(sender);
            if (selectedItem == null) return;

            Frame.Navigate(typeof(BrowserPage), selectedItem.Link);
        }

        /// <summary>
        ///     Triggered in case of the user holds / taps long on a article.
        ///     Open share menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemHolded(object sender, HoldingRoutedEventArgs e)
        {
            selectedFeedItem = GetFeedItemByButtonSender(sender);
            if (selectedFeedItem == null) return;

            DataTransferManager.ShowShareUI();
        }

        /// <summary>
        ///     Triggered in case of an info icon's click.
        ///     Navigates to the About view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onInfoClicked(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(InfoPage));
        }

        #endregion

        #region Helper

        /// <summary>
        ///     Gets a FeedItem object of an object / Button object cascade.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <returns>Casted FeedItem object</returns>
        private FeedItem GetFeedItemByButtonSender(object sender)
        {
            var frame = (Frame)Window.Current.Content;
            var button = (Button)sender;
            return (FeedItem)button.DataContext;
        }

        #endregion
    }
}
