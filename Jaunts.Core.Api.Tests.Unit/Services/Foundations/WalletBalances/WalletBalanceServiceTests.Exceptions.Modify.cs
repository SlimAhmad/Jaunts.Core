// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.WalletBalances
{
    public partial class WalletBalanceServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            WalletBalance someWalletBalance = CreateRandomWalletBalance(randomDateTime);
            someWalletBalance.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedFailedWalletBalanceStorageException =
              new FailedWalletBalanceStorageException(
                  message: "Failed WalletBalance storage error occurred, Please contact support.",
                  innerException: sqlException);

            var expectedWalletBalanceDependencyException =
                new WalletBalanceDependencyException(
                    message: "WalletBalance dependency error occurred, contact support.",
                    innerException: expectedFailedWalletBalanceStorageException);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<WalletBalance> modifyWalletBalanceTask =
                this.walletBalanceService.ModifyWalletBalanceAsync(someWalletBalance);

                WalletBalanceDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<WalletBalanceDependencyException>(
                     modifyWalletBalanceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletBalanceDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedWalletBalanceDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            WalletBalance someWalletBalance = CreateRandomWalletBalance(randomDateTime);
            someWalletBalance.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedWalletBalanceStorageException =
              new FailedWalletBalanceStorageException(
                  message: "Failed WalletBalance storage error occurred, Please contact support.",
                  databaseUpdateException);

            var expectedWalletBalanceDependencyException =
                new WalletBalanceDependencyException(
                    message: "WalletBalance dependency error occurred, contact support.",
                    expectedFailedWalletBalanceStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<WalletBalance> modifyWalletBalanceTask =
                this.walletBalanceService.ModifyWalletBalanceAsync(someWalletBalance);

            WalletBalanceDependencyException actualDependencyException =
                await Assert.ThrowsAsync<WalletBalanceDependencyException>(
                    modifyWalletBalanceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletBalanceDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            WalletBalance randomWalletBalance = CreateRandomWalletBalance(randomDateTime);
            WalletBalance someWalletBalance = randomWalletBalance;
            someWalletBalance.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedWalletBalanceException = new LockedWalletBalanceException(
                message: "Locked WalletBalance record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedWalletBalanceDependencyException =
                new WalletBalanceDependencyException(
                    message: "WalletBalance dependency error occurred, contact support.",
                    innerException: lockedWalletBalanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<WalletBalance> modifyWalletBalanceTask =
                this.walletBalanceService.ModifyWalletBalanceAsync(someWalletBalance);

            WalletBalanceDependencyException actualDependencyException =
             await Assert.ThrowsAsync<WalletBalanceDependencyException>(
                 modifyWalletBalanceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletBalanceDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            WalletBalance randomWalletBalance = CreateRandomWalletBalance(randomDateTime);
            WalletBalance someWalletBalance = randomWalletBalance;
            someWalletBalance.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var failedWalletBalanceServiceException =
             new FailedWalletBalanceServiceException(
                 message: "Failed WalletBalance service error occurred, contact support.",
                 innerException: serviceException);

            var expectedWalletBalanceServiceException =
                new WalletBalanceServiceException(
                    message: "WalletBalance service error occurred, contact support.",
                    innerException: failedWalletBalanceServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<WalletBalance> modifyWalletBalanceTask =
                this.walletBalanceService.ModifyWalletBalanceAsync(someWalletBalance);

            WalletBalanceServiceException actualServiceException =
             await Assert.ThrowsAsync<WalletBalanceServiceException>(
                 modifyWalletBalanceTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedWalletBalanceServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
