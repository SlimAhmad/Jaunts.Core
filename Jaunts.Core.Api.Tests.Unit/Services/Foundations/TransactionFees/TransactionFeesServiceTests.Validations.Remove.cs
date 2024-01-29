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
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
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

            // when
            ValueTask<TransactionFee> actualTransactionFeeTask =
                this.transactionFeeService.RemoveTransactionFeeByIdAsync(inputTransactionFeeId);

            TransactionFeeValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<TransactionFeeValidationException>(
                 actualTransactionFeeTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTransactionFeeAsync(It.IsAny<TransactionFee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageTransactionFeeIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(dateTime);
            Guid inputTransactionFeeId = randomTransactionFee.Id;
            TransactionFee inputTransactionFee = randomTransactionFee;
            TransactionFee nullStorageTransactionFee = null;

            var notFoundTransactionFeeException = new NotFoundTransactionFeeException(inputTransactionFeeId);

            var expectedTransactionFeeValidationException =
                new TransactionFeeValidationException(
                    message: "TransactionFee validation error occurred, Please try again.",
                    innerException: notFoundTransactionFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionFeeByIdAsync(inputTransactionFeeId))
                    .ReturnsAsync(nullStorageTransactionFee);

            // when
            ValueTask<TransactionFee> actualTransactionFeeTask =
                this.transactionFeeService.RemoveTransactionFeeByIdAsync(inputTransactionFeeId);

            TransactionFeeValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<TransactionFeeValidationException>(
                 actualTransactionFeeTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(inputTransactionFeeId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTransactionFeeAsync(It.IsAny<TransactionFee>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
