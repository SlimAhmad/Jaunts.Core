// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Transactions
{
    public partial class TransactionServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenTransactionIsNullAndLogItAsync()
        {
            //given
            Transaction invalidTransaction = null;
            var nullTransactionException = new NullTransactionException();

            var expectedTransactionValidationException =
                new TransactionValidationException(
                    message: "Transaction validation error occurred, Please try again.",
                    nullTransactionException);

            //when
            ValueTask<Transaction> modifyTransactionTask =
                this.transactionService.ModifyTransactionAsync(invalidTransaction);

            TransactionValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<TransactionValidationException>(
                     modifyTransactionTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionAsync(It.IsAny<Transaction>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfTransactionIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidTransaction = new Transaction
            {
                Narration = invalidText
            };

            var invalidTransactionException = new InvalidTransactionException();

            invalidTransactionException.AddData(
                key: nameof(Transaction.Id),
                values: "Id is required");

            invalidTransactionException.AddData(
                  key: nameof(Transaction.WalletBalanceId),
                  values: "Id is required");

            invalidTransactionException.AddData(
                key: nameof(Transaction.Narration),
                values: "Text is required");
 
            invalidTransactionException.AddData(
                key: nameof(Transaction.CreatedDate),
                values: "Date is required");

            invalidTransactionException.AddData(
                key: nameof(Transaction.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(Transaction.CreatedDate)}");

            invalidTransactionException.AddData(
                key: nameof(Transaction.CreatedBy),
                values: "Id is required");

            invalidTransactionException.AddData(
                key: nameof(Transaction.UpdatedBy),
                values: "Id is required");

            var expectedTransactionValidationException =
                new TransactionValidationException(invalidTransactionException);

            // when
            ValueTask<Transaction> createTransactionTask =
                this.transactionService.ModifyTransactionAsync(invalidTransaction);

            // then
            await Assert.ThrowsAsync<TransactionValidationException>(() =>
                createTransactionTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTransactionAsync(It.IsAny<Transaction>()),
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
            Transaction randomTransaction = CreateRandomTransaction(dateTime);
            Transaction inputTransaction = randomTransaction;

            var invalidTransactionException = new InvalidTransactionException(
                message: "Invalid Transaction. Please fix the errors and try again.");

            invalidTransactionException.AddData(
               key: nameof(Transaction.UpdatedDate),
               values: $"Date is the same as {nameof(inputTransaction.CreatedDate)}");

            var expectedTransactionValidationException =
                new TransactionValidationException(
                    message: "Transaction validation error occurred, Please try again.",
                    innerException: invalidTransactionException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Transaction> modifyTransactionTask =
                this.transactionService.ModifyTransactionAsync(inputTransaction);

            TransactionValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<TransactionValidationException>(
                modifyTransactionTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionAsync(It.IsAny<Transaction>()),
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
            Transaction randomTransaction = CreateRandomModifyTransaction(dateTime);
            Transaction inputTransaction = randomTransaction;
            inputTransaction.UpdatedBy = inputTransaction.CreatedBy;
            inputTransaction.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidTransactionException = new InvalidTransactionException(
                message: "Invalid Transaction. Please fix the errors and try again.");

            invalidTransactionException.AddData(
                   key: nameof(Transaction.UpdatedDate),
                   values: "Date is not recent");

            var expectedTransactionValidationException =
                new TransactionValidationException(
                    message: "Transaction validation error occurred, Please try again.",
                    innerException: invalidTransactionException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Transaction> modifyTransactionTask =
                this.transactionService.ModifyTransactionAsync(inputTransaction);

            TransactionValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<TransactionValidationException>(
                modifyTransactionTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionAsync(It.IsAny<Transaction>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTransactionDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Transaction randomTransaction = CreateRandomTransaction(dateTime);
            Transaction nonExistentTransaction = randomTransaction;
            nonExistentTransaction.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Transaction noTransaction = null;
            var notFoundTransactionException = new NotFoundTransactionException(nonExistentTransaction.Id);

            var expectedTransactionValidationException =
                new TransactionValidationException(
                    message: "Transaction validation error occurred, Please try again.",
                    innerException: notFoundTransactionException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionByIdAsync(nonExistentTransaction.Id))
                    .ReturnsAsync(noTransaction);

            // when
            ValueTask<Transaction> modifyTransactionTask =
                this.transactionService.ModifyTransactionAsync(nonExistentTransaction);

            TransactionValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<TransactionValidationException>(
                modifyTransactionTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(nonExistentTransaction.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionAsync(It.IsAny<Transaction>()),
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
            Transaction randomTransaction = CreateRandomModifyTransaction(randomDateTimeOffset);
            Transaction invalidTransaction = randomTransaction.DeepClone();
            Transaction storageTransaction = invalidTransaction.DeepClone();
            storageTransaction.CreatedDate = storageTransaction.CreatedDate.AddMinutes(randomMinutes);
            storageTransaction.UpdatedDate = storageTransaction.UpdatedDate.AddMinutes(randomMinutes);
            Guid TransactionId = invalidTransaction.Id;
          

            var invalidTransactionException = new InvalidTransactionException(
               message: "Invalid Transaction. Please fix the errors and try again.");

            invalidTransactionException.AddData(
                 key: nameof(Transaction.CreatedDate),
                 values: $"Date is not the same as {nameof(Transaction.CreatedDate)}");

            var expectedTransactionValidationException =
              new TransactionValidationException(
                  message: "Transaction validation error occurred, Please try again.",
                  innerException: invalidTransactionException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionByIdAsync(TransactionId))
                    .ReturnsAsync(storageTransaction);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Transaction> modifyTransactionTask =
                this.transactionService.ModifyTransactionAsync(invalidTransaction);

            TransactionValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<TransactionValidationException>(
                modifyTransactionTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(invalidTransaction.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionAsync(It.IsAny<Transaction>()),
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
            Transaction randomTransaction = CreateRandomModifyTransaction(randomDateTimeOffset);
            Transaction invalidTransaction = randomTransaction.DeepClone();
            Transaction storageTransaction = invalidTransaction.DeepClone();
            storageTransaction.UpdatedDate = storageTransaction.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid TransactionId = invalidTransaction.Id;
            invalidTransaction.CreatedBy = invalidCreatedBy;

            var invalidTransactionException = new InvalidTransactionException(
                message: "Invalid Transaction. Please fix the errors and try again.");

            invalidTransactionException.AddData(
                key: nameof(Transaction.CreatedBy),
                values: $"Id is not the same as {nameof(Transaction.CreatedBy)}");

            var expectedTransactionValidationException =
              new TransactionValidationException(
                  message: "Transaction validation error occurred, Please try again.",
                  innerException: invalidTransactionException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionByIdAsync(TransactionId))
                    .ReturnsAsync(storageTransaction);

            // when
            ValueTask<Transaction> modifyTransactionTask =
                this.transactionService.ModifyTransactionAsync(invalidTransaction);

            TransactionValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<TransactionValidationException>(
                modifyTransactionTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(invalidTransaction.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionAsync(It.IsAny<Transaction>()),
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
            Transaction randomTransaction = CreateRandomModifyTransaction(randomDate);
            Transaction invalidTransaction = randomTransaction;
            invalidTransaction.UpdatedDate = randomDate;
            Transaction storageTransaction = randomTransaction.DeepClone();
            Guid TransactionId = invalidTransaction.Id;

            var invalidTransactionException = new InvalidTransactionException(
               message: "Invalid Transaction. Please fix the errors and try again.");

            invalidTransactionException.AddData(
               key: nameof(Transaction.UpdatedDate),
               values: $"Date is the same as {nameof(invalidTransaction.UpdatedDate)}");

            var expectedTransactionValidationException =
              new TransactionValidationException(
                  message: "Transaction validation error occurred, Please try again.",
                  innerException: invalidTransactionException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionByIdAsync(TransactionId))
                    .ReturnsAsync(storageTransaction);

            // when
            ValueTask<Transaction> modifyTransactionTask =
                this.transactionService.ModifyTransactionAsync(invalidTransaction);

            TransactionValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<TransactionValidationException>(
                modifyTransactionTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(invalidTransaction.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionAsync(It.IsAny<Transaction>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
