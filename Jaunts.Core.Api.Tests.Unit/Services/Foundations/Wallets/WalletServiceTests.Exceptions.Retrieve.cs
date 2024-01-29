// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Wallets
{
    public partial class WalletServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedFailedWalletStorageException =
              new FailedWalletStorageException(
                  message: "Failed Wallet storage error occurred, Please contact support.",
                  sqlException);

            var expectedWalletDependencyException =
                new WalletDependencyException(
                    message: "Wallet dependency error occurred, contact support.",
                    expectedFailedWalletStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllWallets())
                    .Throws(sqlException);

            // when
            Action retrieveAllWalletsAction = () =>
                this.walletService.RetrieveAllWallets();

            WalletDependencyException actualDependencyException =
              Assert.Throws<WalletDependencyException>(
                 retrieveAllWalletsAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllWallets(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedWalletDependencyException))),
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

            var failedWalletServiceException =
              new FailedWalletServiceException(
                  message: "Failed Wallet service error occurred, contact support.",
                  innerException: serviceException);

            var expectedWalletServiceException =
                new WalletServiceException(
                    message: "Wallet service error occurred, contact support.",
                    innerException: failedWalletServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllWallets())
                    .Throws(serviceException);

            // when
            Action retrieveAllWalletsAction = () =>
                this.walletService.RetrieveAllWallets();

            WalletServiceException actualServiceException =
              Assert.Throws<WalletServiceException>(
                 retrieveAllWalletsAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedWalletServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllWallets(),
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
