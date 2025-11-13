using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskManagementSystem.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public decimal ClaimAmount { get; set; }
        public DateTime ClaimDate { get; set; }
        public string Description { get; set; }
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
