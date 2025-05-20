using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppRestaurant.Services.Navigation;
using AppRestaurant.ViewModels.Pages;
using AppRestaurant.ViewModels.Pages.Employee;

namespace AppRestaurant.ViewModels.Screens
{
    public partial class EmployeeViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly IPagesNavigationService _pagesNav;
        private readonly IScreensNavigationService _screensNav;

        public EmployeeViewModel(
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
        [RelayCommand] private void ShowActiveOrdersPage()   => _pagesNav.Navigate<ActiveOrdersViewModel>();
        [RelayCommand] private void ShowAllergensPage()   => _pagesNav.Navigate<AllergensViewModel>();
        [RelayCommand] private void ShowAllOrdersPage() => _pagesNav.Navigate<AllOrdersViewModel>();
        [RelayCommand] private void ShowCategoriesPage() => _pagesNav.Navigate<CategoriesViewModel>();
        [RelayCommand] private void ShowDishesPage() => _pagesNav.Navigate<DishesViewModel>();
        [RelayCommand] private void ShowLowStockPage() => _pagesNav.Navigate<LowStockViewModel>();
        [RelayCommand] private void ShowMealsPage() => _pagesNav.Navigate<MealsViewModel>();

        [RelayCommand] private void LogOut()=> _screensNav.Navigate<GuestViewModel>();
    }
}