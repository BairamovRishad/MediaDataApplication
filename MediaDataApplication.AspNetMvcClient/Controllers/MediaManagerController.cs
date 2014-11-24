using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MediaDataApplication.AspNetMvcClient.Global;
using MediaDataApplication.AspNetMvcClient.Helpers;
using MediaDataApplication.AspNetMvcClient.MediaDataServiceReference;
using MediaDataApplication.AspNetMvcClient.Models;
using MediaDataApplication.AspNetMvcClient.Models.MediaManager;
using NLog;

namespace MediaDataApplication.AspNetMvcClient.Controllers {

    [Authorize]
    public class MediaManagerController : BaseController {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ActionResult DownloadMedia(string fileName, string fileUrl) {
            try {
                var urlAbsolutePath = new Uri(fileUrl).AbsolutePath;
                var decodedUrlPath = HttpUtility.UrlDecode(urlAbsolutePath);
                var filePath = Server.MapPath(decodedUrlPath);

                new MediaTransfer(User.Identity.Name).DownloadMedia(fileName, filePath);
            }
            catch (ServiceActivationException) {
                throw new HttpException((int)HttpStatusCode.ServiceUnavailable, "Sorry, the server temporalily is unavailable. Try again later.");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Main() {
            return View();
        }

        [HttpGet]
        public ActionResult MediaMetadataEdit(string fileName) {
            try {
                var mediaMetadata = new MediaManipulator(User.Identity.Name).GetMediaMetadata(fileName);
                var model = ModelMapper.Map<MediaMetadata, MediaMetadataViewModel>(mediaMetadata);
                return PartialView("_Metadata", model);
            }
            catch (ServiceActivationException) {
                throw new HttpException((int)HttpStatusCode.ServiceUnavailable, "Sorry, the server temporalily is unavailable. Try again later.");
            }
        }

        [HttpPost]
        [CustomHandleError]
        public async Task<ActionResult> MediaMetadataEdit(MediaMetadataViewModel model) {
            try {
                if (!model.MediaFileName.Equals(model.FileName)) {
                    var userMediaDir = MediaDataCacheManager.GetUserMediaDir(Server.MapPath("~"), User.Identity.Name);
                    MediaDataCacheManager.RenameCachedMediaAndThumbs(userMediaDir, model.MediaFileName, model.FileName);
                }

                var mediaMetadata = ModelMapper.Map<MediaMetadataViewModel, MediaMetadata>(model);
                await new MediaManipulator(User.Identity.Name).UpdateMediaMetadata(mediaMetadata);
                return PartialView("_Metadata", model);
            }
            catch (IOException e) {
                _logger.Trace("IOException: " + e.Message);
                throw new HttpException((int)HttpStatusCode.ExpectationFailed, "There is already a file with this name. Please try another one.");
            }
            catch (ServiceActivationException) {
                throw new HttpException((int)HttpStatusCode.ServiceUnavailable, "Sorry, the server temporalily is unavailable. Try again later.");
            }
        }

        public ActionResult Search(string term) {
            if (string.IsNullOrWhiteSpace(term)) {
                return new EmptyResult();
            }

            try {
                var mediaMetadata = new MediaManipulator(User.Identity.Name).GetAllUserMediaMetadata();

                var results = SimpleMediaSearchEngine.Search(term, mediaMetadata);

                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (ServiceActivationException e) {
                throw new HttpException((int)HttpStatusCode.ServiceUnavailable, "Sorry, the server temporalily is unavailable. Try again later.");
            }
        }
    }

}