using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }
        [Required]
        public int LoanTypeId { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int DvdId { get; set; }
        [Required]
        public DateTime DateOut { get; set; }
        [Required]
        public DateTime DateDue { get; set; }
        public Nullable<DateTime> DateReturned { get; set; }

        [ForeignKey("LoanTypeId")]
        public virtual LoanType LoanTypes { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member Members { get; set; }
        [ForeignKey("DvdId")]
        public virtual DVD Dvds { get; set; }
    }
}