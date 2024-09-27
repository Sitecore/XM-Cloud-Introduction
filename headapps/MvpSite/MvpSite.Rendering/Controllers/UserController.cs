using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;

namespace MvpSite.Rendering.Controllers;

public class UserController : Controller
{
    public IActionResult SignIn()
    {
        if (!HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            AuthenticationProperties properties = new()
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

    public new IActionResult SignOut()
    {
        return new SignOutResult(
            [
                OktaDefaults.MvcAuthenticationScheme,
                CookieAuthenticationDefaults.AuthenticationScheme
            ],
            new AuthenticationProperties { RedirectUri = "/" });
    }
}