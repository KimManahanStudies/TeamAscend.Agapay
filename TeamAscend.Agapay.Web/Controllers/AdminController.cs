using Microsoft.AspNetCore.Mvc;
using TeamAscend.Agapay.Web.Models;
using TeamAscend.Agapay.Web.Shared;

namespace TeamAscend.Agapay.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return RedirectToAction("Index");
        }

        public IActionResult Users()
        {
            var resp = new UsersPageViewModel();

            resp.Users = new List<UserAccount>();

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var users = (from row in db.UserAccounts where !row.IsDeleted select row).ToList();
                if (users != null)
                {
                    resp.Users = users;
                }
            }
            return View(resp);
        }

        public IActionResult Phonebook()
        {
            return View();
        }

        public IActionResult BlogPosts()
        {
            return View();
        }

        public IActionResult ResourceLocator()
        {
            return View();
        }

        public IActionResult NewsArticles()
        {
            return View();
        }
    }
}
