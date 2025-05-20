// MenuService.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRestaurant.Data;
using AppRestaurant.Models;
using AppRestaurant.ViewModels.Pages;
using Microsoft.EntityFrameworkCore;

namespace AppRestaurant.Services.Menu;

public class MenuService : IMenuService
{
    private readonly AppRestaurantDbContext _context;

    public MenuService(AppRestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<List<MenuItemModel>> GetAllMenuItemsAsync()
    {
        // Această metodă va fi folosită ca bază pentru filtrare,
        // aducând toate elementele din meniu.
        var menuItems = new List<MenuItemModel>();

        var dishes = await _context.Dishes
            .Include(d => d.Category)
            .Include(d => d.Allergens)
            .Where(d => d.Availability) // Doar preparatele disponibile
            .ToListAsync();

        foreach (var dish in dishes)
        {
            menuItems.Add(new MenuItemModel
            {
                Category = dish.Category?.Name,
                Name = dish.Name,
                PortionQuantity = $"{dish.ServingSize}{dish.ServingUnit}",
                Price = dish.Price,
                Allergens = dish.Allergens.Select(a => a.Name).ToArray(),
                ImagePath = dish.ImagePath,
                IsAvailable = dish.Availability
            });
        }

        var meals = await _context.Meals
            .Include(m => m.Category)
            .Include(m => m.DishInMeals)
                .ThenInclude(dim => dim.Dish)
                    .ThenInclude(d => d.Allergens) // Include alergenii fiecărui Dish din Meal
            .Where(m => m.DishInMeals.All(dim => dim.Dish.Availability)) // Un meal este disponibil dacă toate dish-urile sale sunt disponibile
            .ToListAsync();

        foreach (var meal in meals)
        {
            decimal finalPrice = meal.BasePrice * (1 - (meal.DiscountPrecentage / 100));

            var mealAllergens = meal.DishInMeals
                .SelectMany(dim => dim.Dish.Allergens)
                .Select(a => a.Name)
                .Distinct()
                .ToArray();

            string mealPortionQuantity = meal.DishInMeals.Any()
                                         ? $"Meniu cu {meal.DishInMeals.Count} preparate"
                                         : "Meniu";
            
            bool isMealAvailable = meal.DishInMeals.Any() ? meal.DishInMeals.All(dim => dim.Dish.Availability) : true;

            menuItems.Add(new MenuItemModel
            {
                Category = meal.Category?.Name,
                Name = meal.Name,
                PortionQuantity = mealPortionQuantity,
                Price = finalPrice,
                Allergens = mealAllergens,
                ImagePath = meal.ImagePath,
                IsAvailable = isMealAvailable
            });
        }

        return menuItems;
    }

    // Noua metodă care implementează logica de filtrare
    public async Task<List<MenuItemModel>> GetFilteredMenuItemsAsync(string? filterText, bool isAllergenSearchActive, bool isExcludeMode)
    {
        var allItems = await GetAllMenuItemsAsync(); // Obținem toate elementele din meniu

        var filteredItems = allItems.AsEnumerable(); // Folosim AsEnumerable pentru a filtra în memorie

        if (!string.IsNullOrWhiteSpace(filterText))
        {
            var lowerCaseFilterText = filterText.ToLower();

            if (isAllergenSearchActive)
            {
                if (isExcludeMode)
                {
                    // Exclude elementele care CONȚIN alergenul specificat
                    filteredItems = filteredItems.Where(item => 
                        !item.Allergens.Any(allergen => allergen.ToLower().Contains(lowerCaseFilterText)));
                }
                else
                {
                    // Include elementele care CONȚIN alergenul specificat
                    filteredItems = filteredItems.Where(item => 
                        item.Allergens.Any(allergen => allergen.ToLower().Contains(lowerCaseFilterText)));
                }
            }
            else
            {
                // Caută după nume (normal)
                filteredItems = filteredItems.Where(item => 
                    item.Name.ToLower().Contains(lowerCaseFilterText));
            }
        }
        
        return filteredItems.ToList();
    }
}