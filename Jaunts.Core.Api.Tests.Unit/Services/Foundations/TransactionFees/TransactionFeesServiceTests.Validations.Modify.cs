// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.TransactionFees
{
    public partial class TransactionFeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenTransactionFeeIsNullAndLogItAsync()
        {
            //given
            TransactionFee invalidTransactionFee = null;
            var nullTransactionFeeException = new NullTransactionFeeException();

            var expectedTransactionFeeValidationException =
                new TransactionFeeValidationException(
                    message: "TransactionFee validation error occurred, Please try again.",
                    nullTransactionFeeException);

            //when
            ValueTask<TransactionFee> modifyTransactionFeeTask =
                this.transactionFeeService.ModifyTransactionFeeAsync(invalidTransactionFee);

            TransactionFeeValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<TransactionFeeValidationException>(
                     modifyTransactionFeeTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionFeeAsync(It.IsAny<TransactionFee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfTransactionFeeIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidTransactionFee = new TransactionFee
            {
                Name = invalidText,
                Description = invalidText,
            };

            var invalidTransactionFeeException = new InvalidTransactionFeeException();

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.Id),
                values: "Id is required");

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.Name),
                values: "Text is required");

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.Description),
                values: "Text is required");
 
            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.CreatedDate),
                values: "Date is required");

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(TransactionFee.CreatedDate)}");

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.CreatedBy),
                values: "Id is required");

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.UpdatedBy),
                values: "Id is required");

            var expectedTransactionFeeValidationException =
                new TransactionFeeValidationException(invalidTransactionFeeException);

            // when
            ValueTask<TransactionFee> createTransactionFeeTask =
                this.transactionFeeService.ModifyTransactionFeeAsync(invalidTransactionFee);

            // then
            await Assert.ThrowsAsync<TransactionFeeValidationException>(() =>
                createTransactionFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTransactionFeeAsync(It.IsAny<TransactionFee>()),
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
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(dateTime);
            TransactionFee inputTransactionFee = randomTransactionFee;

            var invalidTransactionFeeException = new InvalidTransactionFeeException(
                message: "Invalid TransactionFee. Please fix the errors and try again.");

            invalidTransactionFeeException.AddData(
               key: nameof(TransactionFee.UpdatedDate),
               values: $"Date is the same as {nameof(inputTransactionFee.CreatedDate)}");

            var expectedTransactionFeeValidationException =
                new TransactionFeeValidationException(
                    message: "TransactionFee validation error occurred, Please try again.",
                    innerException: invalidTransactionFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<TransactionFee> modifyTransactionFeeTask =
                this.transactionFeeService.ModifyTransactionFeeAsync(inputTransactionFee);

            TransactionFeeValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<TransactionFeeValidationException>(
                modifyTransactionFeeTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionFeeAsync(It.IsAny<TransactionFee>()),
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
            TransactionFee randomTransactionFee = CreateRandomModifyTransactionFee(dateTime);
            TransactionFee inputTransactionFee = randomTransactionFee;
            inputTransactionFee.UpdatedBy = inputTransactionFee.CreatedBy;
            inputTransactionFee.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidTransactionFeeException = new InvalidTransactionFeeException(
                message: "Invalid TransactionFee. Please fix the errors and try again.");

            invalidTransactionFeeException.AddData(
                   key: nameof(TransactionFee.UpdatedDate),
                   values: "Date is not recent");

            var expectedTransactionFeeValidationException =
                new TransactionFeeValidationException(
                    message: "TransactionFee validation error occurred, Please try again.",
                    innerException: invalidTransactionFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<TransactionFee> modifyTransactionFeeTask =
                this.transactionFeeService.ModifyTransactionFeeAsync(inputTransactionFee);

            TransactionFeeValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<TransactionFeeValidationException>(
                modifyTransactionFeeTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionFeeAsync(It.IsAny<TransactionFee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTransactionFeeDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(dateTime);
            TransactionFee nonExistentTransactionFee = randomTransactionFee;
            nonExistentTransactionFee.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            TransactionFee noTransactionFee = null;
            var notFoundTransactionFeeException = new NotFoundTransactionFeeException(nonExistentTransactionFee.Id);

            var expectedTransactionFeeValidationException =
                new TransactionFeeValidationException(
                    message: "TransactionFee validation error occurred, Please try again.",
                    innerException: notFoundTransactionFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionFeeByIdAsync(nonExistentTransactionFee.Id))
                    .ReturnsAsync(noTransactionFee);

            // when
            ValueTask<TransactionFee> modifyTransactionFeeTask =
                this.transactionFeeService.ModifyTransactionFeeAsync(nonExistentTransactionFee);

            TransactionFeeValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<TransactionFeeValidationException>(
                modifyTransactionFeeTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(nonExistentTransactionFee.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionFeeAsync(It.IsAny<TransactionFee>()),
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
            TransactionFee randomTransactionFee = CreateRandomModifyTransactionFee(randomDateTimeOffset);
            TransactionFee invalidTransactionFee = randomTransactionFee.DeepClone();
            TransactionFee storageTransactionFee = invalidTransactionFee.DeepClone();
            storageTransactionFee.CreatedDate = storageTransactionFee.CreatedDate.AddMinutes(randomMinutes);
            storageTransactionFee.UpdatedDate = storageTransactionFee.UpdatedDate.AddMinutes(randomMinutes);
            Guid TransactionFeeId = invalidTransactionFee.Id;
          

            var invalidTransactionFeeException = new InvalidTransactionFeeException(
               message: "Invalid TransactionFee. Please fix the errors and try again.");

            invalidTransactionFeeException.AddData(
                 key: nameof(TransactionFee.CreatedDate),
                 values: $"Date is not the same as {nameof(TransactionFee.CreatedDate)}");

            var expectedTransactionFeeValidationException =
              new TransactionFeeValidationException(
                  message: "TransactionFee validation error occurred, Please try again.",
                  innerException: invalidTransactionFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionFeeByIdAsync(TransactionFeeId))
                    .ReturnsAsync(storageTransactionFee);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<TransactionFee> modifyTransactionFeeTask =
                this.transactionFeeService.ModifyTransactionFeeAsync(invalidTransactionFee);

            TransactionFeeValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<TransactionFeeValidationException>(
                modifyTransactionFeeTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(invalidTransactionFee.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionFeeAsync(It.IsAny<TransactionFee>()),
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
            TransactionFee randomTransactionFee = CreateRandomModifyTransactionFee(randomDateTimeOffset);
            TransactionFee invalidTransactionFee = randomTransactionFee.DeepClone();
            TransactionFee storageTransactionFee = invalidTransactionFee.DeepClone();
            storageTransactionFee.UpdatedDate = storageTransactionFee.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid TransactionFeeId = invalidTransactionFee.Id;
            invalidTransactionFee.CreatedBy = invalidCreatedBy;

            var invalidTransactionFeeException = new InvalidTransactionFeeException(
                message: "Invalid TransactionFee. Please fix the errors and try again.");

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.CreatedBy),
                values: $"Id is not the same as {nameof(TransactionFee.CreatedBy)}");

            var expectedTransactionFeeValidationException =
              new TransactionFeeValidationException(
                  message: "TransactionFee validation error occurred, Please try again.",
                  innerException: invalidTransactionFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionFeeByIdAsync(TransactionFeeId))
                    .ReturnsAsync(storageTransactionFee);

            // when
            ValueTask<TransactionFee> modifyTransactionFeeTask =
                this.transactionFeeService.ModifyTransactionFeeAsync(invalidTransactionFee);

            TransactionFeeValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<TransactionFeeValidationException>(
                modifyTransactionFeeTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(invalidTransactionFee.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionFeeAsync(It.IsAny<TransactionFee>()),
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
            TransactionFee randomTransactionFee = CreateRandomModifyTransactionFee(randomDate);
            TransactionFee invalidTransactionFee = randomTransactionFee;
            invalidTransactionFee.UpdatedDate = randomDate;
            TransactionFee storageTransactionFee = randomTransactionFee.DeepClone();
            Guid TransactionFeeId = invalidTransactionFee.Id;

            var invalidTransactionFeeException = new InvalidTransactionFeeException(
               message: "Invalid TransactionFee. Please fix the errors and try again.");

            invalidTransactionFeeException.AddData(
               key: nameof(TransactionFee.UpdatedDate),
               values: $"Date is the same as {nameof(invalidTransactionFee.UpdatedDate)}");

            var expectedTransactionFeeValidationException =
              new TransactionFeeValidationException(
                  message: "TransactionFee validation error occurred, Please try again.",
                  innerException: invalidTransactionFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionFeeByIdAsync(TransactionFeeId))
                    .ReturnsAsync(storageTransactionFee);

            // when
            ValueTask<TransactionFee> modifyTransactionFeeTask =
                this.transactionFeeService.ModifyTransactionFeeAsync(invalidTransactionFee);

            TransactionFeeValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<TransactionFeeValidationException>(
                modifyTransactionFeeTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(invalidTransactionFee.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionFeeAsync(It.IsAny<TransactionFee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
