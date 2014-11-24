using System;
using System.Data.Entity;
using MediaDataApplication.WcfService.DAL.Entities;

namespace MediaDataApplication.WcfService.DAL.Repository {

    public class EfUnitOfWork : IUnitOfWork {
        private IRepository<MediaMetadata> mediaMetadataRepository;
        private IRepository<Media> mediaRepository;
        private IRepository<MediaThumbnail> mediaThumbnailsRepository;
        private IRepository<User> userRepository;

        public EfUnitOfWork(DbContext dbContext) {
            this.DbContext = dbContext;
        }

        private DbContext DbContext { get; set; }

        #region IUnitOfWork Implementation

        public IRepository<User> UserRepository {
            get {
                return this.userRepository ?? (this.userRepository = new EfRepository<User>(this.DbContext));
            }
        }

        public IRepository<Media> MediaRepository {
            get {
                return this.mediaRepository ?? (this.mediaRepository = new EfRepository<Media>(this.DbContext));
            }
        }

        public IRepository<MediaMetadata> MediaMetadataRepository {
            get {
                return this.mediaMetadataRepository ?? (this.mediaMetadataRepository = new EfRepository<MediaMetadata>(this.DbContext));
            }
        }

        public IRepository<MediaThumbnail> MediaThumbnailsRepository {
            get {
                return this.mediaThumbnailsRepository ?? (this.mediaThumbnailsRepository = new EfRepository<MediaThumbnail>(this.DbContext));
            }
        }

        /// <summary>
        ///     Save pending changes to the database
        /// </summary>
        public void Commit() {
            this.DbContext.SaveChanges();
        }

        #endregion

        #region IUnitOfWork Members

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing) {
            if (!disposing) {
                return;
            }

            if (this.DbContext != null) {
                this.DbContext.Dispose();
            }
        }
    }

}