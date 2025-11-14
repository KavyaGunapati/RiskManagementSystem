using Microsoft.EntityFrameworkCore;
using RiskManagementSystem.Data;
using RiskManagementSystem.Models;
using RiskManagementSystem.Services;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Security.Claims;

public class RiskAssessmentService : IRiskAssessmentService
{
    private readonly AppDbContext _context;

    public RiskAssessmentService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<int> CalculateRiskScoreAsync(RiskManagementSystem.Models.Claim claim)
    {
        if (claim.Customer == null && claim.CustomerId != 0)
        {
            claim.Customer = await _context.Customers.FindAsync(claim.CustomerId);
        }

        var rules = await _context.RiskRules.ToListAsync();
        int totalScore = 0;
        foreach (var rule in rules)
        {
            var expression = BuildExpression(rule.ConditionExpression);
            var compiled = expression.Compile();
            if (compiled(claim))
                totalScore += rule.RiskScore;
        }
        return totalScore;
    }

    public Task<int> CalculateRiskScoreAsync(RiskManagementSystem.Models.Claim claim)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsHighRiskAsync(RiskManagementSystem.Models.Claim claim)
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
//public Func<Claim, bool> RiskAssesser(string Property, string Operator, string Value)
//{
//    var param = Expression.Parameter(typeof(Claim), "c");
//    var property = Expression.Property(param, Property);
//    var value = Expression.Constant(Convert.ChangeType(Value, property.Type));
//    var Operation = Operator switch
//    {
//        "GreaterThan" => Expression.GreaterThan(property, value),
//        "LessThan" => Expression.LessThan(property, value),
//        "Equal" => Expression.Equal(property, value),
//        _ => throw new NotSupportedException($"Operator '{Operator}' is not supported.")
//    };
//    return Expression.Lambda<Func<Claim, bool>>(Operation, param).Compile();
//}

//public async Task<List<Claim>> CalculateRiskScoreAsync()
//{
//    var claims = await _context.Claims.ToListAsync();
//    var riskrules = await _context.RiskRules.ToListAsync();
//    var suspicious = new List<Claim>();
//    foreach (var claim in claims)
//    {
//        if (claim.RiskScore > 0)
//        {

//            suspicious.Add(claim);
//            claim.Status = Claim.ClaimStatus.Pending;
//        }
//        else if (claim.RiskScore > 3)
//        {
//            claim.Status = Claim.ClaimStatus.Rejected;
//        }
//        else
//        {
//            claim.Status = Claim.ClaimStatus.Approved;
//        }
//        foreach (var riskRule in riskrules)
//        {

//            var filtered = filterService.RiskAssesser(riskRule.RuleProperty, riskRule.RuleOperator, riskRule.RuleValue);
//            if (filtered(claim) == true)
//            {
//                claim.RiskScore += 1;
//            }

//}