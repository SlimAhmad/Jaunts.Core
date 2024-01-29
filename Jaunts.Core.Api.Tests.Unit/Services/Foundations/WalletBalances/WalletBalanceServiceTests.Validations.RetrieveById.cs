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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
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

            //when
            ValueTask<WalletBalance> retrieveWalletBalanceByIdTask =
                this.walletBalanceService.RetrieveWalletBalanceByIdAsync(inputWalletBalanceId);

            WalletBalanceValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<WalletBalanceValidationException>(
                 retrieveWalletBalanceByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageWalletBalanceIsNullAndLogItAsync()
        {
            //given
            Guid randomWalletBalanceId = Guid.NewGuid();
            Guid someWalletBalanceId = randomWalletBalanceId;
            WalletBalance invalidStorageWalletBalance = null;
            var notFoundWalletBalanceException = new NotFoundWalletBalanceException(
                message: $"Couldn't find WalletBalance with id: {someWalletBalanceId}.");

            var expectedWalletBalanceValidationException =
                new WalletBalanceValidationException(
                    message: "WalletBalance validation error occurred, Please try again.",
                    innerException: notFoundWalletBalanceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageWalletBalance);

            //when
            ValueTask<WalletBalance> retrieveWalletBalanceByIdTask =
                this.walletBalanceService.RetrieveWalletBalanceByIdAsync(someWalletBalanceId);

            WalletBalanceValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<WalletBalanceValidationException>(
                 retrieveWalletBalanceByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}