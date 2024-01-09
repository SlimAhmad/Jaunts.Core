// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Customers;

namespace Jaunts.Core.Api.Services.Foundations.Customers
{
    public interface ICustomerService
    {
        ValueTask<Customer> RegisterCustomerAsync(Customer customer);
        IQueryable<Customer> RetrieveAllCustomers();
        ValueTask<Customer> RetrieveCustomerByIdAsync(Guid customerId);
        ValueTask<Customer> ModifyCustomerAsync(Customer customer);
        ValueTask<Customer> RemoveCustomerByIdAsync(Guid customerId);
    }
}