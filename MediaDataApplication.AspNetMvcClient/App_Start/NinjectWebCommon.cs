using System;
using System.Web;
using System.Windows.Forms;
using MediaDataApplication.AspNetMvcClient;
using MediaDataApplication.AspNetMvcClient.Global;
using MediaDataApplication.AspNetMvcClient.Global.Authentication;
using MediaDataApplication.AspNetMvcClient.Mappers;
using MediaDataApplication.AspNetMvcClient.MediaDataServiceReference;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace MediaDataApplication.AspNetMvcClient {

    public static class NinjectWebCommon {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start() {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop() {
            bootstrapper.ShutDown();
        }

        #region Private Helpers

        /// <summary>
        ///     Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel() {
            var kernel = new StandardKernel();
            try {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        ///     Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel) {
            kernel.Bind<IMediaDataService>().ToMethod(x => {
                                                          var proxy = new MediaDataServiceClientWraper();
                                                          return proxy.MediaDataServiceClient;
                                                      }).InTransientScope();
            kernel.Bind<IMapper>().To<CommonMapper>().InSingletonScope();
            kernel.Bind<IAuthenticationManager>().To<CustomAuthenticationManager>().InRequestScope();
            kernel.Bind<IApplicationUserManager>().To<ApplicationUserManager>().InRequestScope();
        }

        #endregion
    }

    internal class MediaDataServiceClientWraper : IDisposable {

        private MediaDataServiceClient mediaDataServiceClient;

        public MediaDataServiceClientWraper() {
            // MessageBox.Show(instances.ToString());
        }

        public IMediaDataService MediaDataServiceClient {
            get {
                return this.mediaDataServiceClient
                       ?? (this.mediaDataServiceClient = new MediaDataServiceClient());
            }
        }

        #region IDisposable Members

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing) {
            MessageBox.Show("Dispose");
            if (!disposing) {
                return;
            }

            if (this.mediaDataServiceClient != null) {
                this.mediaDataServiceClient.Close();
            }
        }
    }

}