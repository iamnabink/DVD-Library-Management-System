using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class CastMember
    {
        [Key]
        public int CastMemberId { get; set; }
        [Required]
        public int ActorId { get; set; }
        [Required]
        public int VideoId { get; set; }

        [ForeignKey("ActorId")]
        public virtual Actor Actors { get; set; }
        [ForeignKey("VideoId")]
        public virtual Video Videos { get; set; }
    }
}