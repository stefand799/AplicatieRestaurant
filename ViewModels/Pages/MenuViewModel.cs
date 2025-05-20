// MenuViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks; // Asigură-te că ai acest using
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppRestaurant.ViewModels; // Asigură-te că ai acest using (pentru ViewModelBase)
using AppRestaurant.Services.Menu; // Asigură-te că ai acest using (pentru IMenuService)

namespace AppRestaurant.ViewModels.Pages
{
    // Asigură-te că este o clasă parțială și moștenește ViewModelBase
    public partial class MenuViewModel : ViewModelBase 
    {
        // FilteredItems este colecția pe care UI-ul o va afișa
        public ObservableCollection<GroupedMenuItems> FilteredItems { get; } = new();

        [ObservableProperty] public bool _isBusy;
        
        // Aceste proprietăți sunt legate direct la elementele UI (TextBox, RadioButtons)
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(AreFiltersActive))]
        private string? filterText; // Textul din bara de căutare

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(AreFiltersActive))]
        private bool isAllergenSearchActive; // True dacă e selectat modul "Nu conține alergen"

        [ObservableProperty]
        private bool isExcludeMode; // True dacă e selectat modul "Nu conține alergen" (modul de excludere)

        // NOU: Proprietatea pentru a controla starea butonului radio "Caută după nume"
        [ObservableProperty]
        private bool isSearchByNameActive; 

        // Proprietate derivată pentru a indica dacă vreun filtru este activ (pentru feedback UI)
        public bool AreFiltersActive => !string.IsNullOrWhiteSpace(FilterText) || IsAllergenSearchActive;

        private readonly IMenuService _menuService; // Serviciul injectat pentru a prelua datele

        public MenuViewModel(IMenuService menuService)
        {
            _menuService = menuService;
            
            // Setează starea inițială a filtrelor la pornire
            IsAllergenSearchActive = false; 
            IsExcludeMode = false; 
            IsSearchByNameActive = true; // Implicit, "Caută după nume" este selectat
        }

        // Metodă parțială apelată automat când FilterText se schimbă (datorită [ObservableProperty])
        partial void OnFilterTextChanging(string? oldValue, string? newValue)
        {
            // Declanșează reîmprospătarea filtrelor la fiecare modificare a textului
            RefreshFilter();
        }

        // Metodă parțială apelată automat când IsAllergenSearchActive se schimbă
        partial void OnIsAllergenSearchActiveChanged(bool value)
        {
            // Sincronizează IsSearchByNameActive cu inversul lui IsAllergenSearchActive
            IsSearchByNameActive = !value; 
            RefreshFilter(); // Declanșează reîmprospătarea filtrelor
        }

        // Metodă parțială apelată automat când IsExcludeMode se schimbă
        partial void OnIsExcludeModeChanged(bool value)
        {
            RefreshFilter(); // Declanșează reîmprospătarea filtrelor
        }

        // Comanda legată de evenimentul Loaded din XAML pentru a iniția încărcarea datelor
        [RelayCommand]
        public async Task LoadMenuItems() // Numele metodei ar trebui să fie LoadMenuItems (nu LoadMenu ITems)
        {
            // Apelăm RefreshFilter pentru a încărca datele inițiale și a le filtra, dacă e cazul.
            await RefreshFilter().ConfigureAwait(false);
        }
        
        // Comandă pentru a reîmprospăta și aplica filtrele
        [RelayCommand]
        private async Task RefreshFilter()
        {
            IsBusy = true; // Activează indicatorul de ocupat (din ViewModelBase)
            try
            {
                // Apelez metoda de filtrare din serviciul IMenuService
                var fetchedItems = await _menuService.GetFilteredMenuItemsAsync(
                    FilterText, 
                    IsAllergenSearchActive, 
                    IsExcludeMode).ConfigureAwait(false);

                // Golește colecția curentă și adaugă elementele filtrate
                FilteredItems.Clear();

                // Grupează elementele după categorie și adaugă-le în FilteredItems
                var groupedItems = fetchedItems
                    .GroupBy(item => item.Category ?? "Diverse") // Grupează după categorie, sau "Diverse" dacă e null
                    .OrderBy(g => g.Key) // Sortează grupurile după numele categoriei
                    .Select(g => new GroupedMenuItems(g.Key, g.OrderBy(item => item.Name))); // Creează obiecte GroupedMenuItems

                foreach (var group in groupedItems)
                {
                    FilteredItems.Add(group);
                }
            }
            catch (Exception ex)
            {
                // Tratează orice erori apărute la preluarea datelor
                Console.WriteLine($"Error fetching menu items: {ex.Message}");
                // Poți adăuga aici un mecanism de notificare a utilizatorului
            }
            finally
            {
                IsBusy = false; // Dezactivează indicatorul de ocupat
            }
        }

        // Comandă pentru setarea modului "Nu conține alergen"
        [RelayCommand]
        private void SetAllergenExcludeMode()
        {
            IsAllergenSearchActive = true;
            IsExcludeMode = true; 
            // RefreshFilter() va fi apelat automat prin OnIsAllergenSearchActiveChanged sau OnIsExcludeModeChanged
        }
        
        // Comandă pentru setarea modului "Caută după nume"
        [RelayCommand]
        private void SetSearchByNameMode()
        {
            IsAllergenSearchActive = false;
            IsExcludeMode = false;
            // RefreshFilter() va fi apelat automat prin OnIsAllergenSearchActiveActiveChanged
        }
    }

    // Modelele MenuItemModel și GroupedMenuItems rămân la fel ca înainte
    public class MenuItemModel
    {
        public string? Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PortionQuantity { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string[] Allergens { get; set; } = Array.Empty<string>();
        public string? ImagePath { get; set; }
        public bool IsAvailable { get; set; }

        public string AllergensString =>
            Allergens != null && Allergens.Length > 0
                ? string.Join(", ", Allergens)
                : "-";
    }

    public class GroupedMenuItems : ObservableCollection<MenuItemModel>
    {
        public string CategoryName { get; }

        public GroupedMenuItems(string categoryName, IEnumerable<MenuItemModel> items) : base(items)
        {
            CategoryName = categoryName;
        }
    }
}