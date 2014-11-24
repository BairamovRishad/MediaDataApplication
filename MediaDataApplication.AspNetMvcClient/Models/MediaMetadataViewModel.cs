using System;
using System.ComponentModel.DataAnnotations;

namespace MediaDataApplication.AspNetMvcClient.Models {

    public class MediaMetadataViewModel {
        [Required]
        public int MediaId { get; set; }

        [Required]
        public string MediaFileName { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Name")]
        public string FileName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Length (bytes)")]
        public Int64 FileLength { get; set; }
    }

}