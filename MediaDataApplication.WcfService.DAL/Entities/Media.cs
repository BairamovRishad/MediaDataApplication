namespace MediaDataApplication.WcfService.DAL.Entities {

    public class Media {
        public int MediaId { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual MediaThumbnail MediaThumbnail { get; set; }

        public virtual MediaMetadata MediaMetadata { get; set; }
    }

}