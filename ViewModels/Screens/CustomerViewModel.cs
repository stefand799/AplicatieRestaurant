using CommunityToolkit.Mvvm.Input;
using AppRestaurant.Services.Navigation;
using AppRestaurant.ViewModels.Pages;
using AppRestaurant.ViewModels.Pages.Customer;

namespace AppRestaurant.ViewModels.Screens
{
    public partial class CustomerViewModel : ViewModelBase
    {
        private readonly IScreensNavigationService _screensNavigator;
        private readonly IPagesNavigationService _pagesNavigator;

        public CustomerViewModel(
            IScreensNavigationService screensNavigator,
            IPagesNavigationService pagesNavigator)
        {
            _screensNavigator = screensNavigator;
            _pagesNavigator = pagesNavigator;

            // La pornire, afișează pagina de meniu (MenuViewModel)
            _pagesNavigator.Navigate<MenuViewModel>();
        }

        // Comenzi pentru navigarea între pagini în interiorul CustomerScreen
        [RelayCommand]
        private void ShowMenuPage() => _pagesNavigator.Navigate<MenuViewModel>();

        [RelayCommand]
        private void ShowOrdersPage() => _pagesNavigator.Navigate<CustomerOrdersViewModel>();

        [RelayCommand]
        private void ShowCartPage() => _pagesNavigator.Navigate<CartViewModel>();

        // // Comenzi pentru navigare între screens (dacă există)
        // // de exemplu un profil, dacă este un screen separat
        // [RelayCommand]
        // private void NavigateToProfileScreen() => _screensNav.Navigate<ProfileViewModel>();

        [RelayCommand]
        private void LogOut()
        {
            // navighează înapoi la GuestScreen
            _screensNavigator.Navigate<GuestViewModel>();
        }

        // Exponi serviciul de pagini pentru binding în XAML
        public IPagesNavigationService PagesNavigator => _pagesNavigator;
    }
}