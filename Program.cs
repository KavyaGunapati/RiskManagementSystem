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
        var services = new ServiceCollection();
        services.AddDbContext<AppDbContext>();

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<IRiskRuleService, RiskRuleService>();
        services.AddScoped<IRiskAssessmentService, RiskAssessmentService>();

        var provider = services.BuildServiceProvider();

        var customerService = provider.GetRequiredService<ICustomerService>();
        var claimService = provider.GetRequiredService<IClaimService>();
        var riskRuleService = provider.GetRequiredService<IRiskRuleService>();

        while (true)
        {
            Console.WriteLine("=== Risk Management System ===");
            Console.WriteLine("1.Add Customer");
            Console.WriteLine("2. View All Customers");
            Console.WriteLine("3.View Customer by Id");
            Console.WriteLine("4.Add Risk Rule");
            Console.WriteLine("5.Add Claim");
            Console.WriteLine("6.Get SuspeciusClaims");
            Console.WriteLine("7.Process Claims");
            Console.WriteLine("8.Update Claim Status");
            Console.WriteLine("9.Get All Claims");
            Console.WriteLine("10.Update customer");
            Console.WriteLine("11.Get Top Customers by Claim amount");
            Console.WriteLine("0.Exit");
            Console.WriteLine("enter your Choice:");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter Customer Name");
                    string name= Console.ReadLine();
                    if (string.IsNullOrEmpty(name))
                    {
                        Console.WriteLine("Enter Name:");
                        break;
                    }
                    Console.WriteLine("Enter Age:");
                    if (!int.TryParse(Console.ReadLine(), out int age))
                    {
                        Console.WriteLine("Age must be greater than 18");
                        break;
                    }
                    Console.WriteLine("Enter Email:");
                    string email = Console.ReadLine();
                    if (string.IsNullOrEmpty(email))
                    {
                        Console.WriteLine("Enter valid email");
                        break;
                    }
                    Console.WriteLine("Enter Reagion");
                    string reagion= Console.ReadLine();
                    if (string.IsNullOrEmpty(reagion))
                    {
                        Console.WriteLine("Enter a valid Reagion");
                        break;
                    }
                    var customer=new Customer { Name = name ,Email=email,Age=age,Region=reagion};
                    await customerService.AddCustomerAsync(customer);
                    Console.WriteLine("Customer added successfully");
                    break;
                case 2: 
                         var customers=await customerService.GetAllCustomersAsync();
                    if (customers.Count == 0)
                    {
                        Console.WriteLine("No customers Found");
                        break;
                    }
                    foreach(var c in customers)
                    {
                        Console.WriteLine($"{c.Name} - {c.Email}-{c.Age} - {c.Region}");
                    }
                    break;
                case 3:
                    Console.WriteLine("Enter Customer Id:");
                    if(!int.TryParse(Console.ReadLine(),out int id))
                    {
                        Console.WriteLine("enter valid Id");
                        break;
                    }
                       var cust=await customerService.GetCustomerByIdAsync(id);
                    if (cust == null)
                    {
                        Console.WriteLine("No customer found");
                        break;
                    }
                    Console.WriteLine($"{cust.Name} - {cust.Email} - {cust.Age}- {cust.Region}");
                    break;
                case 4:
                    Console.WriteLine("Enter Risk Rule Name");
                    string riskName= Console.ReadLine();
                    if (string.IsNullOrEmpty(riskName))
                    {
                        Console.WriteLine("Invalid Rule Name");
                        break;
                    }
                    Console.WriteLine("Enter a Condition Expression:");
                    string conditionExp= Console.ReadLine();
                    if (string.IsNullOrEmpty(conditionExp))
                    {
                        Console.WriteLine("Invalid exp");
                        break;
                    }
                    Console.WriteLine("Enter riskScore");
                    if(!int.TryParse(Console.ReadLine(),out int riskScore))
                    {
                        Console.WriteLine("inavlid score");
                        break;
                    }
                    var riskRule=new RiskRule { RuleName = riskName, ConditionExpression=conditionExp,RiskScore=riskScore };
                    await riskRuleService.AddRiskRuleAsync(riskRule);
                    Console.WriteLine("Risk rule Added Successfully");
                    break;
                case 5:
                    Console.WriteLine("Enter claim amount");
                    if (!double.TryParse(Console.ReadLine(), out double claimAmt))
                    {
                        Console.WriteLine("invalid claim amount");
                        break;
                    }
                    Console.WriteLine("Enter claim date (yyyy-mm-dd)");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime claimDate))
                    {
                        Console.WriteLine("Enter a valid date ");
                        break;
                    }
                    Console.WriteLine("Enter Description:");
                    string description = Console.ReadLine()??"";
                    Console.WriteLine("Enter Claim Status");
                    string claimStatus = Console.ReadLine();
                    Console.WriteLine("Enter Customer ID");
                    if(!int.TryParse(Console.ReadLine(),out int customerId))
                    {
                        Console.WriteLine("enter valid customer Id");
                        break;
                    }
                    var claim = new Claim
                    {
                        ClaimAmount = (decimal)claimAmt, 
                        ClaimDate = claimDate,
                        Description = description,
                        Status = Enum.TryParse<ClaimStatus>(claimStatus, out var status) ? status : ClaimStatus.Pending, 
                        CustomerId = customerId
                    };
                    await claimService.AddClaimAsync(claim);
                    Console.WriteLine("Claim Added Successfully");
                    break;
                case 6:
                    var claims= await claimService.GetSuspiciousClaimsAsync();
                    if (claims == null)
                    {
                        Console.WriteLine("No suspicious claims");
                        break;
                    }
                    foreach(var cl in claims)
                    {
                        Console.WriteLine($"{cl.ClaimAmount} - {cl.ClaimDate} - {cl.Description} - {cl.Status} - {cl.CustomerId}");
                    }
                    break;

                case 7:
                    var allClaims = await claimService.GetAllClaimsAsync();
                     await claimService.ProcessClaimsAsync(allClaims);
                    Console.WriteLine("All Claims Processing...");
                    break;
            
                case 8:
                    Console.WriteLine("Enter Claim ID:");
                    if (!int.TryParse(Console.ReadLine(), out int claimId))
                    {
                        Console.WriteLine("Invalid Claim ID");
                        break;
                    }

                    Console.WriteLine("Enter new Claim Status (Pending, UnderReview, Approved, Rejected):");
                    string claimStatusInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(claimStatusInput))
                    {
                        Console.WriteLine("Status required");
                        break;
                    }

                    if (!Enum.TryParse<ClaimStatus>(claimStatusInput, true, out var newStatus))
                    {
                        Console.WriteLine("Invalid status. Valid options: Pending, UnderReview, Approved, Rejected");
                        break;
                    }

                    await claimService.UpdateClaimStatusAsync(claimId, newStatus);
                    Console.WriteLine($"Claim {claimId} status updated to {newStatus}");
                    break;
                case 9:
                    var allCls= await claimService.GetAllClaimsAsync();
                    foreach(var cl in allCls)
                    {
                        Console.WriteLine($"{cl.ClaimAmount} - {cl.ClaimDate} - {cl.Description} - {cl.Status} - {cl.CustomerId}");
                    }
                    break;
                case 10:
                    Console.WriteLine("Enter Customer ID to Update:");
                    if (!int.TryParse(Console.ReadLine(), out int updateCustomerId)) { Console.WriteLine("Invalid ID."); break; }
                    var customerExist=await customerService.GetCustomerByIdAsync(updateCustomerId);
                    if (customerExist == null)
                    {
                        Console.WriteLine("customer not found");
                        break;
                    }
                    Console.WriteLine($"Enter New Customer Name ({customerExist.Name})");
                    string newName = Console.ReadLine();
                    Console.WriteLine($"Enter new Email ({customerExist.Email})");
                    string newEmail = Console.ReadLine();
                    Console.WriteLine($"Enter new Age ({customerExist.Age})");
                    int newAge= int.Parse(Console.ReadLine());
                    Console.WriteLine($"Enter new Reagion ({customerExist.Region}):");
                    string newRegion= Console.ReadLine();
                    customerExist.Name=string.IsNullOrEmpty(newName)?customerExist.Name:newName;
                    customerExist.Email=string.IsNullOrEmpty(newEmail)?customerExist.Email:newEmail;
                    customerExist.Age=newAge>=18?customerExist.Age:newAge;
                    customerExist.Region=string.IsNullOrEmpty(newRegion)?customerExist.Region:newRegion;
                    await customerService.UpdateCustomerAsync(customerExist);
                    Console.WriteLine("Customer updated SuccessFully");
                    break;
                case 11:
                    Console.WriteLine("Enter topCount");
                    if(!int.TryParse(Console.ReadLine(),out int topCount)){
                        Console.WriteLine("Enter valid top Count");
                        break;
                    }
                    var topCustomers=await customerService.GetTopCustomersByClaimAmountAsync(topCount);
                    foreach (var c in topCustomers) {
                        Console.WriteLine($"{c.Name} - {c.Email} - {c.Age} - {c.Region}");
                    }
                    break;
                case 0:
                    Console.WriteLine("Exiting the Risk Management System");
                    return;
            }
        }
    }
}