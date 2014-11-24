using System.Data.Entity;
using MediaDataApplication.WcfService.DAL.Entities;

namespace MediaDataApplication.WcfService.DAL {

    public class MediaDataDbContext : DbContext {
        public MediaDataDbContext() : base("name=DefaultConnection") { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Media> Media { get; set; }
        public virtual DbSet<MediaMetadata> MediaMetadata { get; set; }
        public virtual DbSet<MediaThumbnail> MediaThumbnails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Media>().HasRequired(x => x.MediaMetadata).WithRequiredPrincipal(x => x.Media).WillCascadeOnDelete(true);
            modelBuilder.Entity<Media>().HasRequired(x => x.MediaThumbnail).WithRequiredPrincipal(x => x.Media).WillCascadeOnDelete(true);
        }
    }

}