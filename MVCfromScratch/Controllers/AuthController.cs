using MVCfromScratch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Web.Mvc;
using System.Security.Claims;

namespace MVCfromScratch.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel {
                ReturnUrl = returnUrl
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(LoginModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            if (model.Email == "admin@admin.com" && model.Password == "123456") {
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, "Josh"),
                    new Claim(ClaimTypes.Email, "josh@joshenglish.com"),
                    new Claim(ClaimTypes.Country, "USA")
                },
                    "ApplicationCookie");
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identity);
                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }
            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }
        private string GetRedirectUrl(string returnUrl) {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl)) {
                return Url.Action("index", "home");
            }
            return returnUrl;
        }
        public ActionResult Logout() {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login", "Auth");
        }
    }
}