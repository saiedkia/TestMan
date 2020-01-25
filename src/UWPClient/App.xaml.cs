using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace UWPClient
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Prism.SimpleInjector.Windows.PrismSimpleInjectorApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("Main", null);
            return Task.CompletedTask;
        }
    }
}
