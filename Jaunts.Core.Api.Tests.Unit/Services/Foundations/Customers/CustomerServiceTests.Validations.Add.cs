// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Customers;
using Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Customers
{
    public partial class CustomerServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCustomerIsNullAndLogItAsync()
        {
            // given
            Customer randomCustomer = null;
            Customer nullCustomer = randomCustomer;

            var nullCustomerException = new NullCustomerException(
                message: "The customer is null.");

            var expectedCustomerValidationException =
                new CustomerValidationException(
                    message: "Customer validation error occurred, Please try again.",
                    innerException: nullCustomerException);

            // when
            ValueTask<Customer> createCustomerTask =
                this.customerService.CreateCustomerAsync(nullCustomer);

             CustomerValidationException actualCustomerDependencyValidationException =
             await Assert.ThrowsAsync<CustomerValidationException>(
                 createCustomerTask.AsTask);

            // then
            actualCustomerDependencyValidationException.Should().BeEquivalentTo(
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


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenCustomerIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidCustomer = new Customer
            {
                Address = invalidText,
                FirstName = invalidText,
                LastName = invalidText,
                MiddleName = invalidText
            };

            var invalidCustomerException = new InvalidCustomerException();

            invalidCustomerException.AddData(
                key: nameof(Customer.Id),
                values: "Id is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.Address),
                values: "Text is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.FirstName),
                values: "Text is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.LastName),
                values: "Text is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.MiddleName),
                values: "Text is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.UserId),
                values: "Id is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.CreatedBy),
                values: "Id is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.UpdatedBy),
                values: "Id is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.BirthDate),
                values: "Date is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.CreatedDate),
                values: "Date is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.UpdatedDate),
                values: "Date is required");

            var expectedCustomerValidationException =
                new CustomerValidationException(
                    message: "Customer validation error occurred, Please try again.",
                    innerException: invalidCustomerException);

            // when
            ValueTask<Customer> createCustomerTask =
                this.customerService.CreateCustomerAsync(invalidCustomer);

             CustomerValidationException actualCustomerDependencyValidationException =
             await Assert.ThrowsAsync<CustomerValidationException>(
                 createCustomerTask.AsTask);

            // then
            actualCustomerDependencyValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            Customer randomCustomer = CreateRandomCustomer(dateTime);
            Customer inputCustomer = randomCustomer;
            inputCustomer.UpdatedBy = Guid.NewGuid();

            var invalidCustomerException = new InvalidCustomerException();

            invalidCustomerException.AddData(
                key: nameof(Customer.UpdatedBy),
                values: $"Id is not the same as {nameof(Customer.CreatedBy)}");

            var expectedCustomerValidationException =
                new CustomerValidationException(
                    message: "Customer validation error occurred, Please try again.",
                    innerException: invalidCustomerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Customer> createCustomerTask =
                this.customerService.CreateCustomerAsync(inputCustomer);

             CustomerValidationException actualCustomerDependencyValidationException =
             await Assert.ThrowsAsync<CustomerValidationException>(
                 createCustomerTask.AsTask);

            // then
            actualCustomerDependencyValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Customer randomCustomer = CreateRandomCustomer(dateTime);
            Customer inputCustomer = randomCustomer;
            inputCustomer.UpdatedBy = randomCustomer.CreatedBy;
            inputCustomer.UpdatedDate = GetRandomDateTime();

            var invalidCustomerException = new InvalidCustomerException();

            invalidCustomerException.AddData(
                key: nameof(Customer.UpdatedDate),
                values: $"Date is not the same as {nameof(Customer.CreatedDate)}");

            var expectedCustomerValidationException =
                new CustomerValidationException(
                    message: "Customer validation error occurred, Please try again.",
                    innerException: invalidCustomerException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Customer> createCustomerTask =
                this.customerService.CreateCustomerAsync(inputCustomer);

             CustomerValidationException actualCustomerDependencyValidationException =
             await Assert.ThrowsAsync<CustomerValidationException>(
                 createCustomerTask.AsTask);

            // then
            actualCustomerDependencyValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Customer randomCustomer = CreateRandomCustomer(dateTime);
            Customer inputCustomer = randomCustomer;
            inputCustomer.UpdatedBy = inputCustomer.CreatedBy;
            inputCustomer.CreatedDate = dateTime.AddMinutes(minutes);
            inputCustomer.UpdatedDate = inputCustomer.CreatedDate;

            var invalidCustomerException = new InvalidCustomerException();

            invalidCustomerException.AddData(
                key: nameof(Customer.CreatedDate),
                values: $"Date is not recent");

            var expectedCustomerValidationException =
                new CustomerValidationException(
                    message: "Customer validation error occurred, Please try again.",
                    innerException: invalidCustomerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Customer> createCustomerTask =
                this.customerService.CreateCustomerAsync(inputCustomer);

             CustomerValidationException actualCustomerDependencyValidationException =
             await Assert.ThrowsAsync<CustomerValidationException>(
                 createCustomerTask.AsTask);

            // then
            actualCustomerDependencyValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCustomerAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Customer randomCustomer = CreateRandomCustomer(dateTime);
            Customer alreadyExistsCustomer = randomCustomer;
            alreadyExistsCustomer.UpdatedBy = alreadyExistsCustomer.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsCustomerException =
                new AlreadyExistsCustomerException(
                   message: "Customer with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedCustomerValidationException =
                new CustomerDependencyValidationException(
                    message: "Customer dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsCustomerException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCustomerAsync(alreadyExistsCustomer))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Customer> createCustomerTask =
                this.customerService.CreateCustomerAsync(alreadyExistsCustomer);

             CustomerDependencyValidationException actualCustomerDependencyValidationException =
             await Assert.ThrowsAsync<CustomerDependencyValidationException>(
                 createCustomerTask.AsTask);

            // then
            actualCustomerDependencyValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAsync(alreadyExistsCustomer),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
