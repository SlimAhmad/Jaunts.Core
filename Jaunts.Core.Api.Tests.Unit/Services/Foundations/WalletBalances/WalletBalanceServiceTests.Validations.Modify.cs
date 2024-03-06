// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.WalletBalances
{
    public partial class WalletBalanceServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenWalletBalanceIsNullAndLogItAsync()
        {
            //given
            WalletBalance invalidWalletBalance = null;
            var nullWalletBalanceException = new NullWalletBalanceException();

            var expectedWalletBalanceValidationException =
                new WalletBalanceValidationException(
                    message: "WalletBalance validation error occurred, Please try again.",
                    nullWalletBalanceException);

            //when
            ValueTask<WalletBalance> modifyWalletBalanceTask =
                this.walletBalanceService.ModifyWalletBalanceAsync(invalidWalletBalance);

            WalletBalanceValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<WalletBalanceValidationException>(
                     modifyWalletBalanceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletBalanceAsync(It.IsAny<WalletBalance>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfWalletBalanceIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidWalletBalance = new WalletBalance();

            var invalidWalletBalanceException = new InvalidWalletBalanceException();

            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.Id),
                values: "Id is required");

            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.WalletId),
                values: "Id is required");
 
            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.CreatedDate),
                values: "Date is required");

            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(WalletBalance.CreatedDate)}");

            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.CreatedBy),
                values: "Id is required");

            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.UpdatedBy),
                values: "Id is required");

            var expectedWalletBalanceValidationException =
                new WalletBalanceValidationException(invalidWalletBalanceException);

            // when
            ValueTask<WalletBalance> createWalletBalanceTask =
                this.walletBalanceService.ModifyWalletBalanceAsync(invalidWalletBalance);

            // then
            await Assert.ThrowsAsync<WalletBalanceValidationException>(() =>
                createWalletBalanceTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertWalletBalanceAsync(It.IsAny<WalletBalance>()),
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
            WalletBalance randomWalletBalance = CreateRandomWalletBalance(dateTime);
            WalletBalance inputWalletBalance = randomWalletBalance;

            var invalidWalletBalanceException = new InvalidWalletBalanceException(
                message: "Invalid WalletBalance. Please fix the errors and try again.");

            invalidWalletBalanceException.AddData(
               key: nameof(WalletBalance.UpdatedDate),
               values: $"Date is the same as {nameof(inputWalletBalance.CreatedDate)}");

            var expectedWalletBalanceValidationException =
                new WalletBalanceValidationException(
                    message: "WalletBalance validation error occurred, Please try again.",
                    innerException: invalidWalletBalanceException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<WalletBalance> modifyWalletBalanceTask =
                this.walletBalanceService.ModifyWalletBalanceAsync(inputWalletBalance);

            WalletBalanceValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<WalletBalanceValidationException>(
                modifyWalletBalanceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletBalanceAsync(It.IsAny<WalletBalance>()),
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
            WalletBalance randomWalletBalance = CreateRandomModifyWalletBalance(dateTime);
            WalletBalance inputWalletBalance = randomWalletBalance;
            inputWalletBalance.UpdatedBy = inputWalletBalance.CreatedBy;
            inputWalletBalance.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidWalletBalanceException = new InvalidWalletBalanceException(
                message: "Invalid WalletBalance. Please fix the errors and try again.");

            invalidWalletBalanceException.AddData(
                   key: nameof(WalletBalance.UpdatedDate),
                   values: "Date is not recent");

            var expectedWalletBalanceValidationException =
                new WalletBalanceValidationException(
                    message: "WalletBalance validation error occurred, Please try again.",
                    innerException: invalidWalletBalanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<WalletBalance> modifyWalletBalanceTask =
                this.walletBalanceService.ModifyWalletBalanceAsync(inputWalletBalance);

            WalletBalanceValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<WalletBalanceValidationException>(
                modifyWalletBalanceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletBalanceAsync(It.IsAny<WalletBalance>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfWalletBalanceDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            WalletBalance randomWalletBalance = CreateRandomWalletBalance(dateTime);
            WalletBalance nonExistentWalletBalance = randomWalletBalance;
            nonExistentWalletBalance.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            WalletBalance noWalletBalance = null;
            var notFoundWalletBalanceException = new NotFoundWalletBalanceException(nonExistentWalletBalance.Id);

            var expectedWalletBalanceValidationException =
                new WalletBalanceValidationException(
                    message: "WalletBalance validation error occurred, Please try again.",
                    innerException: notFoundWalletBalanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletBalanceByIdAsync(nonExistentWalletBalance.Id))
                    .ReturnsAsync(noWalletBalance);

            // when
            ValueTask<WalletBalance> modifyWalletBalanceTask =
                this.walletBalanceService.ModifyWalletBalanceAsync(nonExistentWalletBalance);

            WalletBalanceValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<WalletBalanceValidationException>(
                modifyWalletBalanceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(nonExistentWalletBalance.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletBalanceAsync(It.IsAny<WalletBalance>()),
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
            WalletBalance randomWalletBalance = CreateRandomModifyWalletBalance(randomDateTimeOffset);
            WalletBalance invalidWalletBalance = randomWalletBalance.DeepClone();
            WalletBalance storageWalletBalance = invalidWalletBalance.DeepClone();
            storageWalletBalance.CreatedDate = storageWalletBalance.CreatedDate.AddMinutes(randomMinutes);
            storageWalletBalance.UpdatedDate = storageWalletBalance.UpdatedDate.AddMinutes(randomMinutes);
            Guid WalletBalanceId = invalidWalletBalance.Id;
          

            var invalidWalletBalanceException = new InvalidWalletBalanceException(
               message: "Invalid WalletBalance. Please fix the errors and try again.");

            invalidWalletBalanceException.AddData(
                 key: nameof(WalletBalance.CreatedDate),
                 values: $"Date is not the same as {nameof(WalletBalance.CreatedDate)}");

            var expectedWalletBalanceValidationException =
              new WalletBalanceValidationException(
                  message: "WalletBalance validation error occurred, Please try again.",
                  innerException: invalidWalletBalanceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletBalanceByIdAsync(WalletBalanceId))
                    .ReturnsAsync(storageWalletBalance);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<WalletBalance> modifyWalletBalanceTask =
                this.walletBalanceService.ModifyWalletBalanceAsync(invalidWalletBalance);

            WalletBalanceValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<WalletBalanceValidationException>(
                modifyWalletBalanceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(invalidWalletBalance.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletBalanceAsync(It.IsAny<WalletBalance>()),
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
            WalletBalance randomWalletBalance = CreateRandomModifyWalletBalance(randomDateTimeOffset);
            WalletBalance invalidWalletBalance = randomWalletBalance.DeepClone();
            WalletBalance storageWalletBalance = invalidWalletBalance.DeepClone();
            storageWalletBalance.UpdatedDate = storageWalletBalance.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid WalletBalanceId = invalidWalletBalance.Id;
            invalidWalletBalance.CreatedBy = invalidCreatedBy;

            var invalidWalletBalanceException = new InvalidWalletBalanceException(
                message: "Invalid WalletBalance. Please fix the errors and try again.");

            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.CreatedBy),
                values: $"Id is not the same as {nameof(WalletBalance.CreatedBy)}");

            var expectedWalletBalanceValidationException =
              new WalletBalanceValidationException(
                  message: "WalletBalance validation error occurred, Please try again.",
                  innerException: invalidWalletBalanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletBalanceByIdAsync(WalletBalanceId))
                    .ReturnsAsync(storageWalletBalance);

            // when
            ValueTask<WalletBalance> modifyWalletBalanceTask =
                this.walletBalanceService.ModifyWalletBalanceAsync(invalidWalletBalance);

            WalletBalanceValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<WalletBalanceValidationException>(
                modifyWalletBalanceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(invalidWalletBalance.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletBalanceAsync(It.IsAny<WalletBalance>()),
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
            WalletBalance randomWalletBalance = CreateRandomModifyWalletBalance(randomDate);
            WalletBalance invalidWalletBalance = randomWalletBalance;
            invalidWalletBalance.UpdatedDate = randomDate;
            WalletBalance storageWalletBalance = randomWalletBalance.DeepClone();
            Guid WalletBalanceId = invalidWalletBalance.Id;

            var invalidWalletBalanceException = new InvalidWalletBalanceException(
               message: "Invalid WalletBalance. Please fix the errors and try again.");

            invalidWalletBalanceException.AddData(
               key: nameof(WalletBalance.UpdatedDate),
               values: $"Date is the same as {nameof(invalidWalletBalance.UpdatedDate)}");

            var expectedWalletBalanceValidationException =
              new WalletBalanceValidationException(
                  message: "WalletBalance validation error occurred, Please try again.",
                  innerException: invalidWalletBalanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletBalanceByIdAsync(WalletBalanceId))
                    .ReturnsAsync(storageWalletBalance);

            // when
            ValueTask<WalletBalance> modifyWalletBalanceTask =
                this.walletBalanceService.ModifyWalletBalanceAsync(invalidWalletBalance);

            WalletBalanceValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<WalletBalanceValidationException>(
                modifyWalletBalanceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(invalidWalletBalance.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletBalanceAsync(It.IsAny<WalletBalance>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
