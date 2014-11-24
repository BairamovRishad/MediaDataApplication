using System.Web.Mvc;
using MediaDataApplication.AspNetMvcClient.MediaDataServiceReference;
using NLog;

namespace MediaDataApplication.AspNetMvcClient.Models.MediaManager {

    public abstract class MediaDataProcessor {
        protected readonly string UserName;
        protected IMediaDataService mediaDataService;
        

        protected MediaDataProcessor(string userName) {
            this.UserName = userName;
        }

        public IMediaDataService MediaDataService {
            get {
                return this.mediaDataService
                       ?? (this.mediaDataService = DependencyResolver.Current.GetService<IMediaDataService>());
            }
        }
    }

}