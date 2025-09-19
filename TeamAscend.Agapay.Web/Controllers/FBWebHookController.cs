using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TeamAscend.Agapay.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FBWebHookController : ControllerBase
    {
        [HttpGet]
        public string callbackverify(string hub_mode,string hub_challenge,string hub_verify_token)
        {
            Response.ContentType = "plain/text";
            return hub_challenge;
        }
    }
}
