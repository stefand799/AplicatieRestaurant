using System;
using System.Threading.Tasks;
using AppRestaurant.Services.Auth;
using AppRestaurant.Services.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppRestaurant.ViewModels.Screens.Auth
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly IScreensNavigationService _screensNavigator;
        private readonly IAuthService _authService;
        
        public LoginViewModel(IScreensNavigationService screensNavigator, IAuthService authService)
        {
            _screensNavigator = screensNavigator;
            _authService = authService;
        }
        
        [ObservableProperty]
        private string _email = string.Empty;
        
        [ObservableProperty]
        private string _password = string.Empty;
        
        [ObservableProperty]
        private string _errorMessage = string.Empty;
        
        [RelayCommand]
        public async Task Login()
        {
            ErrorMessage = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    ErrorMessage = "Email and password are required.";
                    return;
                }

                var user = await _authService.LoginAsync(Email, Password);

                if (user == null)
                {
                    ErrorMessage = "Invalid login credentials.";
                    return;
                }

                if (user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _screensNavigator.Navigate<EmployeeViewModel>();
                }
                else
                {
                    _screensNavigator.Navigate<CustomerViewModel>();
                }
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Something went wrong.";
                Console.WriteLine(ex);
            }
        }

        [RelayCommand]
        public void ToRegisterScreen()
        {
            _screensNavigator.Navigate<RegisterViewModel>();
        }

        [RelayCommand]
        public void ToGuestScreen()
        {
            _screensNavigator.Navigate<GuestViewModel>();
        }
    }
}