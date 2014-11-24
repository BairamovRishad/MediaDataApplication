using System;
using System.IO;
using MediaDataApplication.WcfService.BDO;
using MediaDataApplication.WcfService.DAL.Helpers;
using MediaDataApplication.WcfService.DAL.Repository;

namespace MediaDataApplication.WcfService.DAL.DAO {

    public class MediaDAO : BaseDAO {
        private readonly MediaDatabaseManager mediaDatabaseManager;

        public MediaDAO() {
            this.mediaDatabaseManager = new MediaDatabaseManager(UnitOfWork);
        }

        public MediaDAO(IUnitOfWork unitOfWork) : base(unitOfWork) {
            this.mediaDatabaseManager = new MediaDatabaseManager(UnitOfWork);
        }

        public void AddMedia(string userName, string mediaFileName, Stream fileStream) {
            Int64 fileLength;
            new UserMediaFilesManager(userName).CopyTo(mediaFileName, fileStream, out fileLength);
            this.mediaDatabaseManager.AddMedia(userName, mediaFileName, fileLength);
        }

        public void AddMediaThumbnail(string userName, string mediaFileName, string thumbFileName, Stream fileStream) {
            Int64 fileLength;
            new UserMediaThumbFilesManager(userName).CopyTo(thumbFileName, fileStream, out fileLength);
            this.mediaDatabaseManager.AddMediaThumbnail(userName, mediaFileName, thumbFileName);
        }

        public void DeleteMediaAndThumbnail(string userName, string mediaFileName) {
            string thumbFileName;
            this.mediaDatabaseManager.DeleteMediaAndThumbnail(userName, mediaFileName, out thumbFileName);

            new UserMediaFilesManager(userName).Delete(mediaFileName);
            new UserMediaThumbFilesManager(userName).Delete(thumbFileName);
        }

        public Stream GetMedia(string userName, FileChunkBDO fileChunk) {
            return new UserMediaFilesManager(userName).OpenRead(fileChunk.FileName, fileChunk.Offset, fileChunk.Length);
        }

        public Stream GetMediaThumbnail(string userName, string thumbFileName) {
            return new UserMediaThumbFilesManager(userName).OpenRead(thumbFileName);
        }
    }

}