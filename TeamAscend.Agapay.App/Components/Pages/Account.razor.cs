using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamAscend.Agapay.App.Model;
using TeamAscend.Agapay.App.Shared;

namespace TeamAscend.Agapay.App.Components.Pages
{
    public partial class Account : ComponentBase
    {
        [Inject]
        public AppShellContext Shell { get; set; }

        [Inject]
        public DatabaseContext DB { get; set; }

        [Inject]
        private NavigationManager Nav { get; set; }

        [Inject]
        private IJSRuntime jsRT { get; set; }  

        private AppUser Model { get; set; }

        public Account()
        {
            Model = new AppUser();
        }

        protected override void OnInitialized()
        {
            // Load user info when component initializes
            if (Shell.IsUserLoggedIn)
            {
                Model = Shell.CurrentUser;
            }
            
            
            //// If no user is logged in, redirect to home
            //if (Model == null || !Shell.IsUserLoggedIn)
            //{
            //    Nav.NavigateTo("/", true);
            //}
        }

        private void HandleLogout()
        {
            // Clear the session
            Shell.ClearSessionUser();
            Shell.CurrentUser = null;
            Shell.IsUserLoggedIn = false;

            // Redirect to home page
            Nav.NavigateTo("/", true);
        }

        private string GetFullName()
        {
            if (Model == null) return "";
            return $"{Model.FirstName} {Model.LastName}".ToUpper();
        }
    }
}
