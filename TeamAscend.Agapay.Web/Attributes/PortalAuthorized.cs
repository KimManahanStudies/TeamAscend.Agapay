using TeamAscend.Agapay.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;


namespace TeamAscend.Agapay.Web.Attributes
{
    public class PortalAuthorized:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionContext = context.HttpContext;

            try
            {
                bool isAuthorized = false;

                if (actionContext.Request.Cookies.Keys.Contains("AGPSession"))
                {
                    string currentUserCookie = actionContext.Request.Cookies["AGPSession"] ?? "";
                    var translatedKeys = JsonConvert.DeserializeObject<UserAccount>(currentUserCookie);
                    Controller controller = context.Controller as Controller;
                    controller.ViewData["User"] = translatedKeys;
                    isAuthorized = true;
                }

                if (!isAuthorized)
                {
                    context.Result = new RedirectResult("~/Users/Login");
                }

                //base.OnAuthorization(actionContext);
            }
            catch (Exception ex)
            {
                context.Result = new RedirectResult("~/Home/Error");
            }
        }

    }
}
