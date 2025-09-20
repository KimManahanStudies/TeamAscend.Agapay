using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TeamAscend.Agapay.Web.Services
{
    public class FacebookService
    {
        public const string ACCESS_TOKEN = "";

        public const string POSTGETURL = "https://graph.facebook.com/v23.0/759242803941026/feed";

        public static async Task<List<FacebookPagePost>> GetPosts()
        {
            List<FacebookPagePost> resp = new List<FacebookPagePost>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

                var rawResp1 = await httpClient.GetStringAsync(POSTGETURL);
                var pagefeeds = JsonConvert.DeserializeObject<FacebookPageFeed>(rawResp1);

                if (pagefeeds != null)
                {
                    resp = pagefeeds.data;
                }
            }

            return resp;
        }
    }


    public class FacebookPagePost
    {
        public string id { get; set; }
        public string message { get; set; }
        public string created_time { get; set; }
    }

    public class FacebookPagePaging
    {
        public FacebookPageCursor cursors { get; set; }
    }

    public class FacebookPageCursor
    {
        public string before { get; set; }
        public string after { get; set; }
    }

    public class FacebookPageFeed
    {
        public List<FacebookPagePost> data { get; set; }
        public FacebookPagePaging paging { get; set; }
    }
}
