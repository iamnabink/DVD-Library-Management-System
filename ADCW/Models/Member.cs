using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; }
        [Required]
        [MaxLength(50)]
        public string MemberFirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string MemberLastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Address { get; set; }
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public int MemCatId { get; set; }

        [ForeignKey("MemCatId")]
        public virtual MemberCategory MemberCategories { get; set; }
    }
}