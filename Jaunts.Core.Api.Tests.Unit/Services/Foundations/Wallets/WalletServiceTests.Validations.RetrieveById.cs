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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
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

            //when
            ValueTask<Wallet> retrieveWalletByIdTask =
                this.walletService.RetrieveWalletByIdAsync(inputWalletId);

            WalletValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<WalletValidationException>(
                 retrieveWalletByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageWalletIsNullAndLogItAsync()
        {
            //given
            Guid randomWalletId = Guid.NewGuid();
            Guid someWalletId = randomWalletId;
            Wallet invalidStorageWallet = null;
            var notFoundWalletException = new NotFoundWalletException(
                message: $"Couldn't find Wallet with id: {someWalletId}.");

            var expectedWalletValidationException =
                new WalletValidationException(
                    message: "Wallet validation error occurred, Please try again.",
                    innerException: notFoundWalletException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageWallet);

            //when
            ValueTask<Wallet> retrieveWalletByIdTask =
                this.walletService.RetrieveWalletByIdAsync(someWalletId);

            WalletValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<WalletValidationException>(
                 retrieveWalletByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}