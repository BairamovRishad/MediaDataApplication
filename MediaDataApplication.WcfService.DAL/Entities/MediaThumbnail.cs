using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaDataApplication.WcfService.DAL.Entities {

    public class MediaThumbnail {
        [Key]
        [ForeignKey("Media")]
        public int MediaId { get; set; }

        [Required]
        [StringLength(100)]
        public string FileName { get; set; }

        public virtual Media Media { get; set; }
    }

}