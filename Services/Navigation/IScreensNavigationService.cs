using AppRestaurant.ViewModels;

namespace AppRestaurant.Services.Navigation
{
    public interface IScreensNavigationService
    {
        ViewModelBase CurrentViewModel { get; }
        void Navigate<TViewModel>() where TViewModel : ViewModelBase;
    }
}