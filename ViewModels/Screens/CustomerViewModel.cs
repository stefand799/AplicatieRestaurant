using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppRestaurant.Services.Navigation;
using AppRestaurant.ViewModels.Pages;
using AppRestaurant.ViewModels.Pages.Customer;

namespace AppRestaurant.ViewModels.Screens
{
    public partial class CustomerViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly IPagesNavigationService _pagesNav;
        private readonly IScreensNavigationService _screensNav;

        public CustomerViewModel(
            IScreensNavigationService screensNav,
            IPagesNavigationService   pagesNav)
        {
            _screensNav = screensNav;
            _pagesNav   = pagesNav;

            if (_pagesNav is INotifyPropertyChanged np)
            {
                np.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(NavigationService.CurrentPageViewModel))
                        OnPropertyChanged(nameof(CurrentPageViewModel));
                };
            }
            _pagesNav.Navigate<MenuViewModel>();
        }

        public ViewModelBase CurrentPageViewModel 
            => (_pagesNav as NavigationService).CurrentPageViewModel;
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        [RelayCommand] private void ShowMenuPage()   => _pagesNav.Navigate<MenuViewModel>();
        [RelayCommand] private void ShowCartPage()   => _pagesNav.Navigate<CartViewModel>();
        [RelayCommand] private void ShowOrdersPage() => _pagesNav.Navigate<CustomerOrdersViewModel>();

        [RelayCommand] private void LogOut()=> _screensNav.Navigate<GuestViewModel>();
    }

}