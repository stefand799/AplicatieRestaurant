// AppRestaurantDbContextFactory.cs
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using AppRestaurant.Data; // Asigură-te că namespace-ul este corect

public class AppRestaurantDbContextFactory : IDesignTimeDbContextFactory<AppRestaurantDbContext>
{
    public AppRestaurantDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppRestaurantDbContext>();
        
        // AICI! Înlocuiește "YourConnectionStringHere" cu string-ul tău de conexiune real la baza de date.
        // Acest connection string este folosit doar de instrumentele EF la design-time.
        // Asigură-te că folosești același tip de provider de bază de date (ex: UseSqlServer, UseSqlite, UsePostgreSQL).
        optionsBuilder.UseSqlServer("Server=127.0.0.1,14330;Database=AppRestaurant;User Id=sa;Password=Qwerty12345;TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;"); // Exemplu pentru SQL Server

        return new AppRestaurantDbContext(optionsBuilder.Options);
    }
}