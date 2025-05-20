using System.Collections.Generic;
using System.Threading.Tasks;
using AppRestaurant.ViewModels.Pages; 

namespace AppRestaurant.Services.Menu;

public interface IMenuService
{
    // Metoda existentă, o putem refolosi sau o putem face să apeleze noua metodă cu filtre implicite
    Task<List<MenuItemModel>> GetAllMenuItemsAsync(); 

    // Noua metodă pentru a prelua elementele din meniu filtrate
    Task<List<MenuItemModel>> GetFilteredMenuItemsAsync(
        string? searchText,           // Textul introdus în bara de căutare
        bool isAllergenSearchActive,  // Indică dacă căutarea se face după alergeni
        bool isExcludeMode);          // Indică dacă se exclud alergenii (modul "fără")
}