using Microsoft.EntityFrameworkCore;
using RiskManagementSystem.Data;
using RiskManagementSystem.Models;
using RiskManagementSystem.Services;

public class ClaimService : IClaimService
{
    private readonly AppDbContext _context;
    private readonly IRiskAssessmentService _riskAssessmentService;

    public ClaimService(AppDbContext context, IRiskAssessmentService riskAssessmentService)
    {
        _context = context;
        _riskAssessmentService = riskAssessmentService;
    }

  
    public async Task AddClaimAsync(Claim claim)
    {
        if (claim.ClaimAmount <= 0)
            throw new ArgumentException("Claim amount must be positive.");

        if (claim.ClaimDate >= DateTime.Now)
            throw new ArgumentException("Claim date must be in the past.");

       
        int riskScore = await _riskAssessmentService.CalculateRiskScoreAsync(claim);
        if (riskScore >= 70)
            claim.Status = ClaimStatus.UnderReview;

        await _context.Claims.AddAsync(claim);
        await _context.SaveChangesAsync();
    }

    public async Task<Claim?> GetClaimByIdAsync(int claimId)
    {
        return await _context.Claims
            .Include(c => c.Customer)
            .Include(c => c.RiskRules)
            .FirstOrDefaultAsync(c => c.ClaimId == claimId);
    }

    public async Task<List<Claim>> GetAllClaimsAsync()
    {
        return await _context.Claims
            .Include(c => c.Customer)
            .Include(c => c.RiskRules)
            .ToListAsync();
    }

    public async Task UpdateClaimStatusAsync(int claimId, ClaimStatus status)
    {
        var claim = await _context.Claims.FindAsync(claimId);
        if (claim == null) throw new KeyNotFoundException("Claim not found.");

        claim.Status = status;
        _context.Entry(claim).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Claim>> GetSuspiciousClaimsAsync()
    {
        return await _context.Claims
            .Where(c => c.ClaimAmount > 10000 || c.RiskRules.Any())
            .OrderByDescending(c => c.ClaimAmount)
            .Include(c => c.Customer)
            .ToListAsync();
    }
    public async Task ProcessClaimsAsync(IEnumerable<Claim> claims)
    {
        foreach (var claim in claims)
        {
            int riskScore = await _riskAssessmentService.CalculateRiskScoreAsync(claim);
            if (riskScore >= 70)
                claim.Status = ClaimStatus.UnderReview;

            _context.Entry(claim).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync();
    }
}