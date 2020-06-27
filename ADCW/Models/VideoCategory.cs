using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class VideoCategory
    {
        [Key]
        public int VidCatId { get; set; }
        [Required]
        [MaxLength(50)]
        public string VidCatName { get; set; }
        [Required]
        public bool AgeRestricted { get; set; }
    }
}