// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.customers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Customers
{
    public partial class CustomerServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllCustomers())
                    .Throws(sqlException);

            // when
            Action retrieveAllCustomersAction = () =>
                this.customerService.RetrieveAllCustomers();

            CustomerDependencyException actualDependencyException =
              Assert.Throws<CustomerDependencyException>(
                 retrieveAllCustomersAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedCustomerDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCustomers(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllCustomers())
                    .Throws(serviceException);

            // when
            Action retrieveAllCustomersAction = () =>
                this.customerService.RetrieveAllCustomers();

            CustomerServiceException actualServiceException =
              Assert.Throws<CustomerServiceException>(
                 retrieveAllCustomersAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedCustomerServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCustomers(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
