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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
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
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Customer> retrieveByIdCustomerTask =
                this.customerService.RetrieveCustomerByIdAsync(someCustomerId);

            CustomerDependencyException actualDependencyException =
             await Assert.ThrowsAsync<CustomerDependencyException>(
                 retrieveByIdCustomerTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedCustomerDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
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
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Customer> retrieveByIdCustomerTask =
                this.customerService.RetrieveCustomerByIdAsync(someCustomerId);

            CustomerDependencyException actualDependencyException =
             await Assert.ThrowsAsync<CustomerDependencyException>(
                 retrieveByIdCustomerTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedCustomerDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
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
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
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
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Customer> retrieveByIdCustomerTask =
                this.customerService.RetrieveCustomerByIdAsync(someCustomerId);

            CustomerDependencyException actualDependencyException =
             await Assert.ThrowsAsync<CustomerDependencyException>(
                 retrieveByIdCustomerTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedCustomerDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
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
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Customer> retrieveByIdCustomerTask =
                this.customerService.RetrieveCustomerByIdAsync(someCustomerId);

            CustomerServiceException actualServiceException =
                 await Assert.ThrowsAsync<CustomerServiceException>(
                     retrieveByIdCustomerTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedCustomerServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
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
