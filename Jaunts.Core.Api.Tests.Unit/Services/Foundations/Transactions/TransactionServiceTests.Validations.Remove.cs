// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomTransactionId = default;
            Guid inputTransactionId = randomTransactionId;

            var invalidTransactionException = new InvalidTransactionException(
                message: "Invalid Transaction. Please fix the errors and try again.");

            invalidTransactionException.AddData(
                key: nameof(Transaction.Id),
                values: "Id is required");

            var expectedTransactionValidationException =
                new TransactionValidationException(
                    message: "Transaction validation error occurred, Please try again.",
                    innerException: invalidTransactionException);

            // when
            ValueTask<Transaction> actualTransactionTask =
                this.transactionService.RemoveTransactionByIdAsync(inputTransactionId);

            TransactionValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<TransactionValidationException>(
                 actualTransactionTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTransactionAsync(It.IsAny<Transaction>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageTransactionIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Transaction randomTransaction = CreateRandomTransaction(dateTime);
            Guid inputTransactionId = randomTransaction.Id;
            Transaction inputTransaction = randomTransaction;
            Transaction nullStorageTransaction = null;

            var notFoundTransactionException = new NotFoundTransactionException(inputTransactionId);

            var expectedTransactionValidationException =
                new TransactionValidationException(
                    message: "Transaction validation error occurred, Please try again.",
                    innerException: notFoundTransactionException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionByIdAsync(inputTransactionId))
                    .ReturnsAsync(nullStorageTransaction);

            // when
            ValueTask<Transaction> actualTransactionTask =
                this.transactionService.RemoveTransactionByIdAsync(inputTransactionId);

            TransactionValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<TransactionValidationException>(
                 actualTransactionTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(inputTransactionId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTransactionAsync(It.IsAny<Transaction>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
