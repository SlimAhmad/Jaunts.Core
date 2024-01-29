// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Wallets
{
    public partial class WalletServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someWalletId = Guid.NewGuid();
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
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Wallet> retrieveByIdWalletTask =
                this.walletService.RetrieveWalletByIdAsync(someWalletId);

            WalletDependencyException actualDependencyException =
             await Assert.ThrowsAsync<WalletDependencyException>(
                 retrieveByIdWalletTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someWalletId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedWalletStorageException =
              new FailedWalletStorageException(
                  message: "Failed Wallet storage error occurred, Please contact support.",
                  databaseUpdateException);

            var expectedWalletDependencyException =
                new WalletDependencyException(
                    message: "Wallet dependency error occurred, contact support.",
                    expectedFailedWalletStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Wallet> retrieveByIdWalletTask =
                this.walletService.RetrieveWalletByIdAsync(someWalletId);

            WalletDependencyException actualDependencyException =
             await Assert.ThrowsAsync<WalletDependencyException>(
                 retrieveByIdWalletTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletDependencyException))),
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
            Guid someWalletId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();


            var lockedWalletException = new LockedWalletException(
                message: "Locked Wallet record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedWalletDependencyException =
                new WalletDependencyException(
                    message: "Wallet dependency error occurred, contact support.",
                    innerException: lockedWalletException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Wallet> retrieveByIdWalletTask =
                this.walletService.RetrieveWalletByIdAsync(someWalletId);

            WalletDependencyException actualDependencyException =
             await Assert.ThrowsAsync<WalletDependencyException>(
                 retrieveByIdWalletTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someWalletId = Guid.NewGuid();
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
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Wallet> retrieveByIdWalletTask =
                this.walletService.RetrieveWalletByIdAsync(someWalletId);

            WalletServiceException actualServiceException =
                 await Assert.ThrowsAsync<WalletServiceException>(
                     retrieveByIdWalletTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedWalletServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
