using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class MemberLoans
    {
        public int MemberId;
        public string MemberFirstName;
        public string MemberLastName;
        public string Address;
        public string PhoneNumber;
        public DateTime DateOfBirth;
        public MemberCategory category;
        public int TotalLoaned;
    }
}