using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using TeamAscend.Agapay.App.Models;
using TeamAscend.Agapay.App.Shared;

namespace TeamAscend.Agapay.App.Components.Pages
{
    public partial class GoPlanLists : ComponentBase
    {
        [Inject]
        public AppShellContext Shell { get; set; }

        [Inject]
        public DatabaseContext DB { get; set; }

        [Inject]
        private NavigationManager Nav { get; set; }

        [Inject]
        private IJSRuntime jsRT { get; set; }

        private List<AppGoPlan> userPlans = new();
        private List<AppGoBag> userBags = new();

        private AppGoPlan selectedPlan;

        private AppGoPlan eCardModel;

        protected override void OnInitialized()
        {
            // Check if user is logged in
            if (!Shell.IsUserLoggedIn || Shell.CurrentUser == null)
            {
                Nav.NavigateTo("/login");
                return;
            }

            LoadUserPlans();
        }

        private void LoadUserPlans()
        {
            eCardModel = new AppGoPlan();
            // Get plans for current user
            userPlans = DB.AppGoPlans?.Where(p => p.UserID == Shell.CurrentUser.ID && !p.IsDeleted).ToList() ?? new List<AppGoPlan>();

            // If no plans exist, redirect to plan creation page
            if (!userPlans.Any())
            {
                Nav.NavigateTo("/createplan");
                return;
            }
            else
            {
                // Load associated bags for each plan
                foreach (var plan in userPlans)
                {
                    var bags = DB.AppGoBags?.Where(b => b.GoPlanID == plan.ID && !b.IsDeleted).ToList() ?? new List<AppGoBag>();
                    userBags.AddRange(bags);
                }
                eCardModel = userPlans.FirstOrDefault();
                StateHasChanged();
            }
        }

        private void NavigateToEdit(int planId)
        {
            Nav.NavigateTo($"/createplan/{planId}");
        }

        private async Task DeletePlan(int planId)
        {
            var plan = userPlans.FirstOrDefault(p => p.ID == planId);
            if (plan != null)
            {
                bool confirmed = await jsRT.InvokeAsync<bool>("confirm", "Are you sure you want to delete this plan?");
                if (confirmed)
                {
                    plan.IsDeleted = true;
                    plan.ModifiedBy = Shell.CurrentUser.Username;
                    plan.ModifiedDate = DateTime.Now;
                    DB.UpdateAppGoPlan(plan);

                    // Also mark associated bags as deleted
                    var associatedBags = userBags.Where(b => b.GoPlanID == planId).ToList();
                    foreach (var bag in associatedBags)
                    {
                        bag.IsDeleted = true;
                        bag.ModifiedBy = Shell.CurrentUser.Username;
                        bag.ModifiedDate = DateTime.Now;
                        DB.UpdateAppGoBag(bag);
                    }

                    LoadUserPlans();
                    StateHasChanged();
                }
            }
        }

        private async Task CopyShareCode(string shareCode)
        {
            await jsRT.InvokeVoidAsync("navigator.clipboard.writeText", shareCode);
            await jsRT.InvokeVoidAsync("alert", "Share code copied to clipboard!");
        }

        private void CreatePlan()
        {
            Nav.NavigateTo("/createplan");
        }

        private void OpenOptionsSheet(AppGoPlan plan)
        {
            selectedPlan = plan;
            jsRT.InvokeVoidAsync("openOptionsSheet");
        }

        private void CloseBottomSheet()
        {
            jsRT.InvokeVoidAsync("closeBottomSheet");
        }

        private void CloseAllSheets()
        {
            jsRT.InvokeVoidAsync("closeBottomSheet");
            jsRT.InvokeVoidAsync("closeOptionsSheet");
        }
    }
}
