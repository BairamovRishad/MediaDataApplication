using System.Collections.Generic;
using System.IO;
using MediaDataApplication.WcfService.BDO;
using MediaDataApplication.WcfService.DAL.DAO;

namespace MediaDataApplication.WcfService.Logic {

    public class MediaLogic {
        private readonly MediaDAO mediaDAO = new MediaDAO();
        private readonly MediaMetadataDAO mediaMetadataDAO = new MediaMetadataDAO();

        public void DeleteMedia(string userName, string[] mediaFilesName) {
            foreach (var fileName in mediaFilesName) {
                this.mediaDAO.DeleteMediaAndThumbnail(userName, fileName);
            }
        }

        public string[] GetAllUserMediaFilesName(string userName) {
            return this.mediaMetadataDAO.GetAllUserMediaFileNames(userName);
        }

        public ICollection<MediaMetadataBDO> GetAllUserMediaMetadata(string userName) {
            return this.mediaMetadataDAO.GetAllUserMediaMetadata(userName);
        }

        public Stream GetMedia(string userName, FileChunkBDO fileChunk) {
            return this.mediaDAO.GetMedia(userName, fileChunk);
        }

        public MediaMetadataBDO GetMediaMetadata(string userName, string mediaFileName) {
            return this.mediaMetadataDAO.GetMediaMetadata(userName, mediaFileName);
        }

        public Stream GetMediaThumbnail(string userName, string fileName) {
            return this.mediaDAO.GetMediaThumbnail(userName, fileName);
        }

        public void UpdateMediaMetadata(MediaMetadataBDO mediaMetadata) {
            this.mediaMetadataDAO.UpdateMediaMetadata(mediaMetadata);
        }

        public void UploadMedia(string userName, string mediaFileName, Stream file) {
            this.mediaDAO.AddMedia(userName, mediaFileName, file);
        }

        public void UploadMediaThumbnail(string userName, string mediaFileName, string thumbFileName, Stream file) {
            this.mediaDAO.AddMediaThumbnail(userName, mediaFileName, thumbFileName, file);
        }
    }

}