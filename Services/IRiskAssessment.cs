using RiskManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskManagementSystem.Services
{
    public interface IRiskAssessmentService
    {
        Task<int> CalculateRiskScoreAsync(Claim claim);
        Task<bool> IsHighRiskAsync(Claim claim);
    }
}
