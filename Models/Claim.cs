using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskManagementSystem.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }
        [Range(0.01,double.MaxValue)]
        public decimal ClaimAmount { get; set; }
        [DataType(DataType.Date)]
        public DateTime ClaimDate { get; set; }
        public string? Description { get; set; }
        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public List<RiskRule> RiskRules { get; set; }=new List<RiskRule>();

    }
    public enum ClaimStatus {
            Pending,
            UnderReview,
            Approved,
            Rejected
        }
}
