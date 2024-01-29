// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.WalletBalances
{
    public partial class WalletBalanceServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomWalletBalanceId = default;
            Guid inputWalletBalanceId = randomWalletBalanceId;

            var invalidWalletBalanceException = new InvalidWalletBalanceException(
                message: "Invalid WalletBalance. Please fix the errors and try again.");

            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.Id),
                values: "Id is required");

            var expectedWalletBalanceValidationException =
                new WalletBalanceValidationException(
                    message: "WalletBalance validation error occurred, Please try again.",
                    innerException: invalidWalletBalanceException);

            // when
            ValueTask<WalletBalance> actualWalletBalanceTask =
                this.walletBalanceService.RemoveWalletBalanceByIdAsync(inputWalletBalanceId);

            WalletBalanceValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<WalletBalanceValidationException>(
                 actualWalletBalanceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteWalletBalanceAsync(It.IsAny<WalletBalance>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageWalletBalanceIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            WalletBalance randomWalletBalance = CreateRandomWalletBalance(dateTime);
            Guid inputWalletBalanceId = randomWalletBalance.Id;
            WalletBalance inputWalletBalance = randomWalletBalance;
            WalletBalance nullStorageWalletBalance = null;

            var notFoundWalletBalanceException = new NotFoundWalletBalanceException(inputWalletBalanceId);

            var expectedWalletBalanceValidationException =
                new WalletBalanceValidationException(
                    message: "WalletBalance validation error occurred, Please try again.",
                    innerException: notFoundWalletBalanceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletBalanceByIdAsync(inputWalletBalanceId))
                    .ReturnsAsync(nullStorageWalletBalance);

            // when
            ValueTask<WalletBalance> actualWalletBalanceTask =
                this.walletBalanceService.RemoveWalletBalanceByIdAsync(inputWalletBalanceId);

            WalletBalanceValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<WalletBalanceValidationException>(
                 actualWalletBalanceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(inputWalletBalanceId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteWalletBalanceAsync(It.IsAny<WalletBalance>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
