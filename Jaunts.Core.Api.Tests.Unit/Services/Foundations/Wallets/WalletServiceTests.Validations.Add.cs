// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Wallets
{
    public partial class WalletServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenWalletIsNullAndLogItAsync()
        {
            // given
            Wallet randomWallet = null;
            Wallet nullWallet = randomWallet;

            var nullWalletException = new NullWalletException(
                message: "The Wallet is null.");

            var expectedWalletValidationException =
                new WalletValidationException(
                    message: "Wallet validation error occurred, Please try again.",
                    innerException: nullWalletException);

            // when
            ValueTask<Wallet> createWalletTask =
                this.walletService.CreateWalletAsync(nullWallet);

             WalletValidationException actualWalletDependencyValidationException =
             await Assert.ThrowsAsync<WalletValidationException>(
                 createWalletTask.AsTask);

            // then
            actualWalletDependencyValidationException.Should().BeEquivalentTo(
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
        public async void ShouldThrowValidationExceptionOnCreateIfWalletStatusIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Wallet randomWallet = CreateRandomWallet(randomDateTime);
            Wallet invalidWallet = randomWallet;
            invalidWallet.UpdatedBy = randomWallet.CreatedBy;
            invalidWallet.Status = GetInvalidEnum<WalletStatus>();

            var invalidWalletException = new InvalidWalletException();

            invalidWalletException.AddData(
                key: nameof(Wallet.Status),
                values: "Value is not recognized");

            var expectedWalletValidationException = new WalletValidationException(
                message: "Wallet validation error occurred, Please try again.",
                innerException: invalidWalletException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<Wallet> createWalletTask =
                this.walletService.CreateWalletAsync(invalidWallet);

            WalletValidationException actualWalletDependencyValidationException =
            await Assert.ThrowsAsync<WalletValidationException>(
                createWalletTask.AsTask);

            // then
            actualWalletDependencyValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertWalletAsync(It.IsAny<Wallet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenWalletIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidWallet = new Wallet
            {
                Description = invalidText,
                WalletName = invalidText,
            };

            var invalidWalletException = new InvalidWalletException();

            invalidWalletException.AddData(
                key: nameof(Wallet.Id),
                values: "Id is required");

            invalidWalletException.AddData(
                key: nameof(Wallet.UserId),
                values: "Id is required");

            invalidWalletException.AddData(
                key: nameof(Wallet.WalletName),
                values: "Text is required");

            invalidWalletException.AddData(
                key: nameof(Wallet.Description),
                values: "Text is required");

            invalidWalletException.AddData(
                key: nameof(Wallet.CreatedBy),
                values: "Id is required");

            invalidWalletException.AddData(
                key: nameof(Wallet.UpdatedBy),
                values: "Id is required");

            invalidWalletException.AddData(
                key: nameof(Wallet.CreatedDate),
                values: "Date is required");

            invalidWalletException.AddData(
                key: nameof(Wallet.UpdatedDate),
                values: "Date is required");

            var expectedWalletValidationException =
                new WalletValidationException(
                    message: "Wallet validation error occurred, Please try again.",
                    innerException: invalidWalletException);

            // when
            ValueTask<Wallet> createWalletTask =
                this.walletService.CreateWalletAsync(invalidWallet);

             WalletValidationException actualWalletDependencyValidationException =
             await Assert.ThrowsAsync<WalletValidationException>(
                 createWalletTask.AsTask);

            // then
            actualWalletDependencyValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
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
            Wallet randomWallet = CreateRandomWallet(dateTime);
            Wallet inputWallet = randomWallet;
            inputWallet.UpdatedBy = Guid.NewGuid();

            var invalidWalletException = new InvalidWalletException();

            invalidWalletException.AddData(
                key: nameof(Wallet.UpdatedBy),
                values: $"Id is not the same as {nameof(Wallet.CreatedBy)}");

            var expectedWalletValidationException =
                new WalletValidationException(
                    message: "Wallet validation error occurred, Please try again.",
                    innerException: invalidWalletException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Wallet> createWalletTask =
                this.walletService.CreateWalletAsync(inputWallet);

             WalletValidationException actualWalletDependencyValidationException =
             await Assert.ThrowsAsync<WalletValidationException>(
                 createWalletTask.AsTask);

            // then
            actualWalletDependencyValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
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
            Wallet randomWallet = CreateRandomWallet(dateTime);
            Wallet inputWallet = randomWallet;
            inputWallet.UpdatedBy = randomWallet.CreatedBy;
            inputWallet.UpdatedDate = GetRandomDateTime();

            var invalidWalletException = new InvalidWalletException();

            invalidWalletException.AddData(
                key: nameof(Wallet.UpdatedDate),
                values: $"Date is not the same as {nameof(Wallet.CreatedDate)}");

            var expectedWalletValidationException =
                new WalletValidationException(
                    message: "Wallet validation error occurred, Please try again.",
                    innerException: invalidWalletException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Wallet> createWalletTask =
                this.walletService.CreateWalletAsync(inputWallet);

             WalletValidationException actualWalletDependencyValidationException =
             await Assert.ThrowsAsync<WalletValidationException>(
                 createWalletTask.AsTask);

            // then
            actualWalletDependencyValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
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
            Wallet randomWallet = CreateRandomWallet(dateTime);
            Wallet inputWallet = randomWallet;
            inputWallet.UpdatedBy = inputWallet.CreatedBy;
            inputWallet.CreatedDate = dateTime.AddMinutes(minutes);
            inputWallet.UpdatedDate = inputWallet.CreatedDate;

            var invalidWalletException = new InvalidWalletException();

            invalidWalletException.AddData(
                key: nameof(Wallet.CreatedDate),
                values: $"Date is not recent");

            var expectedWalletValidationException =
                new WalletValidationException(
                    message: "Wallet validation error occurred, Please try again.",
                    innerException: invalidWalletException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Wallet> createWalletTask =
                this.walletService.CreateWalletAsync(inputWallet);

             WalletValidationException actualWalletDependencyValidationException =
             await Assert.ThrowsAsync<WalletValidationException>(
                 createWalletTask.AsTask);

            // then
            actualWalletDependencyValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenWalletAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Wallet randomWallet = CreateRandomWallet(dateTime);
            Wallet alreadyExistsWallet = randomWallet;
            alreadyExistsWallet.UpdatedBy = alreadyExistsWallet.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsWalletException =
                new AlreadyExistsWalletException(
                   message: "Wallet with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedWalletValidationException =
                new WalletDependencyValidationException(
                    message: "Wallet dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsWalletException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertWalletAsync(alreadyExistsWallet))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Wallet> createWalletTask =
                this.walletService.CreateWalletAsync(alreadyExistsWallet);

             WalletDependencyValidationException actualWalletDependencyValidationException =
             await Assert.ThrowsAsync<WalletDependencyValidationException>(
                 createWalletTask.AsTask);

            // then
            actualWalletDependencyValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertWalletAsync(alreadyExistsWallet),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
