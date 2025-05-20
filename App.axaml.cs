using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Data.Core.Plugins;
using AppRestaurant.Data; 
using AppRestaurant.Models;
using AppRestaurant.Services.Auth;
using AppRestaurant.Services.Menu;
using AppRestaurant.Services.Navigation;
using AppRestaurant.ViewModels;
using AppRestaurant.ViewModels.Pages;
using AppRestaurant.ViewModels.Pages.Customer;
using AppRestaurant.ViewModels.Pages.Employee;
using AppRestaurant.ViewModels.Screens;
using AppRestaurant.ViewModels.Screens.Auth;
using AppRestaurant.Views;
using AppRestaurant.Views.Screens.Auth;
using Microsoft.AspNetCore.Identity; // Asigură-te că ai acest using pentru IPasswordHasher

namespace AppRestaurant;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            
            // --- Începutul adăugării pentru popularea bazei de date ---
            Console.WriteLine("Starting database initialization in App.cs...");
            using (var scope = ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppRestaurantDbContext>();
                // Obține și IPasswordHasher<User> din scope-ul serviciilor
                var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
                
                try
                {
                    // Aplică migrațiile la baza de date. Este crucial să faci asta înainte de a popula datele.
                    Console.WriteLine("Applying database migrations...");
                    context.Database.Migrate(); 
                    Console.WriteLine("Database migrations applied.");
                    
                    // Apelează metoda de populare a bazei de date, pasând contextul ȘI passwordHasher-ul.
                    // Folosim .Wait() deoarece OnFrameworkInitializationCompleted este o metodă async și blocăm execuția UI-ului,
                    // ceea ce este acceptabil pentru o operație de inițializare la pornire.
                    Console.WriteLine("Calling SeedData.Initialize...");
                    SeedData.Initialize(context, passwordHasher).Wait();
                    Console.WriteLine("SeedData.Initialize completed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred during database seeding: {ex.Message}");
                    // Poți înregistra eroarea într-un fișier log sau un sistem de logging real
                    // exemplu: log.Error(ex, "Eroare la popularea bazei de date.");
                }
            }
            // --- Sfârșitul adăugării pentru popularea bazei de date ---

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddDbContext<AppRestaurantDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                   .EnableSensitiveDataLogging() 
                   .EnableDetailedErrors());    

        // Servicii
        services.AddSingleton<IAuthService, AuthService>();
        // Asigură-te că IPasswordHasher<User> este înregistrat ca un serviciu scoped sau transient
        // Scoped este în general mai potrivit aici, deoarece AuthService îl folosește.
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>(); 
        services.AddSingleton<IScreensNavigationService, NavigationService>();
        services.AddSingleton<IPagesNavigationService, NavigationService>();
        services.AddTransient<IMenuService, MenuService>();
        
        // ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegisterViewModel>();
        services.AddTransient<GuestViewModel>();
        services.AddTransient<CustomerViewModel>();
        services.AddTransient<EmployeeViewModel>();

        // Page ViewModels
        services.AddTransient<MenuViewModel>();
        services.AddTransient<CustomerOrdersViewModel>();
        services.AddTransient<CartViewModel>();
        services.AddTransient<CategoriesViewModel>();
        services.AddTransient<DishesViewModel>();
        services.AddTransient<MealsViewModel>();
        services.AddTransient<AllergensViewModel>();
        services.AddTransient<AllOrdersViewModel>();
        services.AddTransient<ActiveOrdersViewModel>();
        services.AddTransient<LowStockViewModel>();

        // Views
        services.AddSingleton<MainWindow>();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var plugins = BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();
        foreach (var plugin in plugins)
            BindingPlugins.DataValidators.Remove(plugin);
    }
}