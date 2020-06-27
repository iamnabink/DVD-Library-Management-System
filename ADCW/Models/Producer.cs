using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class Producer
    {
        [Key]
        public int ProducerId { get; set; }
        [Required]
        [MaxLength(50)]
        public string ProducerFirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string ProducerLastName { get; set; }
    }
}