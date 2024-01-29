// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Wallets
{
    public partial class WalletServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomWalletId = default;
            Guid inputWalletId = randomWalletId;

            var invalidWalletException = new InvalidWalletException(
                message: "Invalid Wallet. Please fix the errors and try again.");

            invalidWalletException.AddData(
                key: nameof(Wallet.Id),
                values: "Id is required");

            var expectedWalletValidationException =
                new WalletValidationException(
                    message: "Wallet validation error occurred, Please try again.",
                    innerException: invalidWalletException);

            // when
            ValueTask<Wallet> actualWalletTask =
                this.walletService.RemoveWalletByIdAsync(inputWalletId);

            WalletValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<WalletValidationException>(
                 actualWalletTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteWalletAsync(It.IsAny<Wallet>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageWalletIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Wallet randomWallet = CreateRandomWallet(dateTime);
            Guid inputWalletId = randomWallet.Id;
            Wallet inputWallet = randomWallet;
            Wallet nullStorageWallet = null;

            var notFoundWalletException = new NotFoundWalletException(inputWalletId);

            var expectedWalletValidationException =
                new WalletValidationException(
                    message: "Wallet validation error occurred, Please try again.",
                    innerException: notFoundWalletException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletByIdAsync(inputWalletId))
                    .ReturnsAsync(nullStorageWallet);

            // when
            ValueTask<Wallet> actualWalletTask =
                this.walletService.RemoveWalletByIdAsync(inputWalletId);

            WalletValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<WalletValidationException>(
                 actualWalletTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(inputWalletId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteWalletAsync(It.IsAny<Wallet>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
