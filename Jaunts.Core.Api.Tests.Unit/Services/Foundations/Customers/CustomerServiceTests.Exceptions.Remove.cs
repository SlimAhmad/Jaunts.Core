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
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCustomerId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedCustomerStorageException =
              new FailedCustomerStorageException(
                  message: "Failed Customer storage error occurred, Please contact support.",
                  sqlException);

            var expectedCustomerDependencyException =
                new CustomerDependencyException(
                    message: "Customer dependency error occurred, contact support.",
                    expectedFailedCustomerStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerByIdAsync(someCustomerId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Customer> deleteCustomerTask =
                this.customerService.RemoveCustomerByIdAsync(someCustomerId);

            CustomerDependencyException actualDependencyException =
                await Assert.ThrowsAsync<CustomerDependencyException>(
                    deleteCustomerTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedCustomerDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(someCustomerId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCustomerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCustomerId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedCustomerStorageException =
              new FailedCustomerStorageException(
                  message: "Failed Customer storage error occurred, Please contact support.",
                  databaseUpdateException);

            var expectedCustomerDependencyException =
                new CustomerDependencyException(
                    message: "Customer dependency error occurred, contact support.",
                    expectedFailedCustomerStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerByIdAsync(someCustomerId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Customer> deleteCustomerTask =
                this.customerService.RemoveCustomerByIdAsync(someCustomerId);

            CustomerDependencyException actualDependencyException =
                await Assert.ThrowsAsync<CustomerDependencyException>(
                    deleteCustomerTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedCustomerDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(someCustomerId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCustomerId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedCustomerException = new LockedCustomerException(
                message: "Locked Customer record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedCustomerDependencyException =
                new CustomerDependencyException(
                    message: "Customer dependency error occurred, contact support.",
                    innerException: lockedCustomerException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerByIdAsync(someCustomerId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Customer> deleteCustomerTask =
                this.customerService.RemoveCustomerByIdAsync(someCustomerId);

            CustomerDependencyException actualDependencyException =
                await Assert.ThrowsAsync<CustomerDependencyException>(
                    deleteCustomerTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedCustomerDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(someCustomerId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCustomerId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedCustomerServiceException =
             new FailedCustomerServiceException(
                 message: "Failed Customer service error occurred, Please contact support.",
                 innerException: serviceException);

            var expectedCustomerServiceException =
                new CustomerServiceException(
                    message: "Customer service error occurred, contact support.",
                    innerException: failedCustomerServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerByIdAsync(someCustomerId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Customer> deleteCustomerTask =
                this.customerService.RemoveCustomerByIdAsync(someCustomerId);

            CustomerServiceException actualServiceException =
             await Assert.ThrowsAsync<CustomerServiceException>(
                 deleteCustomerTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedCustomerServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(someCustomerId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
