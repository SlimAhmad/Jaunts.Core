// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Wallets
{
    public partial class WalletServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenWalletIsNullAndLogItAsync()
        {
            //given
            Wallet invalidWallet = null;
            var nullWalletException = new NullWalletException();

            var expectedWalletValidationException =
                new WalletValidationException(
                    message: "Wallet validation error occurred, Please try again.",
                    nullWalletException);

            //when
            ValueTask<Wallet> modifyWalletTask =
                this.walletService.ModifyWalletAsync(invalidWallet);

            WalletValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<WalletValidationException>(
                     modifyWalletTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletAsync(It.IsAny<Wallet>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfWalletIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidWallet = new Wallet
            {
                WalletName = invalidText,
                Description = invalidText,
            };

            var invalidWalletException = new InvalidWalletException();

            invalidWalletException.AddData(
                key: nameof(Wallet.Id),
                values: "Id is required");

            invalidWalletException.AddData(
                key: nameof(Wallet.WalletName),
                values: "Text is required");

            invalidWalletException.AddData(
                key: nameof(Wallet.Description),
                values: "Text is required");
 
            invalidWalletException.AddData(
                key: nameof(Wallet.CreatedDate),
                values: "Date is required");

            invalidWalletException.AddData(
                key: nameof(Wallet.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(Wallet.CreatedDate)}");

            invalidWalletException.AddData(
                key: nameof(Wallet.CreatedBy),
                values: "Id is required");

            invalidWalletException.AddData(
                key: nameof(Wallet.UpdatedBy),
                values: "Id is required");

            var expectedWalletValidationException =
                new WalletValidationException(invalidWalletException);

            // when
            ValueTask<Wallet> createWalletTask =
                this.walletService.ModifyWalletAsync(invalidWallet);

            // then
            await Assert.ThrowsAsync<WalletValidationException>(() =>
                createWalletTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertWalletAsync(It.IsAny<Wallet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            Wallet randomWallet = CreateRandomWallet(dateTime);
            Wallet inputWallet = randomWallet;

            var invalidWalletException = new InvalidWalletException(
                message: "Invalid Wallet. Please fix the errors and try again.");

            invalidWalletException.AddData(
               key: nameof(Wallet.UpdatedDate),
               values: $"Date is the same as {nameof(inputWallet.CreatedDate)}");

            var expectedWalletValidationException =
                new WalletValidationException(
                    message: "Wallet validation error occurred, Please try again.",
                    innerException: invalidWalletException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Wallet> modifyWalletTask =
                this.walletService.ModifyWalletAsync(inputWallet);

            WalletValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<WalletValidationException>(
                modifyWalletTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletAsync(It.IsAny<Wallet>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Wallet randomWallet = CreateRandomModifyWallet(dateTime);
            Wallet inputWallet = randomWallet;
            inputWallet.UpdatedBy = inputWallet.CreatedBy;
            inputWallet.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidWalletException = new InvalidWalletException(
                message: "Invalid Wallet. Please fix the errors and try again.");

            invalidWalletException.AddData(
                   key: nameof(Wallet.UpdatedDate),
                   values: "Date is not recent");

            var expectedWalletValidationException =
                new WalletValidationException(
                    message: "Wallet validation error occurred, Please try again.",
                    innerException: invalidWalletException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Wallet> modifyWalletTask =
                this.walletService.ModifyWalletAsync(inputWallet);

            WalletValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<WalletValidationException>(
                modifyWalletTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletAsync(It.IsAny<Wallet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfWalletDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Wallet randomWallet = CreateRandomWallet(dateTime);
            Wallet nonExistentWallet = randomWallet;
            nonExistentWallet.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Wallet noWallet = null;
            var notFoundWalletException = new NotFoundWalletException(nonExistentWallet.Id);

            var expectedWalletValidationException =
                new WalletValidationException(
                    message: "Wallet validation error occurred, Please try again.",
                    innerException: notFoundWalletException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletByIdAsync(nonExistentWallet.Id))
                    .ReturnsAsync(noWallet);

            // when
            ValueTask<Wallet> modifyWalletTask =
                this.walletService.ModifyWalletAsync(nonExistentWallet);

            WalletValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<WalletValidationException>(
                modifyWalletTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(nonExistentWallet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletAsync(It.IsAny<Wallet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetNegativeRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            Wallet randomWallet = CreateRandomModifyWallet(randomDateTimeOffset);
            Wallet invalidWallet = randomWallet.DeepClone();
            Wallet storageWallet = invalidWallet.DeepClone();
            storageWallet.CreatedDate = storageWallet.CreatedDate.AddMinutes(randomMinutes);
            storageWallet.UpdatedDate = storageWallet.UpdatedDate.AddMinutes(randomMinutes);
            Guid WalletId = invalidWallet.Id;
          

            var invalidWalletException = new InvalidWalletException(
               message: "Invalid Wallet. Please fix the errors and try again.");

            invalidWalletException.AddData(
                 key: nameof(Wallet.CreatedDate),
                 values: $"Date is not the same as {nameof(Wallet.CreatedDate)}");

            var expectedWalletValidationException =
              new WalletValidationException(
                  message: "Wallet validation error occurred, Please try again.",
                  innerException: invalidWalletException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletByIdAsync(WalletId))
                    .ReturnsAsync(storageWallet);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Wallet> modifyWalletTask =
                this.walletService.ModifyWalletAsync(invalidWallet);

            WalletValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<WalletValidationException>(
                modifyWalletTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(invalidWallet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletAsync(It.IsAny<Wallet>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int randomPositiveMinutes = GetRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            Wallet randomWallet = CreateRandomModifyWallet(randomDateTimeOffset);
            Wallet invalidWallet = randomWallet.DeepClone();
            Wallet storageWallet = invalidWallet.DeepClone();
            storageWallet.UpdatedDate = storageWallet.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid WalletId = invalidWallet.Id;
            invalidWallet.CreatedBy = invalidCreatedBy;

            var invalidWalletException = new InvalidWalletException(
                message: "Invalid Wallet. Please fix the errors and try again.");

            invalidWalletException.AddData(
                key: nameof(Wallet.CreatedBy),
                values: $"Id is not the same as {nameof(Wallet.CreatedBy)}");

            var expectedWalletValidationException =
              new WalletValidationException(
                  message: "Wallet validation error occurred, Please try again.",
                  innerException: invalidWalletException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletByIdAsync(WalletId))
                    .ReturnsAsync(storageWallet);

            // when
            ValueTask<Wallet> modifyWalletTask =
                this.walletService.ModifyWalletAsync(invalidWallet);

            WalletValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<WalletValidationException>(
                modifyWalletTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(invalidWallet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletAsync(It.IsAny<Wallet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetCurrentDateTime();
            Wallet randomWallet = CreateRandomModifyWallet(randomDate);
            Wallet invalidWallet = randomWallet;
            invalidWallet.UpdatedDate = randomDate;
            Wallet storageWallet = randomWallet.DeepClone();
            Guid WalletId = invalidWallet.Id;

            var invalidWalletException = new InvalidWalletException(
               message: "Invalid Wallet. Please fix the errors and try again.");

            invalidWalletException.AddData(
               key: nameof(Wallet.UpdatedDate),
               values: $"Date is the same as {nameof(invalidWallet.UpdatedDate)}");

            var expectedWalletValidationException =
              new WalletValidationException(
                  message: "Wallet validation error occurred, Please try again.",
                  innerException: invalidWalletException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletByIdAsync(WalletId))
                    .ReturnsAsync(storageWallet);

            // when
            ValueTask<Wallet> modifyWalletTask =
                this.walletService.ModifyWalletAsync(invalidWallet);

            WalletValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<WalletValidationException>(
                modifyWalletTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(invalidWallet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletAsync(It.IsAny<Wallet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
