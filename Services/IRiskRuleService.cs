using RiskManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskManagementSystem.Services
{
    public interface IRiskRuleService
    {
        Task AddRiskRuleAsync(RiskRule rule);
        Task<RiskRule?> GetRiskRuleByIdAsync(int ruleId);
        Task<List<RiskRule>> GetAllRiskRulesAsync();
        Task<List<RiskRule>> GetApplicableRulesAsync(Claim claim);
    }
}
