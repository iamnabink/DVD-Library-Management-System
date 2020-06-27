using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class Studio
    {
        [Key]
        public int StudioId { get; set; }
        [Required]
        [MaxLength(50)]
        public string StudioName { get; set; }
    }
}