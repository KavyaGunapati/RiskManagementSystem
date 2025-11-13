using RiskManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskManagementSystem.Services
{
    public interface IClaimService
    {
        Task AddClaimAsync(Claim claim);
        Task<Claim?> GetClaimByIdAsync(int claimId);
        Task<List<Claim>> GetAllClaimsAsync();
        Task UpdateClaimStatusAsync(int claimId, ClaimStatus status);
        Task<List<Claim>> GetSuspiciousClaimsAsync();
        Task ProcessClaimsAsync(IEnumerable<Claim> claims);
    }

}
