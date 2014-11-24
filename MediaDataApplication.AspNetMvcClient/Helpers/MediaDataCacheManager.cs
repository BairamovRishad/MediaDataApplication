using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace MediaDataApplication.AspNetMvcClient.Helpers {

    public class MediaDataCacheManager {
        public static void CacheFile(string filePath, Stream fileStream) {
            Contract.Requires(!string.IsNullOrWhiteSpace(filePath));
            Contract.Requires(fileStream != null);

            using (var newFile = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None)) {
                fileStream.CopyTo(newFile);
            }
        }

        public static void ClearUserData(string userMediaDir) {
            if (Directory.Exists(userMediaDir)) {
                Directory.Delete(userMediaDir, true);
            }
        }

        public static void CreateStubMediaFile(string mediaFileName, string mediaFilesDirPath) {
            Contract.Requires(!string.IsNullOrWhiteSpace(mediaFileName));
            Contract.Requires(!string.IsNullOrWhiteSpace(mediaFilesDirPath));

            var mediaFilePath = Path.Combine(mediaFilesDirPath, mediaFileName);
            File.Create(mediaFilePath).Dispose();
        }

        public static string GetUserMediaDir(string serverPath, string userName) {
            return Path.Combine(serverPath, "CachedMediaFiles", userName);
        }

        public static bool IsMediaCached(string filePath, Int64 fileLength) {
            Contract.Requires(!string.IsNullOrWhiteSpace(filePath));
            Contract.Requires(fileLength >= 0);

            var requiredFile = new FileInfo(filePath);

            if (!requiredFile.Exists) {
                return false;
            }

            if (requiredFile.Length.Equals(fileLength)) {
                return true;
            }

            // If file in cache has a wrong size, will create an empty file for further appending
            File.Delete(filePath);
            File.Create(filePath).Dispose();
            return false;
        }

        public static bool IsMediaThumbnailCached(string thumbFilePath) {
            Contract.Requires(!string.IsNullOrWhiteSpace(thumbFilePath));

            return File.Exists(thumbFilePath);
        }

        public static void RenameCachedMediaAndThumbs(string userMediaDir, string previousFileName, string newFileName) {
            Contract.Requires(!string.IsNullOrWhiteSpace(userMediaDir));
            Contract.Requires(!string.IsNullOrWhiteSpace(previousFileName));
            Contract.Requires(!string.IsNullOrWhiteSpace(newFileName));

            var previousMediaFilePath = Path.Combine(userMediaDir, previousFileName);
            var newMediaFilePath = Path.Combine(userMediaDir, newFileName);
            File.Move(previousMediaFilePath, newMediaFilePath);

            var thumbsDir = MediaThumbnailManager.ThumbsDirName;
            var thumbsExt = MediaThumbnailManager.ThumbsExtenstion;
            var previousThumbPath = Path.Combine(userMediaDir, thumbsDir, previousFileName + thumbsExt);
            var newThumbPath = Path.Combine(userMediaDir, thumbsDir, newFileName + thumbsExt);
            File.Move(previousThumbPath, newThumbPath);
        }
    }

}