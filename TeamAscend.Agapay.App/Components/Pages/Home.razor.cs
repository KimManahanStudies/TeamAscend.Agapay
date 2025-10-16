using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamAscend.Agapay.App.Models;
using TeamAscend.Agapay.App.Shared;

namespace TeamAscend.Agapay.App.Components.Pages
{
    public partial class Home:ComponentBase
    {
        public static Home Instance { get; private set; }

        [Inject]
        private IJSRuntime jsRT { get; set; }
        
        [Inject]
        private AppShellContext Shell { get; set; }

        [Inject]
        private DatabaseContext DB { get; set; }

        public HomeViewModel Model { get; set; }

        public Home()
        {
            Instance = this;
        }

        protected override async Task OnInitializedAsync()
        {
            Model = new HomeViewModel();
            await jsRT.InvokeVoidAsync("IsBusy",true,"Syncing...");
            await Shell.SyncData(DB);

            Model.Contents = DB.BlogPosts;

            await jsRT.InvokeVoidAsync("IsBusy", false);
            await InvokeAsync(StateHasChanged);
        }

    }
}
