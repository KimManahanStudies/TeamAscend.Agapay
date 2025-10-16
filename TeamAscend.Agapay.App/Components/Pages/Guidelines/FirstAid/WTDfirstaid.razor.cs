using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamAscend.Agapay.App.Models;
using TeamAscend.Agapay.App.Shared;

namespace TeamAscend.Agapay.App.Components.Pages.Guidelines.FirstAid
{
    public partial class WTDfirstaid:ComponentBase
    {
        [Inject]
        private IJSRuntime jsRT { get; set; }

        [Inject]
        private AppShellContext Shell { get; set; }

        [Inject]
        private DatabaseContext DB { get; set; }

        [Inject]
        private NavigationManager Nav { get; set; }

        public GuidelinesViewModel Model { get; set; }

        private string activeTab = "guide";

        public WTDfirstaid()
        {
            Model = new GuidelinesViewModel();
            Model.Contents = new List<AppBlogPost>();
        }

        protected override async Task OnInitializedAsync()
        {
            await jsRT.InvokeVoidAsync("IsBusy", true, "Syncing...");
            await Shell.SyncData(DB);

            if (DB.BlogPosts != null)
            {
                Model.Contents = DB.BlogPosts.Where(r=>r.BlogType == "GUIDE" && !r.IsDeleted).ToList();
            }
            

            await jsRT.InvokeVoidAsync("IsBusy", false);
            await InvokeAsync(StateHasChanged);
        }

        private class FirstAidCard
        {
            public string Title { get; set; }
            public string ImageUrl { get; set; }
            public bool IsBookmarked { get; set; }
        }

        private List<FirstAidCard> firstAidCards = new()
    {
        new FirstAidCard { Title = "Basic CPR", ImageUrl = "imgs/wtd_banners/wtdfa_banners/wtd_cpr.jpeg", IsBookmarked = false },
        new FirstAidCard { Title = "Healing Burns", ImageUrl = "imgs/wtd_banners/wtdfa_banners/wtd_burns.jpeg", IsBookmarked = false },
        new FirstAidCard { Title = "Splinting", ImageUrl = "imgs/wtd_banners/wtdfa_banners/wtd_splinting.jpeg", IsBookmarked = false },
        new FirstAidCard { Title = "Choking", ImageUrl = "imgs/wtd_banners/wtdfa_banners/wtd_choking.jpeg", IsBookmarked = false },
        new FirstAidCard { Title = "Bleeding", ImageUrl = "imgs/wtd_banners/wtdfa_banners/wtd_bleeding.jpeg", IsBookmarked = false },
        new FirstAidCard { Title = "Minor Cuts", ImageUrl = "imgs/wtd_banners/wtdfa_banners/wtd_cuts.jpeg", IsBookmarked = false },
        new FirstAidCard { Title = "Hyperthermia", ImageUrl = "imgs/wtd_banners/wtdfa_banners/wtd_hyperthermia.jpeg", IsBookmarked = false },
        new FirstAidCard { Title = "Heatstroke", ImageUrl = "imgs/wtd_banners/wtdfa_banners/wtd_heatstroke.jpeg", IsBookmarked = false }
    };

        private void SwitchTab(string tab)
        {
            activeTab = tab;
            StateHasChanged();
        }

        private void ToggleBookmark(FirstAidCard card)
        {
            card.IsBookmarked = !card.IsBookmarked;
            StateHasChanged();
        }

        private void NavigateBack()
        {
            // Navigation logic here
        }

        private void NavigateToDetail(FirstAidCard card)
        {
            switch (card.Title)
            {
                case "Basic CPR":
                    Nav.NavigateTo("/WTDcpr");
                    break;
                case "Healing Burns":
                    Nav.NavigateTo("/WTDburns");
                    break;
                case "Splinting":
                    Nav.NavigateTo("/WTDsplinting");
                    break;
                case "Choking":
                    Nav.NavigateTo("/WTDchoking");
                    break;
                case "Bleeding":
                    Nav.NavigateTo("/WTDbleeding");
                    break;
                case "Minor Cuts":
                    Nav.NavigateTo("/WTDminorcutswounds");
                    break;
                case "Hyperthermia":
                    Nav.NavigateTo("/WTDhyperthermia");
                    break;
                case "Heatstroke":
                    Nav.NavigateTo("/WTDstroke");
                    break;
                    // Add other cases as you create more detail pages
            }
        }

        private void NavigateToGuide(int ContentID)
        {
            Nav.NavigateTo($"/guidecontent/{ContentID}");
        }
    }
}
