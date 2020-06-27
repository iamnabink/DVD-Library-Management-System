using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class LoanMember
    {
        [Key]
        public int LoanId;
        [Required]
        public int LoanTypeId;
        [Required]
        public int MemberId;
        [Required]
        public int VideoId;
        [Required]
        public int DvdId;
        public DateTime DateOut;
        public DateTime DateDue;
        public Nullable<DateTime> DateReturned;
    }
}