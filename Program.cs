using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RiskManagementSystem.Data;
using RiskManagementSystem.Impletations;
using RiskManagementSystem.Models;
using RiskManagementSystem.Services;

class Program
{
    static async Task Main(string[] args)
    {
        // Configure DI and DbContext
        var services = new ServiceCollection();
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=RiskManagementDb;Trusted_Connection=True;"));

        // Register services
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<IRiskRuleService, RiskRuleService>();
        services.AddScoped<IRiskAssessmentService, RiskAssessmentService>();

        var provider = services.BuildServiceProvider();

        // Resolve services
        var customerService = provider.GetRequiredService<ICustomerService>();
        var claimService = provider.GetRequiredService<IClaimService>();
        var riskRuleService = provider.GetRequiredService<IRiskRuleService>();

        Console.WriteLine("=== Risk Management System ===");

        // 1. Add sample customer
        var customer = new Customer
        {
            Name = "John Doe",
            Age = 24,
            Region = "HighRiskZone",
            Email = "john@example.com"
        };
        await customerService.AddCustomerAsync(customer);
        Console.WriteLine("Customer added.");

        // 2. Add sample risk rule
        var rule = new RiskRule
        {
            RuleName = "High Amount Young Customer",
            ConditionExpression = "ClaimAmount > 10000 && Customer.Age < 25 && Customer.Region == \"HighRiskZone\"",
            RiskScore = 50
        };
        await riskRuleService.AddRiskRuleAsync(rule);
        Console.WriteLine("Risk rule added.");

        // 3. Add sample claim
        var claim = new Claim
        {
            ClaimAmount = 15000,
            ClaimDate = DateTime.Now.AddDays(-2),
            Description = "Accident claim",
            CustomerId = customer.CustomerId
        };
        await claimService.AddClaimAsync(claim);
        Console.WriteLine($"Claim added with status: {claim.Status}");

        // 4. Fetch suspicious claims
        var suspiciousClaims = await claimService.GetSuspiciousClaimsAsync();
        Console.WriteLine($"Suspicious Claims Count: {suspiciousClaims.Count}");

        // 5. Analytics: Top customers
        var topCustomers = await customerService.GetTopCustomersByClaimAmountAsync(10);
        Console.WriteLine("Top Customers by Claim Amount:");
        foreach (var c in topCustomers)
        {
            Console.WriteLine($"{c.Name} - Total Claims: {c.Claims.Sum(cl => cl.ClaimAmount)}");
        }

        Console.WriteLine("=== End of Demo ===");
    }
}