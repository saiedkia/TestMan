using Prism.Mvvm;
using Prism.Windows.Navigation;
using static System.Net.Mime.MediaTypeNames;

namespace UWPClient.ViewModels
{
    public class BaseViewModel : BindableBase
    {
        public INavigationService NavigationService { get; protected set; }
       
    }
}
