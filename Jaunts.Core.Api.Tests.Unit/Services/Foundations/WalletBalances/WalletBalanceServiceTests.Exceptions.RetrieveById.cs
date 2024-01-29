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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someWalletBalanceId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedWalletBalanceStorageException =
               new FailedWalletBalanceStorageException(
                   message: "Failed WalletBalance storage error occurred, Please contact support.",
                   sqlException);

            var expectedWalletBalanceDependencyException =
                new WalletBalanceDependencyException(
                    message: "WalletBalance dependency error occurred, contact support.",
                    expectedFailedWalletBalanceStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<WalletBalance> retrieveByIdWalletBalanceTask =
                this.walletBalanceService.RetrieveWalletBalanceByIdAsync(someWalletBalanceId);

            WalletBalanceDependencyException actualDependencyException =
             await Assert.ThrowsAsync<WalletBalanceDependencyException>(
                 retrieveByIdWalletBalanceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletBalanceDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedWalletBalanceDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someWalletBalanceId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedWalletBalanceStorageException =
              new FailedWalletBalanceStorageException(
                  message: "Failed WalletBalance storage error occurred, Please contact support.",
                  databaseUpdateException);

            var expectedWalletBalanceDependencyException =
                new WalletBalanceDependencyException(
                    message: "WalletBalance dependency error occurred, contact support.",
                    expectedFailedWalletBalanceStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<WalletBalance> retrieveByIdWalletBalanceTask =
                this.walletBalanceService.RetrieveWalletBalanceByIdAsync(someWalletBalanceId);

            WalletBalanceDependencyException actualDependencyException =
             await Assert.ThrowsAsync<WalletBalanceDependencyException>(
                 retrieveByIdWalletBalanceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletBalanceDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someWalletBalanceId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();


            var lockedWalletBalanceException = new LockedWalletBalanceException(
                message: "Locked WalletBalance record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedWalletBalanceDependencyException =
                new WalletBalanceDependencyException(
                    message: "WalletBalance dependency error occurred, contact support.",
                    innerException: lockedWalletBalanceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<WalletBalance> retrieveByIdWalletBalanceTask =
                this.walletBalanceService.RetrieveWalletBalanceByIdAsync(someWalletBalanceId);

            WalletBalanceDependencyException actualDependencyException =
             await Assert.ThrowsAsync<WalletBalanceDependencyException>(
                 retrieveByIdWalletBalanceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletBalanceDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someWalletBalanceId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedWalletBalanceServiceException =
                 new FailedWalletBalanceServiceException(
                     message: "Failed WalletBalance service error occurred, contact support.",
                     innerException: serviceException);

            var expectedWalletBalanceServiceException =
                new WalletBalanceServiceException(
                    message: "WalletBalance service error occurred, contact support.",
                    innerException: failedWalletBalanceServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<WalletBalance> retrieveByIdWalletBalanceTask =
                this.walletBalanceService.RetrieveWalletBalanceByIdAsync(someWalletBalanceId);

            WalletBalanceServiceException actualServiceException =
                 await Assert.ThrowsAsync<WalletBalanceServiceException>(
                     retrieveByIdWalletBalanceTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedWalletBalanceServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
