using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaDataApplication.WcfService.DAL.Entities {

    public class User {
        public int UserId { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(30)]
        public string UserName { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }

        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public virtual ICollection<Media> Media { get; set; }
    }

}