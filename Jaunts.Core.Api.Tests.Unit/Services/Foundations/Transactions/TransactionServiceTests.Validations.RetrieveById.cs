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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
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

            //when
            ValueTask<Transaction> retrieveTransactionByIdTask =
                this.transactionService.RetrieveTransactionByIdAsync(inputTransactionId);

            TransactionValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<TransactionValidationException>(
                 retrieveTransactionByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageTransactionIsNullAndLogItAsync()
        {
            //given
            Guid randomTransactionId = Guid.NewGuid();
            Guid someTransactionId = randomTransactionId;
            Transaction invalidStorageTransaction = null;
            var notFoundTransactionException = new NotFoundTransactionException(
                message: $"Couldn't find Transaction with id: {someTransactionId}.");

            var expectedTransactionValidationException =
                new TransactionValidationException(
                    message: "Transaction validation error occurred, Please try again.",
                    innerException: notFoundTransactionException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageTransaction);

            //when
            ValueTask<Transaction> retrieveTransactionByIdTask =
                this.transactionService.RetrieveTransactionByIdAsync(someTransactionId);

            TransactionValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<TransactionValidationException>(
                 retrieveTransactionByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}