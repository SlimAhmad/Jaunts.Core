// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.customers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Customers;
using Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Customers
{
    public partial class CustomerServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Customer someCustomer = CreateRandomCustomer(dateTime);
            someCustomer.UpdatedBy = someCustomer.CreatedBy;
            var sqlException = GetSqlException();

            var expectedFailedCustomerStorageException =
                new FailedCustomerStorageException(
                    message: "Failed Customer storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedCustomerDependencyException =
                new CustomerDependencyException(
                    message: "Customer dependency error occurred, contact support.",
                    innerException: expectedFailedCustomerStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Customer> createCustomerTask =
                this.customerService.CreateCustomerAsync(someCustomer);

            CustomerDependencyException actualDependencyException =
             await Assert.ThrowsAsync<CustomerDependencyException>(
                 createCustomerTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedCustomerDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCustomerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAsync(It.IsAny<Customer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Customer someCustomer = CreateRandomCustomer(dateTime);
            someCustomer.UpdatedBy = someCustomer.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedCustomerStorageException =
                new FailedCustomerStorageException(
                    message: "Failed Customer storage error occurred, Please contact support.",
                    databaseUpdateException);

            var expectedCustomerDependencyException =
                new CustomerDependencyException(
                    message: "Customer dependency error occurred, contact support.",
                    expectedFailedCustomerStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Customer> createCustomerTask =
                this.customerService.CreateCustomerAsync(someCustomer);

            CustomerDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<CustomerDependencyException>(
                     createCustomerTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedCustomerDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAsync(It.IsAny<Customer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Customer someCustomer = CreateRandomCustomer(dateTime);
            someCustomer.UpdatedBy = someCustomer.CreatedBy;
            var serviceException = new Exception();

            var failedCustomerServiceException =
                new FailedCustomerServiceException(
                    message: "Failed Customer service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedCustomerServiceException =
                new CustomerServiceException(
                    message: "Customer service error occurred, contact support.",
                    innerException: failedCustomerServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<Customer> createCustomerTask =
                 this.customerService.CreateCustomerAsync(someCustomer);

            CustomerServiceException actualDependencyException =
                 await Assert.ThrowsAsync<CustomerServiceException>(
                     createCustomerTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedCustomerServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAsync(It.IsAny<Customer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
