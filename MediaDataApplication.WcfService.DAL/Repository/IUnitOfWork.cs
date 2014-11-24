using System;
using MediaDataApplication.WcfService.DAL.Entities;

namespace MediaDataApplication.WcfService.DAL.Repository {

    public interface IUnitOfWork : IDisposable {
        // Repositories
        IRepository<User> UserRepository { get; }
        IRepository<Media> MediaRepository { get; }
        IRepository<MediaMetadata> MediaMetadataRepository { get; }
        IRepository<MediaThumbnail> MediaThumbnailsRepository { get; }

        // Save pending changes to the data store.
        void Commit();
    }

}