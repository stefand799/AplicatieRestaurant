using System.Threading.Tasks;
using AppRestaurant.Models;
namespace AppRestaurant.Services.Auth
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string email, string password);
        Task<User?> RegisterAsync(
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            string deliveryAddress,
            string password,
            string role);
        Task LogoutAsync();
    }
}