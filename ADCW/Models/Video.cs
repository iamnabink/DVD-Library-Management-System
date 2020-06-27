using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class Video
    {
        [Key]
        public int VideoId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        public DateTime DateReleased { get; set; }
        [Required]
        public float StandardCharge { get; set; }
        [Required]
        public float PenaltyCharge { get; set; }
        [Required]
        public int ProducerId { get; set; }
        [Required]
        public int StudioId { get; set; }
        [Required]
        public int VidCatId { get; set; }

        [ForeignKey("ProducerId")]
        public virtual Producer Producers { get; set; }
        [ForeignKey("StudioId")]
        public virtual Studio Studios { get; set; }
        [ForeignKey("VidCatId")]
        public virtual VideoCategory VidCategories { get; set; }
    }
}