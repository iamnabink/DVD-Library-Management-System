using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class MemberCategory
    {
        [Key]
        public int MemCatId { get; set; }
        [Required]
        [MaxLength(50)]
        public string MemCatName { get; set; }
        [Required]
        public int TotalLoans { get; set; }
        [Required]
        public int PenaltyDays { get; set; }

    }
}