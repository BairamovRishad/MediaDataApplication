using System;
using System.ServiceModel;
using System.Web.Security;
using MediaDataApplication.AspNetMvcClient.Helpers;
using MediaDataApplication.AspNetMvcClient.MediaDataServiceReference;
using Ninject;
using NLog;

namespace MediaDataApplication.AspNetMvcClient.Global.Authentication {

    public class CustomAuthenticationManager : IAuthenticationManager {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        [Inject]
        public IMediaDataService MediaDataService { get; set; }

        #region IAuthenticationManager Members

        public bool SignIn(string userName, string password, bool isPersistent) {
            try {
                this.MediaDataService.LoginUser(userName, password);
                FormsAuthentication.SetAuthCookie(userName, isPersistent);
                return true;
            }
            catch (FaultException<UserFault> e) {
                _logger.Error("UserFault returned: " + e.Detail.FaultMessage);
            }
            catch (Exception e) {
                var exHandler = new ServiceExceptionHandler(_logger);
                exHandler.Handle(e);
                throw new Exception(exHandler.Message);
            }

            return false;
        }

        public void SignOut() {
            FormsAuthentication.SignOut();
        }

        #endregion
    }

}