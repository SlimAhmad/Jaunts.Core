// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Customers;
using Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Hosting;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Customers
{
    public partial class CustomerServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
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

            //when
            ValueTask<Customer> retrieveCustomerByIdTask =
                this.customerService.RetrieveCustomerByIdAsync(inputCustomerId);

            CustomerValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<CustomerValidationException>(
                 retrieveCustomerByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageCustomerIsNullAndLogItAsync()
        {
            //given
            Guid randomCustomerId = Guid.NewGuid();
            Guid someCustomerId = randomCustomerId;
            Customer invalidStorageCustomer = null;
            var notFoundCustomerException = new NotFoundCustomerException(
                message: $"Couldn't find customer with id: {someCustomerId}.");

            var expectedCustomerValidationException =
                new CustomerValidationException(
                    message: "Customer validation error occurred, Please try again.",
                    innerException: notFoundCustomerException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageCustomer);

            //when
            ValueTask<Customer> retrieveCustomerByIdTask =
                this.customerService.RetrieveCustomerByIdAsync(someCustomerId);

            CustomerValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<CustomerValidationException>(
                 retrieveCustomerByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}