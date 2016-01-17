using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Der_Bayer_und_der_Würschtlmann
{
    /// <summary>
    ///     Page that contains information about the blog.
    /// </summary>
    public sealed partial class InfoPage : Page
    {
        public InfoPage()
        {
            InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += OnAppBackRequested;
        }

        #region System event handlers

        /// <summary>
        ///     Handle back button request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAppBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;

            if (frame == null || !frame.CanGoBack || e.Handled)
            {
                return;
            }

            e.Handled = true;
            frame.GoBack();
        }

        #endregion
    }
}
