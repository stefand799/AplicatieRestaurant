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
using AppRestaurant.Services.Navigation;
using AppRestaurant.ViewModels;
using AppRestaurant.ViewModels.Pages;
using AppRestaurant.ViewModels.Pages.Customer;
using AppRestaurant.ViewModels.Pages.Employee;
using AppRestaurant.ViewModels.Screens;
using AppRestaurant.ViewModels.Screens.Auth;
using AppRestaurant.Views;
using AppRestaurant.Views.Screens.Auth;
using Microsoft.AspNetCore.Identity;

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
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddSingleton<IAuthService, AuthService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddSingleton<NavigationService>();
        services.AddSingleton<IScreensNavigationService>(sp => sp.GetRequiredService<NavigationService>());
        services.AddSingleton<IPagesNavigationService>(sp => sp.GetRequiredService<NavigationService>());

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