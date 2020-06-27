using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class LoanType
    {
        [Key]
        public int LoanTypeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string LoanTypeTitle { get; set; }
    }
}