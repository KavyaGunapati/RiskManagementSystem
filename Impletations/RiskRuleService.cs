using Microsoft.EntityFrameworkCore;
using RiskManagementSystem.Data;
using RiskManagementSystem.Models;
using RiskManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskManagementSystem.Impletations
{

    public class RiskRuleService:IRiskRuleService
    {
    private readonly AppDbContext _context;
        public RiskRuleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRiskRuleAsync(RiskRule rule)
        {
            if (string.IsNullOrWhiteSpace(rule.RuleName))
                throw new ArgumentException("Rule name is required.");

            if (string.IsNullOrWhiteSpace(rule.ConditionExpression))
                throw new ArgumentException("Condition expression is required.");

            await _context.RiskRules.AddAsync(rule);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RiskRule>> GetAllRiskRulesAsync()
        {

            return await _context.RiskRules
                    .Include(r => r.Claims)
                    .ToListAsync();

        }

        public async Task<List<RiskRule>> GetApplicableRulesAsync(Claim claim)
        {

            return await _context.RiskRules.ToListAsync();

        }
        public async Task<RiskRule?> GetRiskRuleByIdAsync(int ruleId)
        {
            return await _context.RiskRules.Include(r=>r.Claims).FirstOrDefaultAsync(r=>r.RiskRuleId == ruleId);
        }
    }
}
