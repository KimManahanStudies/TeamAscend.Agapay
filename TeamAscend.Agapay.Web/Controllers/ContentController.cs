using Microsoft.AspNetCore.Mvc;
using TeamAscend.Agapay.Web.Attributes;
using TeamAscend.Agapay.Web.Models;
using TeamAscend.Agapay.Web.Shared;

namespace TeamAscend.Agapay.Web.Controllers
{
    [PortalAuthorized]
    public class ContentController : Controller
    {
        [Route("~/Admin/Announcements")]
        public IActionResult Announcements()
        {
            var resp = new List<BlogPost>();

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var posts = (from row in db.BlogPosts where !row.IsDeleted select row).ToList();
                if (posts != null)
                {
                    resp = posts;
                }
            }
            return View("Index",resp);
        }

        [Route("~/Admin/InfoGraphs")]
        public IActionResult InfoGraphs()
        {
            return View("Index");
        }

        [HttpGet]
        public BlogPost GetBlogPost(int ID)
        {
            BlogPost resp = null;

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                resp = (from row in db.BlogPosts where row.ID == ID && !row.IsDeleted select row).FirstOrDefault();
            }

            return resp;
        }

        [HttpGet]
        public int DeleteBlogPost(int ID)
        {
            int res = 0;

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var post = (from row in db.BlogPosts where row.ID == ID && !row.IsDeleted select row).FirstOrDefault();
                if(post != null)
                {
                    post.IsDeleted = true;
                    res = db.SaveChanges();
                }
            }

            return res;
        }

        [HttpPost]
        [Route("~/Content/SaveBlogPost")]
        public IActionResult SaveBlogPost(BlogPost request)
        {
            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var existingPost = (from row in db.BlogPosts where row.ID == request.ID select row).FirstOrDefault();
                if (existingPost != null)
                {
                    existingPost.Title = request.Title;
                    existingPost.Content = request.Content;
                    existingPost.BlogStatus = request.BlogStatus;
                    existingPost.HashSlug = request.HashSlug;
                    existingPost.ModifiedBy = request.ModifiedBy ?? "SYSTEM";
                    existingPost.ModifiedDate = DateTime.Now;
                }
                else
                {
                    request.CreatedBy = request.CreatedBy ?? "SYSTEM";
                    request.ModifiedBy = request.ModifiedBy ?? "SYSTEM";
                    request.CreatedDate = DateTime.Now;
                    request.ModifiedDate = DateTime.Now;
                    request.IsDeleted = false;

                    db.BlogPosts.Add(request);
                }

                db.SaveChanges();
            }

            return Redirect("/Admin/Announcements");
        }
    }
}
