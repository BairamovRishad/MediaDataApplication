using System.IO;

namespace MediaDataApplication.AspNetMvcClient.Helpers {

    internal class MediaThumbnailManager {
        public static string ThumbsExtenstion {
            get {
                return ".png";
            }
        }

        public static string ThumbsDirName {
            get {
                return "_thumbs";
            }
        }

        public static string GetThumbnailsDirPath(string mediaFilesRootDirPath) {
            var thumbnailsDirPath = Path.Combine(mediaFilesRootDirPath, ThumbsDirName + "\\");

            if (!Directory.Exists(thumbnailsDirPath)) {
                Directory.CreateDirectory(thumbnailsDirPath);
            }

            return thumbnailsDirPath;
        }
    }

}