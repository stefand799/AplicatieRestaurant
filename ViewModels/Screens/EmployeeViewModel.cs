using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppRestaurant.Services.Navigation;
using AppRestaurant.ViewModels.Pages.Employee; // namespaces for page VMs: CategoriesViewModel, PreparationsViewModel, MenusViewModel, AllergensViewModel

namespace AppRestaurant.ViewModels.Screens
{
    public partial class EmployeeViewModel : ViewModelBase
    {
        private readonly IScreensNavigationService _screensNav;
        private readonly IPagesNavigationService _pagesNav;

        public EmployeeViewModel(
            IScreensNavigationService screensNav,
            IPagesNavigationService pagesNav)
        {
            _screensNav = screensNav;
            _pagesNav = pagesNav;

            // La pornire, afișează lista de categorii în zona de pages
            _pagesNav.Navigate<CategoriesViewModel>();
        }

        // Menu Management Section
        [RelayCommand]
        private void ShowCategoriesPage() => _pagesNav.Navigate<CategoriesViewModel>();
        [RelayCommand]
        private void ShowDishesPage() => _pagesNav.Navigate<DishesViewModel>();
        [RelayCommand]
        private void ShowMealsPage() => _pagesNav.Navigate<MealsViewModel>();
        [RelayCommand]
        private void ShowAllergensPage() => _pagesNav.Navigate<AllergensViewModel>();

        // Order Management Section
        [RelayCommand]
        private void ShowAllOrdersPage() => _pagesNav.Navigate<AllOrdersViewModel>();
        [RelayCommand]
        private void ShowActiveOrdersPage() => _pagesNav.Navigate<ActiveOrdersViewModel>();

        // Inventory Management Section
        [RelayCommand]
        private void ShowLowStockPage() => _pagesNav.Navigate<LowStockViewModel>();

        // Logout
        [RelayCommand]
        private void LogOut()
        {
            _screensNav.Navigate<GuestViewModel>();
        }

        // Expose PagesNav for binding
        public IPagesNavigationService PagesNav => _pagesNav;
    }
}