// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
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
        public async void ShouldThrowValidationExceptionOnCreateWhenTransactionIsNullAndLogItAsync()
        {
            // given
            Transaction randomTransaction = null;
            Transaction nullTransaction = randomTransaction;

            var nullTransactionException = new NullTransactionException(
                message: "The Transaction is null.");

            var expectedTransactionValidationException =
                new TransactionValidationException(
                    message: "Transaction validation error occurred, Please try again.",
                    innerException: nullTransactionException);

            // when
            ValueTask<Transaction> createTransactionTask =
                this.transactionService.CreateTransactionAsync(nullTransaction);

             TransactionValidationException actualTransactionDependencyValidationException =
             await Assert.ThrowsAsync<TransactionValidationException>(
                 createTransactionTask.AsTask);

            // then
            actualTransactionDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateIfTransactionStatusIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Transaction randomTransaction = CreateRandomTransaction(randomDateTime);
            Transaction invalidTransaction = randomTransaction;
            invalidTransaction.UpdatedBy = randomTransaction.CreatedBy;
            invalidTransaction.TransactionStatus = GetInvalidEnum<TransactionStatus>();

            var invalidTransactionException = new InvalidTransactionException();

            invalidTransactionException.AddData(
                key: nameof(Transaction.TransactionStatus),
                values: "Value is not recognized");

            var expectedTransactionValidationException = new TransactionValidationException(
                message: "Transaction validation error occurred, Please try again.",
                innerException: invalidTransactionException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<Transaction> createTransactionTask =
                this.transactionService.CreateTransactionAsync(invalidTransaction);

            TransactionValidationException actualTransactionDependencyValidationException =
            await Assert.ThrowsAsync<TransactionValidationException>(
                createTransactionTask.AsTask);

            // then
            actualTransactionDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTransactionAsync(It.IsAny<Transaction>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateIfTransactionTypeIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Transaction randomTransaction = CreateRandomTransaction(randomDateTime);
            Transaction invalidTransaction = randomTransaction;
            invalidTransaction.UpdatedBy = randomTransaction.CreatedBy;
            invalidTransaction.TransactionType = GetInvalidEnum<TransactionType>();

            var invalidTransactionException = new InvalidTransactionException();

            invalidTransactionException.AddData(
                key: nameof(Transaction.TransactionType),
                values: "Value is not recognized");

            var expectedTransactionValidationException = new TransactionValidationException(
                message: "Transaction validation error occurred, Please try again.",
                innerException: invalidTransactionException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<Transaction> createTransactionTask =
                this.transactionService.CreateTransactionAsync(invalidTransaction);

            TransactionValidationException actualTransactionDependencyValidationException =
            await Assert.ThrowsAsync<TransactionValidationException>(
                createTransactionTask.AsTask);

            // then
            actualTransactionDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTransactionAsync(It.IsAny<Transaction>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenTransactionIsInvalidAndLogItAsync(
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
                key: nameof(Transaction.UserId),
                values: "Id is required");

            invalidTransactionException.AddData(
                key: nameof(Transaction.WalletBalanceId),
                values: "Id is required");

            invalidTransactionException.AddData(
                key: nameof(Transaction.Narration),
                values: "Text is required");

            invalidTransactionException.AddData(
                key: nameof(Transaction.CreatedBy),
                values: "Id is required");

            invalidTransactionException.AddData(
                key: nameof(Transaction.UpdatedBy),
                values: "Id is required");

            invalidTransactionException.AddData(
                key: nameof(Transaction.CreatedDate),
                values: "Date is required");

            invalidTransactionException.AddData(
                key: nameof(Transaction.UpdatedDate),
                values: "Date is required");

            var expectedTransactionValidationException =
                new TransactionValidationException(
                    message: "Transaction validation error occurred, Please try again.",
                    innerException: invalidTransactionException);

            // when
            ValueTask<Transaction> createTransactionTask =
                this.transactionService.CreateTransactionAsync(invalidTransaction);

             TransactionValidationException actualTransactionDependencyValidationException =
             await Assert.ThrowsAsync<TransactionValidationException>(
                 createTransactionTask.AsTask);

            // then
            actualTransactionDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(It.IsAny<Guid>()),
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
            Transaction randomTransaction = CreateRandomTransaction(dateTime);
            Transaction inputTransaction = randomTransaction;
            inputTransaction.UpdatedBy = Guid.NewGuid();

            var invalidTransactionException = new InvalidTransactionException();

            invalidTransactionException.AddData(
                key: nameof(Transaction.UpdatedBy),
                values: $"Id is not the same as {nameof(Transaction.CreatedBy)}");

            var expectedTransactionValidationException =
                new TransactionValidationException(
                    message: "Transaction validation error occurred, Please try again.",
                    innerException: invalidTransactionException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Transaction> createTransactionTask =
                this.transactionService.CreateTransactionAsync(inputTransaction);

             TransactionValidationException actualTransactionDependencyValidationException =
             await Assert.ThrowsAsync<TransactionValidationException>(
                 createTransactionTask.AsTask);

            // then
            actualTransactionDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(It.IsAny<Guid>()),
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
            Transaction randomTransaction = CreateRandomTransaction(dateTime);
            Transaction inputTransaction = randomTransaction;
            inputTransaction.UpdatedBy = randomTransaction.CreatedBy;
            inputTransaction.UpdatedDate = GetRandomDateTime();

            var invalidTransactionException = new InvalidTransactionException();

            invalidTransactionException.AddData(
                key: nameof(Transaction.UpdatedDate),
                values: $"Date is not the same as {nameof(Transaction.CreatedDate)}");

            var expectedTransactionValidationException =
                new TransactionValidationException(
                    message: "Transaction validation error occurred, Please try again.",
                    innerException: invalidTransactionException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Transaction> createTransactionTask =
                this.transactionService.CreateTransactionAsync(inputTransaction);

             TransactionValidationException actualTransactionDependencyValidationException =
             await Assert.ThrowsAsync<TransactionValidationException>(
                 createTransactionTask.AsTask);

            // then
            actualTransactionDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(It.IsAny<Guid>()),
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
            Transaction randomTransaction = CreateRandomTransaction(dateTime);
            Transaction inputTransaction = randomTransaction;
            inputTransaction.UpdatedBy = inputTransaction.CreatedBy;
            inputTransaction.CreatedDate = dateTime.AddMinutes(minutes);
            inputTransaction.UpdatedDate = inputTransaction.CreatedDate;

            var invalidTransactionException = new InvalidTransactionException();

            invalidTransactionException.AddData(
                key: nameof(Transaction.CreatedDate),
                values: $"Date is not recent");

            var expectedTransactionValidationException =
                new TransactionValidationException(
                    message: "Transaction validation error occurred, Please try again.",
                    innerException: invalidTransactionException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Transaction> createTransactionTask =
                this.transactionService.CreateTransactionAsync(inputTransaction);

             TransactionValidationException actualTransactionDependencyValidationException =
             await Assert.ThrowsAsync<TransactionValidationException>(
                 createTransactionTask.AsTask);

            // then
            actualTransactionDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenTransactionAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Transaction randomTransaction = CreateRandomTransaction(dateTime);
            Transaction alreadyExistsTransaction = randomTransaction;
            alreadyExistsTransaction.UpdatedBy = alreadyExistsTransaction.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsTransactionException =
                new AlreadyExistsTransactionException(
                   message: "Transaction with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedTransactionValidationException =
                new TransactionDependencyValidationException(
                    message: "Transaction dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsTransactionException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTransactionAsync(alreadyExistsTransaction))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Transaction> createTransactionTask =
                this.transactionService.CreateTransactionAsync(alreadyExistsTransaction);

             TransactionDependencyValidationException actualTransactionDependencyValidationException =
             await Assert.ThrowsAsync<TransactionDependencyValidationException>(
                 createTransactionTask.AsTask);

            // then
            actualTransactionDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTransactionAsync(alreadyExistsTransaction),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
