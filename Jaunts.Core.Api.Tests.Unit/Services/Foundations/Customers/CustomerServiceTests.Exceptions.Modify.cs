// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.customers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Customers;
using Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions;
using Microsoft.Data.SqlClient;
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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Customer someCustomer = CreateRandomCustomer(randomDateTime);
            someCustomer.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

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
            ValueTask<Customer> modifyCustomerTask =
                this.customerService.ModifyCustomerAsync(someCustomer);

                CustomerDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<CustomerDependencyException>(
                     modifyCustomerTask.AsTask);

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
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Customer someCustomer = CreateRandomCustomer(randomDateTime);
            someCustomer.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Customer> modifyCustomerTask =
                this.customerService.ModifyCustomerAsync(someCustomer);

            CustomerDependencyException actualDependencyException =
                await Assert.ThrowsAsync<CustomerDependencyException>(
                    modifyCustomerTask.AsTask);

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
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Customer randomCustomer = CreateRandomCustomer(randomDateTime);
            Customer someCustomer = randomCustomer;
            someCustomer.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedCustomerException = new LockedCustomerException(
                message: "Locked Customer record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedCustomerDependencyException =
                new CustomerDependencyException(
                    message: "Customer dependency error occurred, contact support.",
                    innerException: lockedCustomerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Customer> modifyCustomerTask =
                this.customerService.ModifyCustomerAsync(someCustomer);

            CustomerDependencyException actualDependencyException =
             await Assert.ThrowsAsync<CustomerDependencyException>(
                 modifyCustomerTask.AsTask);

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
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Customer randomCustomer = CreateRandomCustomer(randomDateTime);
            Customer someCustomer = randomCustomer;
            someCustomer.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Customer> modifyCustomerTask =
                this.customerService.ModifyCustomerAsync(someCustomer);

            CustomerServiceException actualServiceException =
             await Assert.ThrowsAsync<CustomerServiceException>(
                 modifyCustomerTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedCustomerServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
