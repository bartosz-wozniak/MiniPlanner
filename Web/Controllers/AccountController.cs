using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.DtoObjects;
using BusinessLogic.Logic;
using Microsoft.AspNet.Identity.Owin;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var a = await AuthenticationLogic.AuthenticateUser(model.Login, model.Password);
            var result = a != "Unauthorized" ? SignInStatus.Success : SignInStatus.Failure;
            //result = await SignInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    FormsAuthentication.RedirectFromLoginPage(model.Login, true);
                    return new ViewResult();
                default:
                    ModelState.AddModelError("", "Logowanie nie powiodło się.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> Register(string login)
        {
            var ret = new DtoUser();
            if (login == System.Web.HttpContext.Current.User.Identity.Name)
                ret = await new UserLogic().GetUser(login);
            return View("Register", new RegisterViewModel { Id = ret.Id, Login = login, StudentCardId = ret.CardId, AverageScore = ret.AverageScore.ToString(CultureInfo.CurrentCulture) == "0" ? "" : ret.AverageScore.ToString(CultureInfo.CurrentCulture) });
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (await new RegistrationLogic().GetStatus() != "Otwarta")
            {
                ModelState.AddModelError("", "Rejestracja nie jest obecnie otwarta. ");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    NumberFormatInfo provider = new NumberFormatInfo();
                    if (model.AverageScore.Contains(","))
                    {
                        provider = new NumberFormatInfo
                        {
                            NumberDecimalSeparator = ",",
                        };
                    }
                    else if (model.AverageScore.Contains("."))
                    {
                        provider = new NumberFormatInfo
                        {
                            NumberDecimalSeparator = ".",
                        };
                    }
                    double averageScore = Convert.ToDouble(model.AverageScore, provider);
                    if (averageScore < 1.0 || averageScore > 5.0)
                    {
                        ModelState.AddModelError("", "Średnia jest niepoprawna. ");
                        return View(model);
                    }
                    DtoUser user = new DtoUser
                    {
                        Id = model.Id ?? -1,
                        Login = model.Login,
                        Password = model.Password,
                        CardId = model.StudentCardId,
                        AverageScore = averageScore,
                        IsAdmin = false
                    };
                    if (await new UserLogic().SaveUser(user))
                    {
                        FormsAuthentication.RedirectFromLoginPage(model.Login, true);
                    }
                }
                catch (Exception)
                {
                    // ignored
                    ModelState.AddModelError("", "Rejestracja nie powiodła się. Numer legitymacji oraz login muszą być unikalne. Średnia musi być poprawna. ");
                    return View(model);
                }
            }
            //If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Rejestracja nie powiodła się. Numer legitymacji oraz login muszą być unikalne. ");
            return View(model);
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // GET: /Account/LogOff
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "")
            {
                Expires = DateTime.Now.AddYears(-1)
            };
            Response.Cookies.Add(cookie);
            cookie = new HttpCookie("ASP.NET_SessionId", "") { Expires = DateTime.Now.AddYears(-1) };
            Response.Cookies.Add(cookie);
            FormsAuthentication.RedirectToLoginPage();
            return RedirectToAction("Index", "Home");
        }
    }
}