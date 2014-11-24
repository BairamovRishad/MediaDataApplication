using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaDataApplication.WcfService.DAL.Entities {

    public class MediaMetadata {
        [Key]
        [ForeignKey("Media")]
        public int MediaId { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(100)]
        public string FileName { get; set; }

        [Required]
        public long FileLength { get; set; }

        public string Description { get; set; }

        public virtual Media Media { get; set; }
    }

}