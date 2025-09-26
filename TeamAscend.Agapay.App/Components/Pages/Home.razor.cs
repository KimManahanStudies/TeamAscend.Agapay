using Microsoft.AspNetCore.Components;
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
        public HomeVM Model { get; set; }

        public Home()
        {
            Model = new HomeVM() { 
                FBPosts = new List<FacebookPagePost>()            
            };
        }

        protected override async Task OnInitializedAsync()
        {
            Model = new HomeVM();
            //Model.FBPosts = await FacebookService.GetPosts();
            await InvokeAsync(StateHasChanged);
        }

    }
}
