// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Customers;

namespace Jaunts.Core.Api.Services.Foundations.Customers
{
    public partial class CustomerService : ICustomerService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public CustomerService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Customer> RegisterCustomerAsync(Customer Customer) =>
        TryCatch(async () =>
        {
            ValidateCustomerOnRegister(Customer);

            return await this.storageBroker.InsertCustomerAsync(Customer);
        });

        public IQueryable<Customer> RetrieveAllCustomers() =>
        TryCatch(() => this.storageBroker.SelectAllCustomers());

        public ValueTask<Customer> RetrieveCustomerByIdAsync(Guid CustomerId) =>
        TryCatch(async () =>
        {
            ValidateCustomerId(CustomerId);
            Customer maybeCustomer = await this.storageBroker.SelectCustomerByIdAsync(CustomerId);
            ValidateStorageCustomer(maybeCustomer, CustomerId);

            return maybeCustomer;
        });

        public ValueTask<Customer> ModifyCustomerAsync(Customer Customer) =>
        TryCatch(async () =>
        {
            ValidateCustomerOnModify(Customer);

            Customer maybeCustomer =
                await this.storageBroker.SelectCustomerByIdAsync(Customer.Id);

            ValidateStorageCustomer(maybeCustomer, Customer.Id);
            ValidateAgainstStorageCustomerOnModify(inputCustomer: Customer, storageCustomer: maybeCustomer);

            return await this.storageBroker.UpdateCustomerAsync(Customer);
        });

        public ValueTask<Customer> RemoveCustomerByIdAsync(Guid CustomerId) =>
        TryCatch(async () =>
        {
            ValidateCustomerId(CustomerId);

            Customer maybeCustomer =
                await this.storageBroker.SelectCustomerByIdAsync(CustomerId);

            ValidateStorageCustomer(maybeCustomer, CustomerId);

            return await this.storageBroker.DeleteCustomerAsync(maybeCustomer);
        });

    }
}
