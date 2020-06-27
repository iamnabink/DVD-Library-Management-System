using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class Actor
    {
        [Key]
        public int ActorId { get; set; }
        [Required]
        [MaxLength(50)]
        public string ActorFirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string ActorLastName { get; set; }
        [Required]
        public string Gender { get; set; }
    }
}