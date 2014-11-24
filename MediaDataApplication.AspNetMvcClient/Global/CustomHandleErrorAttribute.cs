using System.Web;
using System.Web.Mvc;

namespace MediaDataApplication.AspNetMvcClient.Global {

    public class CustomHandleErrorAttribute : HandleErrorAttribute {
        public override void OnException(ExceptionContext filterContext) {
            var exception = filterContext.Exception;
            var statusCode = new HttpException(null, exception).GetHttpCode();

            filterContext.Result = new JsonResult {
                                                      JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                                                      Data = new {
                                                                     errorMsg = filterContext.Exception.Message
                                                                 }
                                                  };

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = statusCode;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }

}