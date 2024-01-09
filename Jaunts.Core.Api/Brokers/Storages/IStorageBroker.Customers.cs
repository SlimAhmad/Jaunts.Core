using Jaunts.Core.Api.Models.Services.Foundations.Customers;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Customer> InsertCustomerAsync(
            Customer customer);

        IQueryable<Customer> SelectAllCustomers();

        ValueTask<Customer> SelectCustomerByIdAsync(
            Guid customerId);

        ValueTask<Customer> UpdateCustomerAsync(
            Customer customer);

        ValueTask<Customer> DeleteCustomerAsync(
            Customer customer);
    }
}
