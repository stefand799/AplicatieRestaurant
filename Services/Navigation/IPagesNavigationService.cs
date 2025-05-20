using AppRestaurant.ViewModels;

namespace AppRestaurant.Services.Navigation
{
    public interface IPagesNavigationService
    {
        ViewModelBase CurrentViewModel { get; }
        void Navigate<TViewModel>() where TViewModel : ViewModelBase;
    }
}