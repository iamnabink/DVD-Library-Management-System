using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class DVD
    {
        [Key]
        public int DVDId { get; set; }
        [Required]
        public int VideoId { get; set; }
        [Required]
        public DateTime DateAdded { get; set; }
        [ForeignKey("VideoId")]
        public virtual Video Videos { get; set; }
    }
}