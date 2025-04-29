using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DinarakProject01.Models
{
    public class DinarakClass
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(120)]
        public string UserName { get; set; }
        [MaxLength(120)]
        public string Password { get; set; }
        public string Role { get; set; }

        public DinarakClass() {
            Role = "User";
        }
    }
    
}