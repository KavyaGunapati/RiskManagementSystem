using RiskManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskManagementSystem.Services
{
    public interface ICustomerService
    {
        Task AddCustomerAsync(Customer customer);
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task<List<Customer>> GetAllCustomersAsync();
        Task<List<Customer>> GetTopCustomersByClaimAmountAsync(int topCount);
        Task <Customer> UpdateCustomerAsync(Customer customer);
    }
}
