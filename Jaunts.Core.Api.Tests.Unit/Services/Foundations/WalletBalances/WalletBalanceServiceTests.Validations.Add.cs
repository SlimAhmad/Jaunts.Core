// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.WalletBalances
{
    public partial class WalletBalanceServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenWalletBalanceIsNullAndLogItAsync()
        {
            // given
            WalletBalance randomWalletBalance = null;
            WalletBalance nullWalletBalance = randomWalletBalance;

            var nullWalletBalanceException = new NullWalletBalanceException(
                message: "The WalletBalance is null.");

            var expectedWalletBalanceValidationException =
                new WalletBalanceValidationException(
                    message: "WalletBalance validation error occurred, Please try again.",
                    innerException: nullWalletBalanceException);

            // when
            ValueTask<WalletBalance> createWalletBalanceTask =
                this.walletBalanceService.CreateWalletBalanceAsync(nullWalletBalance);

             WalletBalanceValidationException actualWalletBalanceDependencyValidationException =
             await Assert.ThrowsAsync<WalletBalanceValidationException>(
                 createWalletBalanceTask.AsTask);

            // then
            actualWalletBalanceDependencyValidationException.Should().BeEquivalentTo(
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
        public async void ShouldThrowValidationExceptionOnCreateWhenWalletBalanceIsInvalidAndLogItAsync()
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
                key: nameof(WalletBalance.CreatedBy),
                values: "Id is required");

            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.UpdatedBy),
                values: "Id is required");

            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.CreatedDate),
                values: "Date is required");

            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.UpdatedDate),
                values: "Date is required");

            var expectedWalletBalanceValidationException =
                new WalletBalanceValidationException(
                    message: "WalletBalance validation error occurred, Please try again.",
                    innerException: invalidWalletBalanceException);

            // when
            ValueTask<WalletBalance> createWalletBalanceTask =
                this.walletBalanceService.CreateWalletBalanceAsync(invalidWalletBalance);

             WalletBalanceValidationException actualWalletBalanceDependencyValidationException =
             await Assert.ThrowsAsync<WalletBalanceValidationException>(
                 createWalletBalanceTask.AsTask);

            // then
            actualWalletBalanceDependencyValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
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
            WalletBalance randomWalletBalance = CreateRandomWalletBalance(dateTime);
            WalletBalance inputWalletBalance = randomWalletBalance;
            inputWalletBalance.UpdatedBy = Guid.NewGuid();

            var invalidWalletBalanceException = new InvalidWalletBalanceException();

            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.UpdatedBy),
                values: $"Id is not the same as {nameof(WalletBalance.CreatedBy)}");

            var expectedWalletBalanceValidationException =
                new WalletBalanceValidationException(
                    message: "WalletBalance validation error occurred, Please try again.",
                    innerException: invalidWalletBalanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<WalletBalance> createWalletBalanceTask =
                this.walletBalanceService.CreateWalletBalanceAsync(inputWalletBalance);

             WalletBalanceValidationException actualWalletBalanceDependencyValidationException =
             await Assert.ThrowsAsync<WalletBalanceValidationException>(
                 createWalletBalanceTask.AsTask);

            // then
            actualWalletBalanceDependencyValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
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
            WalletBalance randomWalletBalance = CreateRandomWalletBalance(dateTime);
            WalletBalance inputWalletBalance = randomWalletBalance;
            inputWalletBalance.UpdatedBy = randomWalletBalance.CreatedBy;
            inputWalletBalance.UpdatedDate = GetRandomDateTime();

            var invalidWalletBalanceException = new InvalidWalletBalanceException();

            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.UpdatedDate),
                values: $"Date is not the same as {nameof(WalletBalance.CreatedDate)}");

            var expectedWalletBalanceValidationException =
                new WalletBalanceValidationException(
                    message: "WalletBalance validation error occurred, Please try again.",
                    innerException: invalidWalletBalanceException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<WalletBalance> createWalletBalanceTask =
                this.walletBalanceService.CreateWalletBalanceAsync(inputWalletBalance);

             WalletBalanceValidationException actualWalletBalanceDependencyValidationException =
             await Assert.ThrowsAsync<WalletBalanceValidationException>(
                 createWalletBalanceTask.AsTask);

            // then
            actualWalletBalanceDependencyValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
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
            WalletBalance randomWalletBalance = CreateRandomWalletBalance(dateTime);
            WalletBalance inputWalletBalance = randomWalletBalance;
            inputWalletBalance.UpdatedBy = inputWalletBalance.CreatedBy;
            inputWalletBalance.CreatedDate = dateTime.AddMinutes(minutes);
            inputWalletBalance.UpdatedDate = inputWalletBalance.CreatedDate;

            var invalidWalletBalanceException = new InvalidWalletBalanceException();

            invalidWalletBalanceException.AddData(
                key: nameof(WalletBalance.CreatedDate),
                values: $"Date is not recent");

            var expectedWalletBalanceValidationException =
                new WalletBalanceValidationException(
                    message: "WalletBalance validation error occurred, Please try again.",
                    innerException: invalidWalletBalanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<WalletBalance> createWalletBalanceTask =
                this.walletBalanceService.CreateWalletBalanceAsync(inputWalletBalance);

             WalletBalanceValidationException actualWalletBalanceDependencyValidationException =
             await Assert.ThrowsAsync<WalletBalanceValidationException>(
                 createWalletBalanceTask.AsTask);

            // then
            actualWalletBalanceDependencyValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenWalletBalanceAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            WalletBalance randomWalletBalance = CreateRandomWalletBalance(dateTime);
            WalletBalance alreadyExistsWalletBalance = randomWalletBalance;
            alreadyExistsWalletBalance.UpdatedBy = alreadyExistsWalletBalance.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsWalletBalanceException =
                new AlreadyExistsWalletBalanceException(
                   message: "WalletBalance with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedWalletBalanceValidationException =
                new WalletBalanceDependencyValidationException(
                    message: "WalletBalance dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsWalletBalanceException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertWalletBalanceAsync(alreadyExistsWalletBalance))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<WalletBalance> createWalletBalanceTask =
                this.walletBalanceService.CreateWalletBalanceAsync(alreadyExistsWalletBalance);

             WalletBalanceDependencyValidationException actualWalletBalanceDependencyValidationException =
             await Assert.ThrowsAsync<WalletBalanceDependencyValidationException>(
                 createWalletBalanceTask.AsTask);

            // then
            actualWalletBalanceDependencyValidationException.Should().BeEquivalentTo(
                expectedWalletBalanceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedWalletBalanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertWalletBalanceAsync(alreadyExistsWalletBalance),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
