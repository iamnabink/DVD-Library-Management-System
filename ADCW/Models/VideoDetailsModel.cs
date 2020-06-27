using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADCW.Models
{
    public class VideoDetailsModel
    {
        public int VideoId{ get; set; }
        [Required]
        [MaxLength(50)]
        public string Title{ get; set; }
        [Required]
        public DateTime DateReleased{ get; set; }
        [Required]
        public float StandardCharge{ get; set; }
        [Required]
        public float PenaltyCharge{ get; set; }
        [Required]
        public int ProducerId{ get; set; }
        [Required]
        public int StudioId{ get; set; }
        [Required]
        public int VidCatId{ get; set; }
        public int[] ActorIds { get; set; }
        public IEnumerable<SelectListItem> Actors { get; set; }
    }
}