using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DinarakProject01.Models
{
    public class BookClass 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int SerialNumber { get; set; }
        [Required]
        public int NumOfBooks { get; set; }
        
        public int Price { get; set; }
        [Required]
        public string BookName { get; set; }
        public string Author { get; set; }
        [Required]
        public string TypeOfBook { get; set; }
        [Required]
        public string ImageLink { get; set; }
        [Required]
        public string DescriptionOfBook { get; set; }

    }
}