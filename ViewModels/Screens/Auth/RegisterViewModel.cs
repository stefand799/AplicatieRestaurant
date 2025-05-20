using System;
using System.Threading.Tasks;
using AppRestaurant.Models;
using AppRestaurant.Services.Auth;
using AppRestaurant.Services.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppRestaurant.ViewModels.Screens.Auth
{
    public partial class RegisterViewModel : ViewModelBase
    {
        private readonly IScreensNavigationService _screensNavigator;
        private readonly IAuthService _authService;

        public RegisterViewModel(
            IScreensNavigationService screensNavigator,
            IAuthService authService)
        {
            _screensNavigator = screensNavigator;
            _authService = authService;
        }
        
        [ObservableProperty] private string _firstName = string.Empty;
        [ObservableProperty] private string _lastName = string.Empty;
        [ObservableProperty] private string _email = string.Empty;
        [ObservableProperty] private string _phoneNumber = string.Empty;
        [ObservableProperty] private string _address = string.Empty;
        [ObservableProperty] private string _password = string.Empty;
        [ObservableProperty] private string _confirmPassword = string.Empty;
        [ObservableProperty] private string _role = "Customer";
        [ObservableProperty] private string _errorMessage = string.Empty;
        
        [RelayCommand]
        public async Task RegisterAsync()
        {
            ErrorMessage = string.Empty;

            // Validations
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Role))
            {
                ErrorMessage = "All required fields must be filled.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            try
            {
                // Call RegisterAsync with matching parameter order
                var user = await _authService.RegisterAsync(
                    firstName: FirstName,
                    lastName: LastName,
                    email: Email,
                    phoneNumber: PhoneNumber,
                    deliveryAddress: Address,
                    password: Password,
                    role: Role
                );

                if (user == null)
                {
                    ErrorMessage = "Registration failed. Please check your details or try another email.";
                    return;
                }

                // Set the current user in the auth service if supported
                if (_authService is AuthService concreteAuth)
                {
                    concreteAuth.SetCurrentUser(user);
                }

                // Navigate based on role
                if (user.Role.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                {
                    _screensNavigator.Navigate<EmployeeViewModel>();
                }
                else
                {
                    _screensNavigator.Navigate<CustomerViewModel>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred during registration.";
                Console.WriteLine(ex);
            }
        }

        [RelayCommand]
        public void ToLoginScreen()
        {
            _screensNavigator.Navigate<LoginViewModel>();
        }

        [RelayCommand]
        public void ToGuestScreen()
        {
            _screensNavigator.Navigate<GuestViewModel>();
        }
    }
}