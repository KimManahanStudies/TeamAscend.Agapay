using Microsoft.AspNetCore.Components;
using TeamAscend.Agapay.App.Models;
using TeamAscend.Agapay.App.Shared;

namespace TeamAscend.Agapay.App.Components.Pages
{
    public partial class CreatePlan : ComponentBase
    {
        [Inject]
        private NavigationManager Nav { get; set; }

        [Inject]
        private DatabaseContext DB { get; set; }

        [Inject]
        private AppShellContext Shell { get; set; }

        private AppGoPlan Model { get; set; }

        protected override void OnInitialized()
        {
            Model = new AppGoPlan();
        }

        private void SavePlan()
        {
            // Set current user details and metadata
            var currentUser = Shell.GetSessionUser();
            if (currentUser != null)
            {
                Model.UserID = currentUser.ID;
                Model.CreatedBy = currentUser.Username;
                Model.CreatedDate = DateTime.Now;
                Model.ModifiedBy = currentUser.Username;
                Model.ModifiedDate = DateTime.Now;
                Model.IsDeleted = false;
            }

            // Save to local database
            DB.Init();
            DB.SaveAppGoPlan(Model);

            // Redirect to plan list
            Nav.NavigateTo("/goplanlists");
        }

        private void GotoList()
        {
            Nav.NavigateTo("/goplanlists");
        }
    }
}
