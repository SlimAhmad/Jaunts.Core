// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            WalletBalance someWalletBalance = CreateRandomWalletBalance(dateTime);
            someWalletBalance.UpdatedBy = someWalletBalance.CreatedBy;
            var sqlException = GetSqlException();

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
            ValueTask<WalletBalance> createWalletBalanceTask =
                this.walletBalanceService.CreateWalletBalanceAsync(someWalletBalance);

            WalletBalanceDependencyException actualDependencyException =
             await Assert.ThrowsAsync<WalletBalanceDependencyException>(
                 createWalletBalanceTask.AsTask);

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
                broker.InsertWalletBalanceAsync(It.IsAny<WalletBalance>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            WalletBalance someWalletBalance = CreateRandomWalletBalance(dateTime);
            someWalletBalance.UpdatedBy = someWalletBalance.CreatedBy;
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
            ValueTask<WalletBalance> createWalletBalanceTask =
                this.walletBalanceService.CreateWalletBalanceAsync(someWalletBalance);

            WalletBalanceDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<WalletBalanceDependencyException>(
                     createWalletBalanceTask.AsTask);

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
                broker.InsertWalletBalanceAsync(It.IsAny<WalletBalance>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            WalletBalance someWalletBalance = CreateRandomWalletBalance(dateTime);
            someWalletBalance.UpdatedBy = someWalletBalance.CreatedBy;
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
            ValueTask<WalletBalance> createWalletBalanceTask =
                 this.walletBalanceService.CreateWalletBalanceAsync(someWalletBalance);

            WalletBalanceServiceException actualDependencyException =
                 await Assert.ThrowsAsync<WalletBalanceServiceException>(
                     createWalletBalanceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletBalanceServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertWalletBalanceAsync(It.IsAny<WalletBalance>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
