using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using AppRestaurant.ViewModels;

namespace AppRestaurant.Services.Navigation
{
    /// <summary>
    /// Central navigation service handling both screen- and page-level navigation.
    /// Resolves ViewModel instances via DI to support constructor injection.
    /// </summary>
    public class NavigationService : IScreensNavigationService, IPagesNavigationService, INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;
        private ViewModelBase _currentScreen;
        private ViewModelBase _currentPage;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// The ViewModel of the currently active screen.
        /// </summary>
        public ViewModelBase CurrentScreenViewModel
        {
            get => _currentScreen;
            private set
            {
                if (_currentScreen != value)
                {
                    _currentScreen = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The ViewModel of the currently active page within a screen.
        /// </summary>
        public ViewModelBase CurrentPageViewModel
        {
            get => _currentPage;
            private set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged();
                }
            }
        }

        // Explicit interface implementations to separate screen vs. page contexts
        ViewModelBase IScreensNavigationService.CurrentViewModel => CurrentScreenViewModel;
        ViewModelBase IPagesNavigationService.CurrentViewModel => CurrentPageViewModel;

        /// <summary>
        /// Navigate to a new screen of type T, resetting any active page.
        /// </summary>
        public void NavigateScreen<TViewModel>() where TViewModel : ViewModelBase
        {
            CurrentScreenViewModel = _serviceProvider.GetRequiredService<TViewModel>();
            CurrentPageViewModel = null;
        }

        /// <summary>
        /// Navigate to a new page of type T within the current screen.
        /// </summary>
        public void NavigatePage<TViewModel>() where TViewModel : ViewModelBase
        {
            CurrentPageViewModel = _serviceProvider.GetRequiredService<TViewModel>();
        }

        // Interface method mappings
        void IScreensNavigationService.Navigate<TViewModel>() => NavigateScreen<TViewModel>();
        void IPagesNavigationService.Navigate<TViewModel>() => NavigatePage<TViewModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
