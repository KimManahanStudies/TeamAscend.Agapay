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
        private List<AppGoPlanMember> sharedPlans = new();

        private AppGoPlan selectedPlan;
        private bool isSharedPlan = false;

        private AppGoPlan eCardModel;
        
        private string shareCodeInput = string.Empty;

        protected override void OnInitialized()
        {
            // Check if user is logged in
            if (!Shell.IsUserLoggedIn || Shell.CurrentUser == null)
            {
                Nav.NavigateTo("/login");
                return;
            }

            LoadUserPlans();
            LoadSharedPlans();
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

        private void LoadSharedPlans()
        {
            // Get plans shared with current user
            DB.Init();
            var planMemberships = DB.AppGoPlanMembers?.Where(m => m.MemberID == Shell.CurrentUser.ID && !m.IsDeleted).ToList() ?? new List<AppGoPlanMember>();
            sharedPlans = planMemberships;
            StateHasChanged();
        }

        private void NavigateToEdit(int planId)
        {
            Nav.NavigateTo($"/createplan/{planId}");
        }

        private void NavigateToView(int planId)
        {
            Nav.NavigateTo($"/createplan/{planId}/view");
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
            OpenAddSheet();
        }

        private void OpenAddSheet()
        {
            shareCodeInput = string.Empty;
            jsRT.InvokeVoidAsync("openAddSheet");
        }

        private void CloseAddSheet()
        {
            jsRT.InvokeVoidAsync("closeAddSheet");
        }

        private async Task JoinSharedPlan()
        {
            if (string.IsNullOrWhiteSpace(shareCodeInput))
            {
                await jsRT.InvokeVoidAsync("alert", "Please enter a share code.");
                return;
            }

            DB.Init();
            
            // Find the plan with the matching share code
            var sharedPlan = DB.AppGoPlans?.FirstOrDefault(p => 
                p.ShareCode == shareCodeInput.Trim() && !p.IsDeleted);

            if (sharedPlan == null)
            {
                await jsRT.InvokeVoidAsync("alert", "Invalid share code. Plan not found.");
                return;
            }

            // Check if user is already a member of this plan
            var existingMembership = DB.AppGoPlanMembers?.FirstOrDefault(m => 
                m.GoPlanID == sharedPlan.ID && 
                m.MemberID == Shell.CurrentUser.ID && 
                !m.IsDeleted);

            if (existingMembership != null)
            {
                await jsRT.InvokeVoidAsync("alert", "You are already a member of this plan.");
                return;
            }

            // Create membership record
            var membership = new AppGoPlanMember
            {
                GoPlanID = sharedPlan.ID,
                PlanOwnerID = sharedPlan.UserID,
                MemberID = Shell.CurrentUser.ID,
                PhoneNumber = Shell.CurrentUser.ContactNo1,
                FamilyMemberRole = "Member",
                IsDeleted = false,
                CreatedBy = Shell.CurrentUser.Username,
                CreatedDate = DateTime.Now,
                ModifiedBy = Shell.CurrentUser.Username,
                ModifiedDate = DateTime.Now
            };

            // Save membership
            DB.SaveAppGoPlanMember(membership);
            
            // Reload shared plans
            LoadSharedPlans();
            CloseAddSheet();
            
            await jsRT.InvokeVoidAsync("alert", $"Successfully joined '{sharedPlan.Name}' plan!");
            StateHasChanged();
        }

        private void OpenOptionsSheet(AppGoPlan plan, bool isShared = false)
        {
            selectedPlan = plan;
            isSharedPlan = isShared;
            jsRT.InvokeVoidAsync("openOptionsSheet");
        }

        private void CloseBottomSheet()
        {
            jsRT.InvokeVoidAsync("closeBottomSheet");
        }

        private void CloseAllSheets()
        {
            jsRT.InvokeVoidAsync("closeAddSheet");
            jsRT.InvokeVoidAsync("closeOptionsSheet");
        }
    }
}
