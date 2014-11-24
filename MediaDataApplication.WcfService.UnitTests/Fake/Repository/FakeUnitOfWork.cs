using System.Collections.Generic;
using MediaDataApplication.WcfService.DAL.Entities;
using MediaDataApplication.WcfService.DAL.Repository;

namespace MediaDataApplication.WcfService.Tests.Fake.Repository {

    public class FakeUnitOfWork : IUnitOfWork {
        private readonly IRepository<MediaMetadata> mediaMetadataRepository;
        private readonly IRepository<Media> mediaRepository;
        private readonly IRepository<MediaThumbnail> mediaThumbnailRepository;
        private readonly IRepository<User> userRepository;

        public FakeUnitOfWork() {
            this.userRepository = new FakeRepository<User>(new List<User>());
            this.mediaRepository = new FakeRepository<Media>(new List<Media>());
            this.mediaMetadataRepository = new FakeRepository<MediaMetadata>(new List<MediaMetadata>());
            this.mediaThumbnailRepository = new FakeRepository<MediaThumbnail>(new List<MediaThumbnail>());
            new CustomDataGenerator(this).GenerateData(1);
        }

        #region IUnitOfWork Members

        public void Dispose() { }

        #endregion

        #region IUnitOfWork Implementation

        public IRepository<User> UserRepository {
            get {
                return this.userRepository;
            }
        }

        public IRepository<Media> MediaRepository {
            get {
                return this.mediaRepository;
            }
        }

        public IRepository<MediaMetadata> MediaMetadataRepository {
            get {
                return this.mediaMetadataRepository;
            }
        }

        public IRepository<MediaThumbnail> MediaThumbnailsRepository {
            get {
                return this.mediaThumbnailRepository;
            }
        }

        public void Commit() { }

        #endregion
    }

}