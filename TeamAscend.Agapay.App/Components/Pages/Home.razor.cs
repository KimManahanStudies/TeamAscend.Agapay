using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamAscend.Agapay.App.Models;
using TeamAscend.Agapay.App.Shared;
using TeamAscend.Agapay.Web.Services;

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
        
        [Inject]
        private FacebookService FB { get; set; }

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
            Model.Announcements = DB.BlogPosts.Where(r=>r.BlogType == "ANNOUNCEMENT").ToList();
            Model.InfoGraphs = DB.BlogPosts.Where(r=>r.BlogType == "INFOGRAPH").ToList();

            try
            {
                var fbposts = await FB.GetPosts();

                var postLimit = 2;
                foreach (var post in fbposts.Take(postLimit))
                {
                    Model.Announcements = Model.Announcements ?? new List<AppBlogPost>();
                    Model.Announcements.Add(new AppBlogPost()
                    {
                        Title = $"Facebook Alert: {post.message.Substring(0,10)}",
                        Content = post.message,
                        //CreatedAt = DateTime.Parse(post.created_time),
                        BlogType = "ANNOUNCEMENT"
                        
                    });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            await jsRT.InvokeVoidAsync("IsBusy", false);
            await InvokeAsync(StateHasChanged);
            await jsRT.InvokeVoidAsync("loadAnnouncementSlider");
        }

    }
}
