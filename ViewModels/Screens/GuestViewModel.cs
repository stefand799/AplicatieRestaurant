using CommunityToolkit.Mvvm.Input;
using AppRestaurant.Services.Navigation;
using AppRestaurant.ViewModels.Pages;
using AppRestaurant.ViewModels.Screens.Auth;

namespace AppRestaurant.ViewModels.Screens
{
    public partial class GuestViewModel : ViewModelBase
    {
        private readonly IScreensNavigationService _screensNav;
        private readonly IPagesNavigationService _pagesNav;

        public GuestViewModel(IScreensNavigationService screensNav, IPagesNavigationService pagesNav)
        {
            _screensNav = screensNav;
            _pagesNav = pagesNav;

            // setează pagina implicită (menu)
            _pagesNav.Navigate<MenuViewModel>();
        }

        [RelayCommand]
        private void NavigateToLogin() => _screensNav.Navigate<LoginViewModel>();

        [RelayCommand]
        private void NavigateToRegister() => _screensNav.Navigate<RegisterViewModel>();
    
        public IPagesNavigationService PagesNav => _pagesNav;
    }

}