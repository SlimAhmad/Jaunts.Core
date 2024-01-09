using Jaunts.Core.Api.Models.Services.Foundations.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Customer> Customer { get; set; }

        public async ValueTask<Customer> InsertCustomerAsync(Customer customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Customer> customerEntityEntry = await broker.Customer.AddAsync(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }

        public IQueryable<Customer> SelectAllCustomers() => this.Customer;

        public async ValueTask<Customer> SelectCustomerByIdAsync(Guid customerId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Customer.FindAsync(customerId);
        }

        public async ValueTask<Customer> UpdateCustomerAsync(Customer customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Customer> customerEntityEntry = broker.Customer.Update(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }

        public async ValueTask<Customer> DeleteCustomerAsync(Customer customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Customer> customerEntityEntry = broker.Customer.Remove(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }
    }
}
