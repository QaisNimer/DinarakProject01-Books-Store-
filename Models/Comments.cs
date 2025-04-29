using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace DinarakProject01.Models
{
    public class Comments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int CommentId { get; set; }

        [Required]
        public int SerialNumber { get; set; }
        [ForeignKey("SerialNumber")]

        public BookClass Book { get; set; }

        public Guid GuestId { get; set; }

        public string CommentDescription { get; set; }

        public int Rating { get; set; }

        public DateTime CommentedOn { get; set;}

    }
}