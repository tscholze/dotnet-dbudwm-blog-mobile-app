using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Der_Bayer_und_der_Würschtlmann
{
    /// <summary>
    ///    A page that does only contain a web view.
    ///    It is back button compatible.
    /// </summary>
    public sealed partial class BrowserPage : Page
    {
        public BrowserPage()
        {
            InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += OnAppBackRequested;
        }

        #region System event handler

        /// <summary>
        ///     Triggered if view will be visible.
        ///     Loads the given parameter as web source for the browser
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            WebView.Source = (Uri)e.Parameter;
        }

        /// <summary>
        ///     Handle back button request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAppBackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
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
