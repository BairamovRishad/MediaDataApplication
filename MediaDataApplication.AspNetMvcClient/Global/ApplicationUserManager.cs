using System;
using System.ServiceModel;
using MediaDataApplication.AspNetMvcClient.Helpers;
using MediaDataApplication.AspNetMvcClient.MediaDataServiceReference;
using Ninject;
using NLog;

namespace MediaDataApplication.AspNetMvcClient.Global {

    public interface IApplicationUserManager {
        bool Register(User user);
    }

    public class ApplicationUserManager : IApplicationUserManager {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        [Inject]
        public IMediaDataService MediaDataService { get; set; }

        #region IApplicationUserManager Members

        public bool Register(User user) {
            try {
                this.MediaDataService.RegisterUser(user);
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

        #endregion
    }

}