using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using MediaDataApplication.AspNetMvcClient.Global;
using MediaDataApplication.AspNetMvcClient.Global.Authentication;
using MediaDataApplication.AspNetMvcClient.Helpers;
using MediaDataApplication.AspNetMvcClient.MediaDataServiceReference;
using MediaDataApplication.AspNetMvcClient.Models;
using Ninject;

namespace MediaDataApplication.AspNetMvcClient.Controllers {

    [Authorize]
    public class AccountController : BaseController {
        [Inject]
        public IAuthenticationManager AuthenticationManager { get; set; }

        [Inject]
        public IApplicationUserManager UserManager { get; set; }

        [AllowAnonymous]
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl) {
            if (!ModelState.IsValid) {
                return RedirectToAction("Index", "Home");
            }

            try {
                var loginSucceeded = this.AuthenticationManager.SignIn(model.UserName, model.Password, model.RememberMe);
                if (loginSucceeded) {
                    var userMediaDir = MediaDataCacheManager.GetUserMediaDir(Server.MapPath("~"), model.UserName);
                    MediaDataCacheManager.ClearUserData(userMediaDir);
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            catch (ServiceActivationException) {
                ModelState.AddModelError("", "Sorry, login failed due to server problems. Try again later.");
            }

            // If we got this far, something failed, redisplay form   
            Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
            return PartialView("_Login", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout() {
            this.AuthenticationManager.SignOut();
            var userMediaDir = MediaDataCacheManager.GetUserMediaDir(Server.MapPath("~"), User.Identity.Name);
            Task.Run(() => MediaDataCacheManager.ClearUserData(userMediaDir));
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model) {
            if (!ModelState.IsValid) {
                return RedirectToAction("Index", "Home");
            }

            try {
                var user = ModelMapper.Map<RegisterViewModel, User>(model);
                var registerSucceeded = this.UserManager.Register(user);
                if (registerSucceeded) {
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                ModelState.AddModelError("", "Username \'" + model.UserName + "\' already registered");
            }
            catch (ServiceActivationException) {
                ModelState.AddModelError("", "Sorry, the registration failed due to server problems. Try again later.");
            }

            // If we got this far, something failed, redisplay form
            Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
            return PartialView("_Register", model);
        }
    }

}