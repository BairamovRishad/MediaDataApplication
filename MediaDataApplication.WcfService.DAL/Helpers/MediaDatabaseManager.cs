using System;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using MediaDataApplication.WcfService.DAL.Entities;
using MediaDataApplication.WcfService.DAL.Repository;

namespace MediaDataApplication.WcfService.DAL.Helpers {

    public class MediaDatabaseManager {
        private readonly IUnitOfWork unitOfWork;

        public MediaDatabaseManager(IUnitOfWork unitOfWork) {
            if (unitOfWork == null) {
                throw new ArgumentNullException("unitOfWork");
            }
            this.unitOfWork = unitOfWork;
        }

        public void AddMedia(string userName, string mediaFileName, Int64 length) {
            var user = this.GetUser(userName);

            var m = user.Media.SingleOrDefault(x => x.MediaMetadata.FileName.Equals(mediaFileName));

            if (m == null) {
                user.Media.Add(new Media {
                                             MediaMetadata =
                                                 new MediaMetadata { FileName = mediaFileName, FileLength = length },
                                         });
            }
            else {
                m.MediaMetadata.FileName = mediaFileName;
                m.MediaMetadata.FileLength = length;
                this.unitOfWork.MediaRepository.Update(m);
            }

            this.unitOfWork.Commit();
        }

        public void AddMediaThumbnail(string userName, string mediaFileName, string thumbFileName) {
            var user = this.GetUser(userName);
            var media = user.Media.SingleOrDefault(x => x.MediaMetadata.FileName.Equals(mediaFileName));
            if (media == null) {
                throw new FileNotFoundException("Media file not found.", mediaFileName);
            }

            var d = media.MediaThumbnail;
            if (d == null) {
                media.MediaThumbnail = new MediaThumbnail { FileName = thumbFileName };
            }
            else {
                d.FileName = thumbFileName;
                this.unitOfWork.MediaThumbnailsRepository.Update(d);
            }

            this.unitOfWork.Commit();
        }

        public void DeleteMediaAndThumbnail(string userName, string mediaFileName, out string thumbFileName) {
            var user = this.GetUser(userName);
            var media = user.Media.SingleOrDefault(x => x.MediaMetadata.FileName.Equals(mediaFileName));
            if (media == null) {
                throw new ObjectNotFoundException("Media '" + mediaFileName + "' not found in database.");
            }

            var thumb = media.MediaThumbnail;
            if (thumb == null) {
                throw new ObjectNotFoundException("Media '" + mediaFileName + "' thumnail not found in database.");
            }
            thumbFileName = thumb.FileName;

            this.unitOfWork.MediaMetadataRepository.Delete(media.MediaMetadata);
            this.unitOfWork.MediaThumbnailsRepository.Delete(media.MediaThumbnail);
            this.unitOfWork.MediaRepository.Delete(media);
            this.unitOfWork.Commit();
        }

        #region Private Helpers

        private User GetUser(string userName) {
            var user = this.unitOfWork.UserRepository.SingleOrDefault(x => x.UserName.Equals(userName));
            if (user == null) {
                throw new Exception("There are no user by name: \'" + userName + "\'");
            }
            return user;
        }

        #endregion
    }

}