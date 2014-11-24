using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using MediaDataApplication.AspNetMvcClient.Helpers;
using MediaDataApplication.AspNetMvcClient.MediaDataServiceReference;
using NLog;

namespace MediaDataApplication.AspNetMvcClient.Models.MediaManager {

    public class MediaTransfer : MediaDataProcessor {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public MediaTransfer(string userName) : base(userName) { }

        public void DownloadMedia(string fileName, string filePath) {
            Contract.Requires(!string.IsNullOrWhiteSpace(fileName));
            Contract.Requires(!string.IsNullOrWhiteSpace(filePath));

            try {
                var mediaMetadata = MediaDataService.GetMediaMetadata(UserName, fileName);

                // Do not download if the media file already exists in the cache
                if (MediaDataCacheManager.IsMediaCached(filePath, mediaMetadata.FileLength)) {
                    return;
                }

                this.DownloadMedia(fileName, filePath, mediaMetadata.FileLength);
            }
            catch (Exception e) {
                new ServiceExceptionHandler(_logger).Handle(e);
                throw new ServiceActivationException();
            }
        }

        public async Task DownloadMediaThumbnailAsync(string mediaFilesDirPath) {
            Contract.Requires(!string.IsNullOrWhiteSpace(mediaFilesDirPath));

            var thumbsDirPath = MediaThumbnailManager.GetThumbnailsDirPath(mediaFilesDirPath);

            try {
                var mediaFilesName = await MediaDataService.GetAllUserMediaFilesNameAsync(UserName);

                foreach (var mediaFileName in mediaFilesName) {
                    var thumbFileName = mediaFileName + MediaThumbnailManager.ThumbsExtenstion;
                    var thumbFilePath = Path.Combine(thumbsDirPath, thumbFileName);

                    // Do not download if the media thumbnail already exists in the cache
                    if (MediaDataCacheManager.IsMediaThumbnailCached(thumbFilePath)) {
                        continue;
                    }

                    await this.DownloadMediaThumbnailAsync(thumbFileName, thumbFilePath);

                    // We'll download the actual file only when it is required. But now we just create a stub file.
                    MediaDataCacheManager.CreateStubMediaFile(mediaFileName, mediaFilesDirPath);
                }
            }
            catch (Exception e) {
                new ServiceExceptionHandler(_logger).Handle(e);
                throw new ServiceActivationException();
            }
        }

        public void UploadMedia(string mediaFileName, byte[] mediaFileData) {
            // Large files will be written in chunks, because it is allowed to transfer data up to 2 GB 
            // at the same connection.
            // Let 'large file' will be considered as a file size of more than 250 MB.
            const int chunk_size = 250 * 1024 * 1024;
            var length = mediaFileData.Length;
            var count = length < chunk_size ? length : chunk_size;

            var uploadFileInfo = new UploadMediaFileInfo {
                                                             FileName = mediaFileName,
                                                             UserName = UserName
                                                         };

            for (int index = 0; index < length; index += count) {
                // Calculation of the the residue on last iteration.
                if (index + count > length) {
                    count = length - index;
                }

                using (Stream stream = new MemoryStream(mediaFileData, index, count)) {
                    try {
                        uploadFileInfo.FileByteStream = stream;
                        MediaDataService.UploadMedia(uploadFileInfo);
                    }
                    catch (Exception e) {
                        new ServiceExceptionHandler(_logger).Handle(e);
                        throw new ServiceActivationException();
                    }
                }
            }
            // Actually, this algorithm will not be used, because the FileUploader was configured to 
            // upload files in chunks of 100 MB.
        }

        public async Task UploadMediaThumbnailAsync(string mediaFileName, string thumbnailName, string thumbnailPath) {
            if (!File.Exists(thumbnailPath)) {
                throw new FileNotFoundException("File not found: ", thumbnailName);
            }

            using (var stream = new FileStream(thumbnailPath, FileMode.Open, FileAccess.Read)) {
                var uploadFileInfo = new UploadThumbFileInfo {
                                                                 UserName = UserName,
                                                                 MediaFileName = mediaFileName,
                                                                 FileName = thumbnailName,
                                                                 FileByteStream = stream
                                                             };

                try {
                    await MediaDataService.UploadMediaThumbnailAsync(uploadFileInfo);
                }
                catch (Exception e) {
                    new ServiceExceptionHandler(_logger).Handle(e);
                    throw new ServiceActivationException();
                }
            }
        }

        #region Private Helpers

        private void DownloadMedia(string fileName, string filePath, Int64 length) {
            // Large files will be download in chunks, because it is allowed to transfer data up to 2 GB 
            // at the same connection.
            // Let 'large file' will be considered as a file size of more than 100 MB.
            const Int64 chunk_size = 100 * 1024 * 1024;
            var count = length < chunk_size ? length : chunk_size;

            var requestData = new DownloadChunkRequest { UserName = UserName, FileName = fileName };

            for (Int64 index = 0; index < length; index += count) {
                // Calculation of the the residue on last iteration.
                if (index + count > length) {
                    count = length - index;
                }

                requestData.Offset = index;
                requestData.Length = (int)count;

                var file = MediaDataService.DownloadMedia(requestData);
                if (file == null) {
                    break;
                }
                MediaDataCacheManager.CacheFile(filePath, file.FileByteStream);
                file.FileByteStream.Dispose();
            }
        }

        private async Task DownloadMediaThumbnailAsync(string thumbFileName, string thumbFilePath) {
            try {
                var requestData = new DownloadRequest { UserName = UserName, FileName = thumbFileName };
                var thumb = await MediaDataService.DownloadMediaThumbnailAsync(requestData);

                MediaDataCacheManager.CacheFile(thumbFilePath, thumb.FileByteStream);
            }
            catch (Exception e) {
                new ServiceExceptionHandler(_logger).Handle(e);
                throw new ServiceActivationException();
            }
        }

        #endregion
    }

}