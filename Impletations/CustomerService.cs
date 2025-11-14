using Microsoft.EntityFrameworkCore;
using RiskManagementSystem.Data;
using RiskManagementSystem.Models;
using RiskManagementSystem.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddCustomerAsync(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.Name))
            throw new ArgumentException("Customer name is required.");

        if (customer.Age < 18)
            throw new ArgumentException("Customer age must be at least 18.");

        if (string.IsNullOrWhiteSpace(customer.Region))
            throw new ArgumentException("Region is required.");

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }


    public async Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
        return await _context.Customers
            .Include(c => c.Claims)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

 
    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .Include(c => c.Claims)
            .ToListAsync();
    }

  
    public async Task<List<Customer>> GetTopCustomersByClaimAmountAsync(int topCount)
    {
        return await _context.Customers
            .Include(c => c.Claims)
            .OrderByDescending(c => c.Claims.Sum(cl => cl.ClaimAmount))
            .Take(topCount)
            .ToListAsync();
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
}