using System;
using System.Collections.Generic;
using MediaDataApplication.WcfService.DAL.Entities;
using MediaDataApplication.WcfService.DAL.Repository;

namespace MediaDataApplication.WcfService.Tests.Fake.Repository {

    internal class CustomDataGenerator {
        private readonly IUnitOfWork unitOfWork;

        public CustomDataGenerator(IUnitOfWork unitOfWork) {
            this.unitOfWork = unitOfWork;
        }

        public void GenerateData(int i) {
            for (int j = 1; j <= i; j++) {
                this.GenerateUser(j);
                this.GenerateMedia(j);
                this.GenerateMediaMetadata(j);
                this.GenerateMediaThumbnail(j);
            }
        }

        #region Private Helpers

        private void GenerateMedia(int i) {
            var media = new Media {
                                      MediaId = 1,
                                      UserId = i,
                                  };

            var user = this.unitOfWork.UserRepository.Single(x => x.UserId.Equals(media.UserId));
            media.User = user;

            user.Media.Add(media);
            this.unitOfWork.MediaRepository.Add(media);
        }

        private void GenerateMediaMetadata(int i) {
            var mediaMetadata = new MediaMetadata {
                                                      MediaId = 1,
                                                      FileName = "test1.jpg",
                                                      FileLength = new Random().Next(),
                                                      Description = "It's a beautiful place!",
                                                  };
            var media = this.unitOfWork.MediaRepository.Single(x => x.MediaId.Equals(mediaMetadata.MediaId));
            mediaMetadata.Media = media;

            media.MediaMetadata = mediaMetadata;
            this.unitOfWork.MediaMetadataRepository.Add(mediaMetadata);
        }

        private void GenerateMediaThumbnail(int i) {
            var mediaThumbnail = new MediaThumbnail {
                                                        MediaId = 1,
                                                        FileName = "test1.jpg.png",
                                                    };
            var media = this.unitOfWork.MediaRepository.Single(x => x.MediaId.Equals(mediaThumbnail.MediaId));
            mediaThumbnail.Media = media;

            media.MediaThumbnail = mediaThumbnail;
            this.unitOfWork.MediaThumbnailsRepository.Add(mediaThumbnail);
        }

        private void GenerateUser(int i) {
            var user = new User {
                                    UserId = i,
                                    UserName = "TestBob" + i,
                                    Password = "password",
                                    FirstName = "TestRobert",
                                    LastName = "TestBobert",
                                    CreationDate = DateTime.Now,
                                    Media = new List<Media>()
                                };

            this.unitOfWork.UserRepository.Add(user);
        }

        #endregion
    }

}