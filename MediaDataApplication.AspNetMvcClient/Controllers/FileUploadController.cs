using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Backload;
using Backload.Eventing.Args;
using MediaDataApplication.AspNetMvcClient.Models.MediaManager;
using NLog;

namespace MediaDataApplication.AspNetMvcClient.Controllers {

    public class FileUploadController : BaseController {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private MediaTransfer mediaTransfer;

        public async Task<ActionResult> FileHandler() {
            var handler = new FileUploadHandler(Request, this);

            handler.GetFilesRequestStartedAsync += this.GetFilesRequestStartedAsyncHandler;
            handler.GetFilesRequestException += this.GetFilesRequestExceptionHandler;

            handler.StoreFileRequestStarted += this.StoreFileRequestStartedHandler;
            handler.StoreFileRequestFinishedAsync += this.StoreFileRequestFinishedAsyncHandler;
            handler.StoreFileRequestException += this.StoreFileRequestExceptionHandler;

            handler.DeleteFilesRequestStartedAsync += this.DeleteFilesRequestStartedAsyncHandler;
            handler.DeleteFilesRequestException += this.DeleteFilesRequestExceptionHandler;

            ActionResult result = await handler.HandleRequestAsync();
            return result;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            this.mediaTransfer = new MediaTransfer(User.Identity.Name);
            base.OnActionExecuting(filterContext);
        }

        #region Private Helpers

        private void DeleteFilesRequestExceptionHandler(object sender, DeleteFilesRequestEventArgs e) {
            try {
                var filesName = e.Param.DeleteFiles.Files.Select(file => file.FileName).ToArray();
                var message = String.Format("StoreFileRequestException. User: {0} Files name: {1}",
                                            User.Identity.Name,
                                            string.Join(", ", filesName));
                _logger.Error(message);
            }
            catch (Exception ex) {
                _logger.Error("StoreFileRequestExceptionHandler: " + ex);
                e.Context.Response.StatusCode = 500;
            }
        }

        private async Task DeleteFilesRequestStartedAsyncHandler(object sender, DeleteFilesRequestEventArgs e) {
            try {
                var files = e.Param.DeleteFiles.Files;
                var filesName = files.Select(file => file.FileName).ToArray();

                var mediaManipulator = new MediaManipulator(User.Identity.Name);
                await mediaManipulator.DeleteMediaAsync(filesName);
            }
            catch (Exception ex) {
                _logger.Error("DeleteFilesRequestStartedAsyncHandler: " + ex);

                foreach (var deletefiles in e.Param.DeleteFiles) {
                    deletefiles.Success = false;
                    deletefiles.ErrorMessage = "Failed to delete media. Please try again.";
                }
            }
        }

        private void GetFilesRequestExceptionHandler(object sender, GetFilesRequestEventArgs e) {
            _logger.Error("GetFilesRequestException: User " + User.Identity.Name);
        }

        private async Task GetFilesRequestStartedAsyncHandler(object sender, GetFilesRequestEventArgs e) {
            try {
                var userMediaFilesDirPath = Path.Combine(e.Param.SearchPath, User.Identity.Name);
                e.Param.SearchPath = userMediaFilesDirPath;
                await this.mediaTransfer.DownloadMediaThumbnailAsync(userMediaFilesDirPath);
            }
            catch (Exception ex) {
                _logger.Error("GetFilesRequestStartedAsyncHandler: " + ex);
                e.Context.Response.StatusCode = 500;
            }
        }

        private void StoreFileRequestExceptionHandler(object sender, StoreFileRequestEventArgs e) {
            var message = String.Format("StoreFileRequestException. User: {0}",
                                        User.Identity.Name);
            _logger.Error(message);
        }

        private async Task StoreFileRequestFinishedAsyncHandler(object sender, StoreFileRequestEventArgs e) {
            var file = e.Param.FileStatusItem;

            try {
                await this.mediaTransfer.UploadMediaThumbnailAsync(file.FileName,
                                                                   file.ThumbnailName,
                                                                   file.StorageInfo.ThumbnailPath);
            }
            catch (Exception ex) {
                _logger.Error("StoreFileRequestFinishedAsyncHandler: " + ex);
                file.Success = false;
                file.ErrorMessage = "Failed to save media or thumbnail. Please try again.";
            }
        }

        private void StoreFileRequestStartedHandler(object sender, StoreFileRequestEventArgs e) {
            if (e.Param.FileStatusItem.FileSize.Equals(0L)) {
                e.Param.FileStatusItem.Success = false;
                e.Param.FileStatusItem.ErrorMessage = "Media file is empty";
            }

            var file = e.Param.FileStatusItem;

            try {
                this.mediaTransfer.UploadMedia(file.FileName, file.FileData);
            }
            catch (Exception ex) {
                _logger.Error("StoreFileRequestFinishedAsyncHandler: " + ex);
                file.Success = false;
                file.ErrorMessage = "Failed to save media or thumbnail. Please try again.";
            }
        }

        #endregion
    }

}