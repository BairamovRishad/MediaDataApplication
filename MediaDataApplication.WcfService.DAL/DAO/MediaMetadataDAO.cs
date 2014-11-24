using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using MediaDataApplication.WcfService.BDO;
using MediaDataApplication.WcfService.DAL.Entities;
using MediaDataApplication.WcfService.DAL.Repository;

namespace MediaDataApplication.WcfService.DAL.DAO {

    public class MediaMetadataDAO : BaseDAO {
        public MediaMetadataDAO() { }

        public MediaMetadataDAO(IUnitOfWork unitOfWork)
            : base(unitOfWork) { }

        public string[] GetAllUserMediaFileNames(string userName) {
            var user = this.GetUser(userName);
            return user.Media.Select(media => media.MediaMetadata.FileName).ToArray();
        }

        public ICollection<MediaMetadataBDO> GetAllUserMediaMetadata(string userName) {
            var user = this.GetUser(userName);

            var userAllMediaMetadata = from m in user.Media
                                       select m.MediaMetadata;

            return Mapper.Map<ICollection<MediaMetadata>, ICollection<MediaMetadataBDO>>(userAllMediaMetadata.ToList());
        }

        public MediaMetadataBDO GetMediaMetadata(string userName, string mediaFileName) {
            var user = this.GetUser(userName);

            var mediaMetadata = user.Media
                                    .Where(media => media.MediaMetadata.FileName.Equals(mediaFileName))
                                    .Select(media => media.MediaMetadata).SingleOrDefault();

            if (mediaMetadata == null) {
                throw new ObjectNotFoundException("Metadata for '" + mediaFileName + "' not found.");
            }

            return Mapper.Map<MediaMetadata, MediaMetadataBDO>(mediaMetadata);
        }

        public void UpdateMediaMetadata(MediaMetadataBDO mediaMetadataBDO) {
            var mediaMetadata = UnitOfWork.MediaMetadataRepository.GetById(mediaMetadataBDO.MediaId);

            if (mediaMetadata == null) {
                throw new ObjectNotFoundException("Metadata for media with id=" + mediaMetadataBDO.MediaId + " not found.");
            }

            var previousMediaFileName = mediaMetadata.FileName;
            var newMediaFileName = mediaMetadataBDO.FileName;

            // Update media metadata in DB
            mediaMetadata.FileName = newMediaFileName;
            mediaMetadata.Description = mediaMetadataBDO.Description;
            UnitOfWork.MediaMetadataRepository.Update(mediaMetadata);

            if (!newMediaFileName.Equals(previousMediaFileName)) {
                // Rename thumbnail in DB
                var mediaThumbnail = mediaMetadata.Media.MediaThumbnail;
                var previousMediaThumbFileName = mediaThumbnail.FileName;
                var newThumbFileName = previousMediaThumbFileName.Replace(previousMediaFileName, newMediaFileName);

                mediaThumbnail.FileName = newThumbFileName;
                UnitOfWork.MediaThumbnailsRepository.Update(mediaThumbnail);

                // Rename media file and its thumb
                var userName = mediaMetadata.Media.User.UserName;
                new UserMediaFilesManager(userName).Rename(previousMediaFileName, newMediaFileName);
                new UserMediaThumbFilesManager(userName).Rename(previousMediaThumbFileName, newThumbFileName);
            }

            UnitOfWork.Commit();
        }

        #region Private Helpers

        private User GetUser(string userName) {
            var user = UnitOfWork.UserRepository.SingleOrDefault(x => x.UserName.Equals(userName));
            if (user == null) {
                throw new Exception("There are no user by name: \'" + userName + "\'");
            }
            return user;
        }

        #endregion
    }

}