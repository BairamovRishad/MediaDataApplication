using System.Web.Mvc;

namespace MediaDataApplication.AspNetMvcClient.Controllers {

    public class ErrorController : BaseController {
        public ActionResult HttpError() {
            return View("Error");
        }

        public ActionResult Index() {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult NotFound() {
            return View("Error");
        }
    }

}