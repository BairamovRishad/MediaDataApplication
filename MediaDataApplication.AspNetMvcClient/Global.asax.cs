using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog;

namespace MediaDataApplication.AspNetMvcClient {

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication {
        protected void Application_Error(object sender, EventArgs args) {
            Debugger.Break();
            Exception error = ((HttpApplication)sender).Context.Server.GetLastError();
            LogManager.GetCurrentClassLogger().Error(error);
        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Contract.ContractFailed += ContractFailed;
        }

        #region Private Helpers

        private static void ContractFailed(object sender, ContractFailedEventArgs e) {
            LogManager.GetCurrentClassLogger().Error(e);
            e.SetUnwind();
            // e.SetHandled();
        }

        #endregion
    }

}