using System;
using System.IO;
using System.Net.Mime;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using System.Windows.Forms;

namespace MediaDataApplication.AspNetMvcClient.Controllers {

    public class HomeController : BaseController {
        public ActionResult Index() {
            if (User.Identity.IsAuthenticated) {
                return RedirectToAction("Main", "MediaManager");
            }
            return RedirectToAction("Index", "Account");
        }
    }

}