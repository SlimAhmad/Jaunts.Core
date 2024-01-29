// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Customers;
using Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Customers
{
    public partial class CustomerServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomCustomerId = default;
            Guid inputCustomerId = randomCustomerId;

            var invalidCustomerException = new InvalidCustomerException(
                message: "Invalid Customer. Please fix the errors and try again.");

            invalidCustomerException.AddData(
                key: nameof(Customer.Id),
                values: "Id is required");

            var expectedCustomerValidationException =
                new CustomerValidationException(
                    message: "Customer validation error occurred, Please try again.",
                    innerException: invalidCustomerException);

            // when
            ValueTask<Customer> actualCustomerTask =
                this.customerService.RemoveCustomerByIdAsync(inputCustomerId);

            CustomerValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<CustomerValidationException>(
                 actualCustomerTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCustomerAsync(It.IsAny<Customer>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageCustomerIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Customer randomCustomer = CreateRandomCustomer(dateTime);
            Guid inputCustomerId = randomCustomer.Id;
            Customer inputCustomer = randomCustomer;
            Customer nullStorageCustomer = null;

            var notFoundCustomerException = new NotFoundCustomerException(inputCustomerId);

            var expectedCustomerValidationException =
                new CustomerValidationException(
                    message: "Customer validation error occurred, Please try again.",
                    innerException: notFoundCustomerException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerByIdAsync(inputCustomerId))
                    .ReturnsAsync(nullStorageCustomer);

            // when
            ValueTask<Customer> actualCustomerTask =
                this.customerService.RemoveCustomerByIdAsync(inputCustomerId);

            CustomerValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<CustomerValidationException>(
                 actualCustomerTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(inputCustomerId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCustomerAsync(It.IsAny<Customer>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
