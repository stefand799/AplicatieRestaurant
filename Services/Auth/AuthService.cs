using System.Threading.Tasks;
using AppRestaurant.Data;
using AppRestaurant.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppRestaurant.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppRestaurantDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private User _currentUser = null;

        public AuthService(AppRestaurantDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }
        
        public User CurrentUser => _currentUser;

        // Create (Register)
        public async Task<User?> RegisterAsync(
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            string deliveryAddress,
            string password,
            string role)
        {
            
            if (!email.Contains("@") || !email.Contains("."))
            {
                return null;
            }

            if (password.Length < 6) 
            {
                return null;
            }

            var existingUser = await _context.Users.AnyAsync(u => u.Email == email);
            if (existingUser)
            {
                return null;
            }

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber, 
                Address = deliveryAddress, 
                PasswordHash = (password), 
                Role = role 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
          
            return user;
        }
        
        // Read (Login)
        public async Task<User?> LoginAsync(string email, string password)
        {
            // Find the user by email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            // Verify hashed password
            var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (verifyResult == PasswordVerificationResult.Failed)
                return null;

            return user;
        }
        
        public async Task LogoutAsync()
        {
            _currentUser = null;
            await Task.CompletedTask; 
        }

        public bool IsAuthenticated => _currentUser is not null;
        
        public void SetCurrentUser(User user)
        {
            _currentUser = user;
        }
    }
}