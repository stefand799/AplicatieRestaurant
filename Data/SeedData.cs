using AppRestaurant.Data;
using AppRestaurant.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppRestaurant.Data
{
    public static class SeedData
    {
        public static async Task Initialize(AppRestaurantDbContext context, IPasswordHasher<User> passwordHasher)
        {
            Console.WriteLine("Starting database seeding...");

            // 1. Adaugă Utilizatori
            if (!await context.Users.AnyAsync().ConfigureAwait(false))
            {
                Console.WriteLine("Adding users...");
                var users = new User[]
                {
                    new User
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "admin@example.com",
                        PasswordHash = passwordHasher.HashPassword(new User(), "AdminPass123!"),
                        PhoneNumber = "0700111222",
                        Address = "Str. Libertatii 1, Bl. A1, Ap. 1",
                        Role = "Admin"
                    },
                    new User
                    {
                        FirstName = "Customer",
                        LastName = "User",
                        Email = "customer@example.com",
                        PasswordHash = passwordHasher.HashPassword(new User(), "CustomerPass123!"),
                        PhoneNumber = "0700333444",
                        Address = "Bulevardul Eroilor 22, Bl. B2, Ap. 5",
                        Role = "Customer"
                    }
                };
                await context.Users.AddRangeAsync(users).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                Console.WriteLine("Users added.");
            }
            else
            {
                Console.WriteLine("Users already exist. Skipping user seeding.");
            }

            var usersInDb = await context.Users.ToListAsync().ConfigureAwait(false);

            // 2. Adaugă Categorii
            var categories = new List<Category>();
            if (!await context.Categories.AnyAsync().ConfigureAwait(false))
            {
                Console.WriteLine("Adding categories...");
                categories.AddRange(new Category[]
                {
                    new Category { Name = "Aperitive" },
                    new Category { Name = "Fel Principal" },
                    new Category { Name = "Desert" },
                    new Category { Name = "Bauturi" },
                    new Category { Name = "Meniul Zilei" }
                });
                await context.Categories.AddRangeAsync(categories).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                Console.WriteLine("Categories added.");
            }
            else
            {
                Console.WriteLine("Categories already exist. Skipping category seeding.");
                categories = await context.Categories.ToListAsync().ConfigureAwait(false);
            }

            // 3. Adaugă Alergeni
            var allergens = new List<Allergen>();
            if (!await context.Allergens.AnyAsync().ConfigureAwait(false))
            {
                Console.WriteLine("Adding allergens...");
                allergens.AddRange(new Allergen[]
                {
                    new Allergen { Name = "Gluten" },
                    new Allergen { Name = "Lactoza" },
                    new Allergen { Name = "Oua" },
                    new Allergen { Name = "Nuci" },
                    new Allergen { Name = "Soia" },
                    new Allergen { Name = "Peste" },
                    new Allergen { Name = "Crustacee" }
                });
                await context.Allergens.AddRangeAsync(allergens).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                Console.WriteLine("Allergens added.");
            }
            else
            {
                Console.WriteLine("Allergens already exist. Skipping allergen seeding.");
                allergens = await context.Allergens.ToListAsync().ConfigureAwait(false);
            }

            // 4. Adaugă Feluri de Mancare (Dishes)
            var dishes = new List<Dish>();
            if (!await context.Dishes.AnyAsync().ConfigureAwait(false))
            {
                Console.WriteLine("Adding dishes...");

                var aperitiveCat = categories.Single(c => c.Name == "Aperitive");
                var felPrincipalCat = categories.Single(c => c.Name == "Fel Principal");
                var desertCat = categories.Single(c => c.Name == "Desert");
                var bauturiCat = categories.Single(c => c.Name == "Bauturi");

                var glutenAllergen = allergens.Single(a => a.Name == "Gluten");
                var lactozaAllergen = allergens.Single(a => a.Name == "Lactoza");
                var ouaAllergen = allergens.Single(a => a.Name == "Oua");
                var pesteAllergen = allergens.Single(a => a.Name == "Peste");
                var crustaceeAllergen = allergens.Single(a => a.Name == "Crustacee");

                dishes.AddRange(new Dish[]
                {
                    new Dish
                    {
                        CategoryId = aperitiveCat.Id,
                        Name = "Salata Cezar",
                        Price = 35.00m,
                        ServingSize = 300,
                        ServingUnit = "g",
                        StockQuantity = 100,
                        Availability = true,
                        ImagePath = "avares://AppRestaurant/Assets/salata_cezar.jpg",
                        Allergens = new List<Allergen> { glutenAllergen, lactozaAllergen }
                    },
                    new Dish
                    {
                        CategoryId = felPrincipalCat.Id,
                        Name = "Friptura de Vita cu Cartofi",
                        Price = 75.00m,
                        ServingSize = 450,
                        ServingUnit = "g",
                        StockQuantity = 50,
                        Availability = true,
                        ImagePath = "avares://AppRestaurant/Assets/friptura_vita.jpg",
                        Allergens = new List<Allergen> { lactozaAllergen }
                    },
                    new Dish
                    {
                        CategoryId = desertCat.Id,
                        Name = "Tort de Ciocolata",
                        Price = 25.00m,
                        ServingSize = 150,
                        ServingUnit = "g",
                        StockQuantity = 80,
                        Availability = true,
                        ImagePath = "avares://AppRestaurant/Assets/tort_ciocolata.jpg",
                        Allergens = new List<Allergen> { glutenAllergen, lactozaAllergen, ouaAllergen }
                    },
                    new Dish
                    {
                        CategoryId = bauturiCat.Id,
                        Name = "Limonada cu Menta",
                        Price = 15.00m,
                        ServingSize = 400,
                        ServingUnit = "ml",
                        StockQuantity = 200,
                        Availability = true,
                        ImagePath = "avares://AppRestaurant/Assets/limonada_menta.jpg",
                        Allergens = new List<Allergen>()
                    },
                    new Dish
                    {
                        CategoryId = felPrincipalCat.Id,
                        Name = "Paella Valenciana",
                        Price = 65.00m,
                        ServingSize = 500,
                        ServingUnit = "g",
                        StockQuantity = 30,
                        Availability = true,
                        ImagePath = "avares://AppRestaurant/Assets/paella.jpg",
                        Allergens = new List<Allergen> { pesteAllergen, crustaceeAllergen }
                    }
                });
                await context.Dishes.AddRangeAsync(dishes).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                Console.WriteLine("Dishes added.");
            }
            else
            {
                Console.WriteLine("Dishes already exist. Skipping dish seeding.");
                dishes = await context.Dishes.Include(d => d.Allergens).ToListAsync().ConfigureAwait(false);
            }

            // 5. Adaugă Mese (Meals)
            var meals = new List<Meal>();
            if (!await context.Meals.AnyAsync().ConfigureAwait(false))
            {
                Console.WriteLine("Adding meals...");
                var meniulZileiCat = categories.Single(c => c.Name == "Meniul Zilei");

                meals.AddRange(new Meal[]
                {
                    new Meal
                    {
                        CategoryId = meniulZileiCat.Id,
                        Name = "Meniul Zilei 1 (Friptura)",
                        BasePrice = 80.00m,
                        DiscountPrecentage = 0.10m,
                        ImagePath = "avares://AppRestaurant/Assets/meniul_zilei1.jpg"
                    },
                    new Meal
                    {
                        CategoryId = meniulZileiCat.Id,
                        Name = "Meniul Zilei 2 (Paella)",
                        BasePrice = 70.00m,
                        DiscountPrecentage = 0.05m,
                        ImagePath = "avares://AppRestaurant/Assets/meniul_zilei2.jpg"
                    }
                });
                await context.Meals.AddRangeAsync(meals).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                Console.WriteLine("Meals added.");
            }
            else
            {
                Console.WriteLine("Meals already exist. Skipping meal seeding.");
                meals = await context.Meals.ToListAsync().ConfigureAwait(false);
            }

            // 6. Adaugă DishInMeals (legături între Dishes și Meals)
            if (!await context.DishInMeals.AnyAsync().ConfigureAwait(false))
            {
                Console.WriteLine("Adding dish-in-meals relations...");

                var fripturaVita = dishes.Single(d => d.Name == "Friptura de Vita cu Cartofi");
                var salataCezar = dishes.Single(d => d.Name == "Salata Cezar");
                var paellaValenciana = dishes.Single(d => d.Name == "Paella Valenciana");
                var limonadaMenta = dishes.Single(d => d.Name == "Limonada cu Menta");

                var meniulZilei1 = meals.Single(m => m.Name == "Meniul Zilei 1 (Friptura)");
                var meniulZilei2 = meals.Single(m => m.Name == "Meniul Zilei 2 (Paella)");

                var dishInMeals = new DishInMeal[]
                {
                    new DishInMeal
                    {
                        DishId = fripturaVita.Id,
                        MealId = meniulZilei1.Id,
                        DishServingSize = 1.0m
                    },
                    new DishInMeal
                    {
                        DishId = salataCezar.Id,
                        MealId = meniulZilei1.Id,
                        DishServingSize = 0.5m
                    },
                    new DishInMeal
                    {
                        DishId = paellaValenciana.Id,
                        MealId = meniulZilei2.Id,
                        DishServingSize = 1.0m
                    },
                    new DishInMeal
                    {
                        DishId = limonadaMenta.Id,
                        MealId = meniulZilei2.Id,
                        DishServingSize = 0.75m
                    }
                };
                await context.DishInMeals.AddRangeAsync(dishInMeals).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                Console.WriteLine("Dish-in-meals relations added.");
            }
            else
            {
                Console.WriteLine("Dish-in-meals relations already exist. Skipping seeding.");
            }

            // 7. Adaugă Comenzi (Orders) și Articole Comanda (OrderItems)
            if (!await context.Orders.AnyAsync().ConfigureAwait(false))
            {
                Console.WriteLine("Adding orders...");
                var customerUser = usersInDb.Single(u => u.Email == "customer@example.com");

                var orders = new Order[]
                {
                    new Order
                    {
                        UserId = customerUser.Id,
                        OrderCode = "ORD001",
                        TotalPrice = 100.00m,
                        TransportCost = 10.00m,
                        DiscountPrecentage = 0,
                        Status = "Completed",
                        OrderDate = DateTime.Now.AddDays(-7),
                        EstimatedDeliveryTime = DateTime.Now.AddDays(-7).AddHours(1)
                    },
                    new Order
                    {
                        UserId = customerUser.Id,
                        OrderCode = "ORD002",
                        TotalPrice = 95.00m,
                        TransportCost = 10.00m,
                        DiscountPrecentage = 0.05m,
                        Status = "Pending",
                        OrderDate = DateTime.Now.AddDays(-1),
                        EstimatedDeliveryTime = DateTime.Now.AddDays(-1).AddHours(1)
                    }
                };
                await context.Orders.AddRangeAsync(orders).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                Console.WriteLine("Orders added.");

                Console.WriteLine("Adding order items...");

                var ordersInDb = await context.Orders.ToListAsync().ConfigureAwait(false);

                var orderItems = new OrderItem[]
                {
                    new OrderItem
                    {
                        OrderId = ordersInDb.Single(o => o.OrderCode == "ORD001").Id,
                        DishId = dishes.Single(d => d.Name == "Friptura de Vita cu Cartofi").Id,
                        Qunatity = 1,
                        Type = "Dish",
                        Price = 75.00m,
                        TotalPrice = 75.00m
                    },
                    new OrderItem
                    {
                        OrderId = ordersInDb.Single(o => o.OrderCode == "ORD001").Id,
                        DishId = dishes.Single(d => d.Name == "Tort de Ciocolata").Id,
                        Qunatity = 1,
                        Type = "Dish",
                        Price = 25.00m,
                        TotalPrice = 25.00m
                    },
                    new OrderItem
                    {
                        OrderId = ordersInDb.Single(o => o.OrderCode == "ORD002").Id,
                        MealId = meals.Single(m => m.Name == "Meniul Zilei 1 (Friptura)").Id,
                        Qunatity = 1,
                        Type = "Meal",
                        Price = 80.00m,
                        TotalPrice = 72.00m
                    },
                    new OrderItem
                    {
                        OrderId = ordersInDb.Single(o => o.OrderCode == "ORD002").Id,
                        DishId = dishes.Single(d => d.Name == "Limonada cu Menta").Id,
                        Qunatity = 1,
                        Type = "Dish",
                        Price = 15.00m,
                        TotalPrice = 15.00m
                    }
                };
                await context.OrderItems.AddRangeAsync(orderItems).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                Console.WriteLine("Order items added.");
            }
            else
            {
                Console.WriteLine("Orders and Order Items already exist. Skipping seeding.");
            }

            Console.WriteLine("Database seeding completed successfully.");
        }
    }
}