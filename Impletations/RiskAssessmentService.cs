using Microsoft.EntityFrameworkCore;
using RiskManagementSystem.Data;
using RiskManagementSystem.Models;
using RiskManagementSystem.Services;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

public class RiskAssessmentService : IRiskAssessmentService
{
    private readonly AppDbContext _context;

    public RiskAssessmentService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<int> CalculateRiskScoreAsync(Claim claim)
    {
        var rules = await _context.RiskRules.ToListAsync();
        int totalScore = 0;

        foreach (var rule in rules)
        {
            var expression = BuildExpression(rule.ConditionExpression);
            var compiled = expression.Compile();

            if (compiled(claim))
            {
                totalScore += rule.RiskScore;
            }
        }

        return totalScore;
    }
    public async Task<bool> IsHighRiskAsync(Claim claim)
    {
        int score = await CalculateRiskScoreAsync(claim);
        return score >= 70; 
    }
    private Expression<Func<Claim, bool>> BuildExpression(string condition)
    {
        var parameter = Expression.Parameter(typeof(Claim), "c");
        return DynamicExpressionParser.ParseLambda<Claim, bool>(new ParsingConfig(), false, condition);
    }
}