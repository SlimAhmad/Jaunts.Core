// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.TransactionFees
{
    public partial class TransactionFeeServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomTransactionFeeId = default;
            Guid inputTransactionFeeId = randomTransactionFeeId;

            var invalidTransactionFeeException = new InvalidTransactionFeeException(
                message: "Invalid TransactionFee. Please fix the errors and try again.");

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.Id),
                values: "Id is required");

            var expectedTransactionFeeValidationException =
                new TransactionFeeValidationException(
                    message: "TransactionFee validation error occurred, Please try again.", 
                    innerException: invalidTransactionFeeException);

            //when
            ValueTask<TransactionFee> retrieveTransactionFeeByIdTask =
                this.transactionFeeService.RetrieveTransactionFeeByIdAsync(inputTransactionFeeId);

            TransactionFeeValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<TransactionFeeValidationException>(
                 retrieveTransactionFeeByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageTransactionFeeIsNullAndLogItAsync()
        {
            //given
            Guid randomTransactionFeeId = Guid.NewGuid();
            Guid someTransactionFeeId = randomTransactionFeeId;
            TransactionFee invalidStorageTransactionFee = null;
            var notFoundTransactionFeeException = new NotFoundTransactionFeeException(
                message: $"Couldn't find TransactionFee with id: {someTransactionFeeId}.");

            var expectedTransactionFeeValidationException =
                new TransactionFeeValidationException(
                    message: "TransactionFee validation error occurred, Please try again.",
                    innerException: notFoundTransactionFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageTransactionFee);

            //when
            ValueTask<TransactionFee> retrieveTransactionFeeByIdTask =
                this.transactionFeeService.RetrieveTransactionFeeByIdAsync(someTransactionFeeId);

            TransactionFeeValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<TransactionFeeValidationException>(
                 retrieveTransactionFeeByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}