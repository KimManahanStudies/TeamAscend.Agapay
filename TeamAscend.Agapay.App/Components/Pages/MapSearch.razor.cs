using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamAscend.Agapay.App.Models;
using TeamAscend.Agapay.App.Shared;

namespace TeamAscend.Agapay.App.Components.Pages
{
    public partial class MapSearch : ComponentBase
    {
        [Inject]
        private IJSRuntime jsRT { get; set; }

        [Inject]
        private AppShellContext Shell { get; set; }

        [Inject]
        private DatabaseContext DbContext { get; set; }

        private readonly string[] locationTypes = new[]
        {
            "all",
            "Hospital",
            "Evacuation Site",
            "Supply Store",
            "Government Centers",
            "Relief Warehouse",
            "Water Refill Station",
            "Police",
            "Fire",
            "Shelter",
            "Others"
        };

        private List<AppMapLocation> allLocations = new();
        private List<AppMapLocation> filteredLocations = new();
        private int currentFilterIndex = 0;
        private string currentFilter = "all";
        private string searchText = "";

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(1000);
            await jsRT.InvokeVoidAsync("clearMap");
            await LoadLocations();
        }

        private async Task LoadLocations()
        {
            allLocations = DbContext.MapLocations.Where(l => !l.IsDeleted).ToList();
            await FilterLocations(0); // Default to "all"
        }

        private async Task FilterLocations(int filterIndex)
        {
            string filterType = locationTypes[filterIndex];
            currentFilterIndex = filterIndex;
            currentFilter = filterType;
            filteredLocations = filterType == "all"
                ? allLocations
                : allLocations.Where(l => l.LocationType == filterType).ToList();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filteredLocations = filteredLocations
                    .Where(l => l.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                               l.Address.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            await jsRT.InvokeVoidAsync("initMap", JsonConvert.SerializeObject(filteredLocations));
            StateHasChanged();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            
        }

        private async Task OnSearchInput(ChangeEventArgs e)
        {
            searchText = e.Value?.ToString() ?? "";
            await FilterLocations(currentFilterIndex);
        }
    }
}
