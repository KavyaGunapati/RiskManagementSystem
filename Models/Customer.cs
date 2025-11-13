using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskManagementSystem.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(18,int.MaxValue)]
        public int Age { get; set; }
        [EmailAddress]
        public string? Email { get; set; } 
        [Required]
        public string Region { get; set; }
        public List<Claim> Claims { get; set; }  = new List<Claim>();
    }
}
