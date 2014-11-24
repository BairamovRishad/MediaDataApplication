using System.Web.Optimization;
using Backload.Configuration;
using MediaDataApplication.AspNetMvcClient;
using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(BackloadConfig), "Initialize")]

namespace MediaDataApplication.AspNetMvcClient {

    public static class BackloadConfig {
        public static void Initialize() {
            // Use bundeling for client files (scripts (js) and styles (css)). 
            // Comment this out, if you manually include the files in your page
            RegisterBundles(BundleTable.Bundles);
        }

        #region Private Helpers

        private static void RegisterBundles(BundleCollection bundles) {
            var clientFiles = Bundles.GetClientFiles();
            if (clientFiles == null) {
                clientFiles = new ClientFilesElement(); // Default values
            }

            // Bootstrap     
            bundles.Add(new ScriptBundle("~/bundles/fileupload/bootstrap/BasicPlusUI/js").Include(
                                                                                                  clientFiles.Scripts + "jqueryui/jquery.ui.widget.js",
                                                                                                  clientFiles.Scripts + "tmpl.debug.js",
                                                                                                  clientFiles.Scripts + "load-image.debug.js",
                                                                                                  clientFiles.Scripts + "canvas-to-blob.debug.js",
                                                                                                  clientFiles.Scripts + "bootstrap/bootstrap.debug.js",
                                                                                                  clientFiles.Scripts + "bootstrap/bootstrap-image-gallery.debug.js",
                                                                                                  clientFiles.Scripts + "jquery.iframe-transport.js",
                                                                                                  clientFiles.Scripts + "jquery.fileupload.js",
                                                                                                  clientFiles.Scripts + "jquery.fileupload-process.js",
                                                                                                  clientFiles.Scripts + "jquery.fileupload-image.js",
                                                                                                  clientFiles.Scripts + "jquery.fileupload-audio.js",
                                                                                                  clientFiles.Scripts + "jquery.fileupload-video.js",
                                                                                                  clientFiles.Scripts + "jquery.fileupload-validate.js",
                                                                                                  clientFiles.Scripts + "jquery.fileupload-ui.js"));
            bundles.Add(new StyleBundle("~/bundles/fileupload/bootstrap/BasicPlusUI/css").Include(
                                                                                                  clientFiles.Styles + "bootstrap/bootstrap-responsive.debug.css",
                                                                                                  clientFiles.Styles + "bootstrap/bootstrap-image-gallery.debug.css",
                                                                                                  clientFiles.Styles + "jquery.fileupload-ui.css"));
        }

        #endregion
    }

}