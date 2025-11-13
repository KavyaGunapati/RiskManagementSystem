using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskManagementSystem.Models
{
    public class RiskRule
    {
        public int RiskRuleId { get; set; }
        [Required]
        public string RuleName { get; set; }
        public string ConditionExpression { get; set; }  
        public int RiskScore { get; set; }
        public List<Claim> Claims { get; set; }=new List<Claim>();
    }
}
