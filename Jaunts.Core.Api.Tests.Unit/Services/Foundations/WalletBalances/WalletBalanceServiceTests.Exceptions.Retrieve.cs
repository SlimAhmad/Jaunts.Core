// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.WalletBalances
{
    public partial class WalletBalanceServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllWalletBalances())
                    .Throws(sqlException);

            // when
            Action retrieveAllWalletBalancesAction = () =>
                this.walletBalanceService.RetrieveAllWalletBalances();

            WalletBalanceDependencyException actualDependencyException =
              Assert.Throws<WalletBalanceDependencyException>(
                 retrieveAllWalletBalancesAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletBalanceDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllWalletBalances(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllWalletBalances())
                    .Throws(serviceException);

            // when
            Action retrieveAllWalletBalancesAction = () =>
                this.walletBalanceService.RetrieveAllWalletBalances();

            WalletBalanceServiceException actualServiceException =
              Assert.Throws<WalletBalanceServiceException>(
                 retrieveAllWalletBalancesAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedWalletBalanceServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletBalanceServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllWalletBalances(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
