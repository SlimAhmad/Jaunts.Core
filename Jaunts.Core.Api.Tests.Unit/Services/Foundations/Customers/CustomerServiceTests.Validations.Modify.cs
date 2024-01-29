// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Force.DeepCloner;
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
        public async Task ShouldThrowValidationExceptionOnModifyWhenCustomerIsNullAndLogItAsync()
        {
            //given
            Customer invalidCustomer = null;
            var nullCustomerException = new NullCustomerException();

            var expectedCustomerValidationException =
                new CustomerValidationException(
                    message: "Customer validation error occurred, Please try again.",
                    nullCustomerException);

            //when
            ValueTask<Customer> modifyCustomerTask =
                this.customerService.ModifyCustomerAsync(invalidCustomer);

            CustomerValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<CustomerValidationException>(
                     modifyCustomerTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCustomerAsync(It.IsAny<Customer>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfCustomerIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidCustomer = new Customer
            {
                Address = invalidText,
                FirstName = invalidText,
                MiddleName = invalidText,
                LastName = invalidText,
            };

            var invalidCustomerException = new InvalidCustomerException();

            invalidCustomerException.AddData(
                key: nameof(Customer.Id),
                values: "Id is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.UserId),
                values: "Id is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.Address),
                values: "Text is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.FirstName),
                values: "Text is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.MiddleName),
                values: "Text is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.BirthDate),
                values: "Date is required");
 
            invalidCustomerException.AddData(
                key: nameof(Customer.CreatedDate),
                values: "Date is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(Customer.CreatedDate)}");

            invalidCustomerException.AddData(
                key: nameof(Customer.CreatedBy),
                values: "Id is required");

            invalidCustomerException.AddData(
                key: nameof(Customer.UpdatedBy),
                values: "Id is required");

            var expectedCustomerValidationException =
                new CustomerValidationException(invalidCustomerException);

            // when
            ValueTask<Customer> createCustomerTask =
                this.customerService.ModifyCustomerAsync(invalidCustomer);

            // then
            await Assert.ThrowsAsync<CustomerValidationException>(() =>
                createCustomerTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAsync(It.IsAny<Customer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            Customer randomCustomer = CreateRandomCustomer(dateTime);
            Customer inputCustomer = randomCustomer;

            var invalidCustomerException = new InvalidCustomerException(
                message: "Invalid Customer. Please fix the errors and try again.");

            invalidCustomerException.AddData(
               key: nameof(Customer.UpdatedDate),
               values: $"Date is the same as {nameof(inputCustomer.CreatedDate)}");

            var expectedCustomerValidationException =
                new CustomerValidationException(
                    message: "Customer validation error occurred, Please try again.",
                    innerException: invalidCustomerException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Customer> modifyCustomerTask =
                this.customerService.ModifyCustomerAsync(inputCustomer);

            CustomerValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<CustomerValidationException>(
                modifyCustomerTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCustomerAsync(It.IsAny<Customer>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Customer randomCustomer = CreateRandomModifyCustomer(dateTime);
            Customer inputCustomer = randomCustomer;
            inputCustomer.UpdatedBy = inputCustomer.CreatedBy;
            inputCustomer.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidCustomerException = new InvalidCustomerException(
                message: "Invalid Customer. Please fix the errors and try again.");

            invalidCustomerException.AddData(
                   key: nameof(Customer.UpdatedDate),
                   values: "Date is not recent");

            var expectedCustomerValidationException =
                new CustomerValidationException(
                    message: "Customer validation error occurred, Please try again.",
                    innerException: invalidCustomerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Customer> modifyCustomerTask =
                this.customerService.ModifyCustomerAsync(inputCustomer);

            CustomerValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<CustomerValidationException>(
                modifyCustomerTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCustomerAsync(It.IsAny<Customer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCustomerDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Customer randomCustomer = CreateRandomCustomer(dateTime);
            Customer nonExistentCustomer = randomCustomer;
            nonExistentCustomer.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Customer noCustomer = null;
            var notFoundCustomerException = new NotFoundCustomerException(nonExistentCustomer.Id);

            var expectedCustomerValidationException =
                new CustomerValidationException(
                    message: "Customer validation error occurred, Please try again.",
                    innerException: notFoundCustomerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerByIdAsync(nonExistentCustomer.Id))
                    .ReturnsAsync(noCustomer);

            // when
            ValueTask<Customer> modifyCustomerTask =
                this.customerService.ModifyCustomerAsync(nonExistentCustomer);

            CustomerValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<CustomerValidationException>(
                modifyCustomerTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(nonExistentCustomer.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCustomerAsync(It.IsAny<Customer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetNegativeRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            Customer randomCustomer = CreateRandomModifyCustomer(randomDateTimeOffset);
            Customer invalidCustomer = randomCustomer.DeepClone();
            Customer storageCustomer = invalidCustomer.DeepClone();
            storageCustomer.CreatedDate = storageCustomer.CreatedDate.AddMinutes(randomMinutes);
            storageCustomer.UpdatedDate = storageCustomer.UpdatedDate.AddMinutes(randomMinutes);
            Guid CustomerId = invalidCustomer.Id;
          

            var invalidCustomerException = new InvalidCustomerException(
               message: "Invalid Customer. Please fix the errors and try again.");

            invalidCustomerException.AddData(
                 key: nameof(Customer.CreatedDate),
                 values: $"Date is not the same as {nameof(Customer.CreatedDate)}");

            var expectedCustomerValidationException =
              new CustomerValidationException(
                  message: "Customer validation error occurred, Please try again.",
                  innerException: invalidCustomerException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerByIdAsync(CustomerId))
                    .ReturnsAsync(storageCustomer);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Customer> modifyCustomerTask =
                this.customerService.ModifyCustomerAsync(invalidCustomer);

            CustomerValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<CustomerValidationException>(
                modifyCustomerTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(invalidCustomer.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCustomerAsync(It.IsAny<Customer>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int randomPositiveMinutes = GetRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            Customer randomCustomer = CreateRandomModifyCustomer(randomDateTimeOffset);
            Customer invalidCustomer = randomCustomer.DeepClone();
            Customer storageCustomer = invalidCustomer.DeepClone();
            storageCustomer.UpdatedDate = storageCustomer.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid CustomerId = invalidCustomer.Id;
            invalidCustomer.CreatedBy = invalidCreatedBy;

            var invalidCustomerException = new InvalidCustomerException(
                message: "Invalid Customer. Please fix the errors and try again.");

            invalidCustomerException.AddData(
                key: nameof(Customer.CreatedBy),
                values: $"Id is not the same as {nameof(Customer.CreatedBy)}");

            var expectedCustomerValidationException =
              new CustomerValidationException(
                  message: "Customer validation error occurred, Please try again.",
                  innerException: invalidCustomerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerByIdAsync(CustomerId))
                    .ReturnsAsync(storageCustomer);

            // when
            ValueTask<Customer> modifyCustomerTask =
                this.customerService.ModifyCustomerAsync(invalidCustomer);

            CustomerValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<CustomerValidationException>(
                modifyCustomerTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(invalidCustomer.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCustomerAsync(It.IsAny<Customer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetCurrentDateTime();
            Customer randomCustomer = CreateRandomModifyCustomer(randomDate);
            Customer invalidCustomer = randomCustomer;
            invalidCustomer.UpdatedDate = randomDate;
            Customer storageCustomer = randomCustomer.DeepClone();
            Guid CustomerId = invalidCustomer.Id;

            var invalidCustomerException = new InvalidCustomerException(
               message: "Invalid Customer. Please fix the errors and try again.");

            invalidCustomerException.AddData(
               key: nameof(Customer.UpdatedDate),
               values: $"Date is the same as {nameof(invalidCustomer.UpdatedDate)}");

            var expectedCustomerValidationException =
              new CustomerValidationException(
                  message: "Customer validation error occurred, Please try again.",
                  innerException: invalidCustomerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerByIdAsync(CustomerId))
                    .ReturnsAsync(storageCustomer);

            // when
            ValueTask<Customer> modifyCustomerTask =
                this.customerService.ModifyCustomerAsync(invalidCustomer);

            CustomerValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<CustomerValidationException>(
                modifyCustomerTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(invalidCustomer.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCustomerAsync(It.IsAny<Customer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
