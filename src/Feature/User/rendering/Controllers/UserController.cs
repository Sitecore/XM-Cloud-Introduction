using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;

namespace Mvp.Feature.Users.Rendering.Controllers
{
    public class UserController : Controller
    {
        public IActionResult SignIn()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                var properties = new AuthenticationProperties
                {
                    RedirectUri = "/"
                };

                return Challenge(properties, OktaDefaults.MvcAuthenticationScheme);
            }

            return Redirect("/");
        }

        public IActionResult Claims()
        {
            return View();
        }

        public IActionResult SignOut()
        {
            return new SignOutResult(
                new[]
                {
                     OktaDefaults.MvcAuthenticationScheme,
                     CookieAuthenticationDefaults.AuthenticationScheme,
                },
                new AuthenticationProperties { RedirectUri = "/" });
        }
    }
}