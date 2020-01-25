using System.Collections.Generic;
using UWPClient.Models;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UWPClient.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static CoreDispatcher Invoker { get; private set; }
        //public List<API> Apis { get; set; }
        //public List<Parameter> Parameters { get; set; }
        //INavigationService navigationService;
        
        public MainPage()
        {
            this.InitializeComponent();
            //this.navigationService = navigationService; 
            requestsLsv.SelectionChanged += RequestsLsv_SelectionChanged;
            Invoker = Dispatcher;
        }

        private void RequestsLsv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<RequestParameterBindable> lst = new List<RequestParameterBindable>();
            foreach (object i in requestsLsv.SelectedItems)
                lst.Add(i as RequestParameterBindable);

            ((ViewModels.MainPageViewModel)DataContext).SelectedRequests = lst;
        }

        private void HomeNavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // ContentFrame.Navigate(typeof(MainContentPage));

            //MainPageViewModel vm = (MainPageViewModel)this.DataContext;
            //vm.NavigationService.Navigate("MainContent", null);
        }

        private async void TokenNavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {

            //TokenInput tokenInput = new TokenInput
            //{
            //    Callback = (_, token) =>
            //    {

            //    }
            //};

            //await tokenInput.ShowAsync();
        }

        private void RequestsLsv_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ((ViewModels.MainPageViewModel)DataContext).RequestRename();
        }

       
    }
}
