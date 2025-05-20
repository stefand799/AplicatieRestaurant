using System.ComponentModel;
using AppRestaurant.Services.Navigation;
using AppRestaurant.ViewModels;
using AppRestaurant.ViewModels.Screens;

namespace AppRestaurant.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IScreensNavigationService _nav;

        public MainWindowViewModel(IScreensNavigationService nav)
        {
            _nav = nav;
            // Ne abonăm la schimbările din NavigationService pe CurrentScreenViewModel
            if (_nav is INotifyPropertyChanged notify)
            {
                notify.PropertyChanged += (s, e) =>
                {
                    // când NavigationService ridică PropertyChanged pentru CurrentScreenViewModel
                    if (e.PropertyName == nameof(NavigationService.CurrentScreenViewModel))
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentScreenViewModel)));
                };
            }
            // Ecranul inițial
            _nav.Navigate<GuestViewModel>();
        }

        // Proprietate pe care o leagă UI-ul
        public ViewModelBase CurrentScreenViewModel => (_nav as NavigationService).CurrentScreenViewModel;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}