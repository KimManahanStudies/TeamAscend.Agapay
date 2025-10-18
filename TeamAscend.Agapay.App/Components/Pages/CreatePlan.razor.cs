using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using TeamAscend.Agapay.App.Models;
using TeamAscend.Agapay.App.Shared;

namespace TeamAscend.Agapay.App.Components.Pages
{
    public partial class CreatePlan : ComponentBase
    {
        [Parameter]
        public string PlanId { get; set; }

        [Inject]
        private NavigationManager Nav { get; set; }

        [Inject]
        private DatabaseContext DB { get; set; }

        [Inject]
        private AppShellContext Shell { get; set; }

        private AppGoPlan Model { get; set; }
        private Dictionary<string, List<ChecklistItem>> ChecklistCategories { get; set; }
        private bool IsEditMode => !string.IsNullOrEmpty(PlanId);

        [Inject]
        private ILogger<CreatePlan> Logger { get; set; }

        protected override void OnInitialized()
        {
            InitializeChecklist();
            LoadPlan();
        }

        private void LoadPlan()
        {
            if (IsEditMode && int.TryParse(PlanId, out int planId))
            {
                Logger.LogInformation($"Loading plan with ID: {planId}");
                
                // Load existing plan
                Model = DB.AppGoPlans?.FirstOrDefault(p => p.ID == planId && !p.IsDeleted);
                if (Model == null)
                {
                    Logger.LogWarning($"Plan with ID {planId} not found");
                    Nav.NavigateTo("/goplanlists");
                    return;
                }

                // Load associated GoBag items
                DB.Init();
                var bagItems = DB.AppGoBags?.Where(b => b.GoPlanID == planId && !b.IsDeleted).ToList();
                
                Logger.LogInformation($"Found {bagItems?.Count ?? 0} bag items for plan {planId}");

                if (bagItems != null && bagItems.Any())
                {
                    // Mark checkboxes for existing items
                    foreach (var bag in bagItems)
                    {
                        Logger.LogInformation($"Processing bag item: Category={bag.Category}, Description={bag.Description}");

                        if (ChecklistCategories.TryGetValue(bag.Category, out var categoryItems))
                        {
                            var matchingItem = categoryItems.FirstOrDefault(i => 
                                string.Equals(i.Description.Trim(), bag.Description.Trim(), 
                                StringComparison.OrdinalIgnoreCase));

                            if (matchingItem != null)
                            {
                                matchingItem.IsChecked = true;
                                Logger.LogInformation($"Matched and checked item: {matchingItem.Description}");
                            }
                            else
                            {
                                Logger.LogWarning($"No matching checklist item found for: {bag.Description}");
                            }
                        }
                        else
                        {
                            Logger.LogWarning($"Category not found: {bag.Category}");
                        }
                    }
                }

                // Force UI refresh
                StateHasChanged();
            }
            else
            {
                Model = new AppGoPlan();
            }
        }

        private class ChecklistItem
        {
            public string Description { get; set; }
            public bool IsChecked { get; set; }
            public string Category { get; set; }
        }

