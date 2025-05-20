using System.ComponentModel;
using AppRestaurant.Services.Navigation;
using AppRestaurant.ViewModels;
using AppRestaurant.ViewModels.Screens;

namespace AppRestaurant.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IScreensNavigationService _screensNav;

        public MainWindowViewModel(IScreensNavigationService screensNav)
        {
            _screensNav = screensNav;
            // abonare la schimbarea Screen-ului
            if (_screensNav is INotifyPropertyChanged np)
                np.PropertyChanged += (s,e) =>
                { if (e.PropertyName == nameof(NavigationService.CurrentScreenViewModel))
                    OnPropertyChanged(nameof(CurrentScreenViewModel)); };
            _screensNav.Navigate<GuestViewModel>();
        }

        public ViewModelBase CurrentScreenViewModel 
            => (_screensNav as NavigationService).CurrentScreenViewModel;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}