        private void InitializeChecklist()
        {
            ChecklistCategories = new Dictionary<string, List<ChecklistItem>>
            {
                ["Food"] = new List<ChecklistItem>
                {
                    new ChecklistItem { Description = "Non-perishable food (biscuits, crackers, cookies, cereals, canned goods, cup noodles)", Category = "Food" },
                    new ChecklistItem { Description = "1.5L of drinking water, instant coffee, and tea", Category = "Food" },
                    new ChecklistItem { Description = "Eating utensils (spoon, fork, plate)", Category = "Food" },
                    new ChecklistItem { Description = "Plastic bag", Category = "Food" }
                },
                ["Toiletries"] = new List<ChecklistItem>
                {
                    new ChecklistItem { Description = "Toothbrush, toothpaste, mouthwash", Category = "Toiletries" },
                    new ChecklistItem { Description = "Sanitary napkin", Category = "Toiletries" },
                    new ChecklistItem { Description = "Soap, shampoo, conditioner", Category = "Toiletries" },
                    new ChecklistItem { Description = "Hand sanitizer or alcohol", Category = "Toiletries" },
                    new ChecklistItem { Description = "Insect repellant", Category = "Toiletries" },
                    new ChecklistItem { Description = "Sunblock", Category = "Toiletries" }
                },
                ["First Aid Kit"] = new List<ChecklistItem>
                {
                    new ChecklistItem { Description = "Band-aid and sterile gauze", Category = "First Aid Kit" },
                    new ChecklistItem { Description = "Micropore tape", Category = "First Aid Kit" },
                    new ChecklistItem { Description = "Povidone-iodine and alcohol", Category = "First Aid Kit" },
                    new ChecklistItem { Description = "Ointment for wounds", Category = "First Aid Kit" },
                    new ChecklistItem { Description = "Mefenamic acid and paracetamol", Category = "First Aid Kit" },
                    new ChecklistItem { Description = "Maintenance medication", Category = "First Aid Kit" }
                },
                ["Survival Kit"] = new List<ChecklistItem>
                {
                    new ChecklistItem { Description = "Pocket knife with can opener", Category = "Survival Kit" },
                    new ChecklistItem { Description = "Flashlight with extra batteries, matches, or lighter (kept in a zip lock bag)", Category = "Survival Kit" },
                    new ChecklistItem { Description = "Rope, fishing hook, fishing line", Category = "Survival Kit" },
                    new ChecklistItem { Description = "Whistle or anything that makes noise to signal for help", Category = "Survival Kit" }
                },
                ["PPEs and Technical Equipment"] = new List<ChecklistItem>
                {
                    new ChecklistItem { Description = "Safety goggles, dust mask, N95 mask, surgical gloves, raincoat, or poncho", Category = "PPEs and Technical Equipment" },
                    new ChecklistItem { Description = "Small radio, batteries, powerbank, charger, extension cord", Category = "PPEs and Technical Equipment" }
                },
                ["Clothes and Bedding"] = new List<ChecklistItem>
                {
                    new ChecklistItem { Description = "Lightweight shoes or slippers", Category = "Clothes and Bedding" },
                    new ChecklistItem { Description = "Jacket and warm clothes", Category = "Clothes and Bedding" },
                    new ChecklistItem { Description = "Blanket or sleeping bag", Category = "Clothes and Bedding" },
                    new ChecklistItem { Description = "Clothes good for at least three days", Category = "Clothes and Bedding" }
                }
            };
        }

        private async void SavePlan()
        {
            var currentUser = Shell.GetSessionUser();
            if (currentUser != null)
            {
                if (!IsEditMode)
                {
                    // Set creation metadata for new plan
                    Model.UserID = currentUser.ID;
                    Model.CreatedBy = currentUser.Username;
                    Model.CreatedDate = DateTime.Now;
                    
                    // Generate share code for new plans only
                    Model.ShareCode = GenerateShareCode();
                }

                // Update modification metadata
                Model.ModifiedBy = currentUser.Username;
                Model.ModifiedDate = DateTime.Now;
                Model.IsDeleted = false;
            }

            DB.Init();
            int planId;

            if (IsEditMode)
            {
                // Update existing plan
                DB.UpdateAppGoPlan(Model);
                planId = Model.ID;

                // Delete existing bag items
                var existingBags = DB.AppGoBags?.Where(b => b.GoPlanID == planId).ToList();
                if (existingBags != null)
                {
                    foreach (var bag in existingBags)
                    {
                        bag.IsDeleted = true;
                        bag.ModifiedBy = currentUser?.Username;
                        bag.ModifiedDate = DateTime.Now;
                        DB.UpdateAppGoBag(bag);
                    }
                }
            }
            else
            {
                // Save new plan
                planId = DB.SaveAppGoPlan(Model);
            }

            // Save checked items as GoBag records
            foreach (var category in ChecklistCategories)
            {
                var categoryName = category.Key; // Use the exact category name
                foreach (var item in category.Value.Where(x => x.IsChecked))
                {
                    var goBag = new AppGoBag
                    {
                        GoPlanID = planId,
                        UserID = currentUser?.ID ?? 0,
                        Category = categoryName, // Use the exact category name
                        Description = item.Description.Trim(), // Trim the description
                        IsDeleted = false,
                        CreatedBy = currentUser?.Username,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = currentUser?.Username,
                        ModifiedDate = DateTime.Now
                    };

                    DB.SaveAppGoBag(goBag);
                }
            }

            // Redirect to plan list
            Nav.NavigateTo("/goplanlists");
        }

        private void GotoList()
        {
            Nav.NavigateTo("/goplanlists");
        }

        private string GenerateShareCode()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string numbers = "0123456789";

            // Generate first part (2 numbers + 2 letters)
            string part1 = $"{random.Next(10)}{random.Next(10)}" +
                           new string(Enumerable.Repeat(chars, 2)
                           .Select(s => s[random.Next(s.Length)])
                           .ToArray());

            // Generate second part (4 numbers)
            string part2 = new string(Enumerable.Repeat(numbers, 4)
                           .Select(s => s[random.Next(s.Length)])
                           .ToArray());

            // Generate third part (2 letters + 2 numbers)
            string part3 = new string(Enumerable.Repeat(chars, 2)
                           .Select(s => s[random.Next(s.Length)])
                           .ToArray()) +
                           $"{random.Next(10)}{random.Next(10)}";

            return $"{part1}-{part2}-{part3}";
        }
    }
}
