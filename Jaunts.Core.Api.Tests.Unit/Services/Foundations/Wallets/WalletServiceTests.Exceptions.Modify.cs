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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Wallet someWallet = CreateRandomWallet(randomDateTime);
            someWallet.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedFailedWalletStorageException =
              new FailedWalletStorageException(
                  message: "Failed Wallet storage error occurred, Please contact support.",
                  innerException: sqlException);

            var expectedWalletDependencyException =
                new WalletDependencyException(
                    message: "Wallet dependency error occurred, contact support.",
                    innerException: expectedFailedWalletStorageException);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Wallet> modifyWalletTask =
                this.walletService.ModifyWalletAsync(someWallet);

                WalletDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<WalletDependencyException>(
                     modifyWalletTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedWalletDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
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
            Wallet someWallet = CreateRandomWallet(randomDateTime);
            someWallet.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedWalletStorageException =
              new FailedWalletStorageException(
                  message: "Failed Wallet storage error occurred, Please contact support.",
                  databaseUpdateException);

            var expectedWalletDependencyException =
                new WalletDependencyException(
                    message: "Wallet dependency error occurred, contact support.",
                    expectedFailedWalletStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Wallet> modifyWalletTask =
                this.walletService.ModifyWalletAsync(someWallet);

            WalletDependencyException actualDependencyException =
                await Assert.ThrowsAsync<WalletDependencyException>(
                    modifyWalletTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
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
            Wallet randomWallet = CreateRandomWallet(randomDateTime);
            Wallet someWallet = randomWallet;
            someWallet.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedWalletException = new LockedWalletException(
                message: "Locked Wallet record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedWalletDependencyException =
                new WalletDependencyException(
                    message: "Wallet dependency error occurred, contact support.",
                    innerException: lockedWalletException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Wallet> modifyWalletTask =
                this.walletService.ModifyWalletAsync(someWallet);

            WalletDependencyException actualDependencyException =
             await Assert.ThrowsAsync<WalletDependencyException>(
                 modifyWalletTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
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
            Wallet randomWallet = CreateRandomWallet(randomDateTime);
            Wallet someWallet = randomWallet;
            someWallet.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var failedWalletServiceException =
             new FailedWalletServiceException(
                 message: "Failed Wallet service error occurred, contact support.",
                 innerException: serviceException);

            var expectedWalletServiceException =
                new WalletServiceException(
                    message: "Wallet service error occurred, contact support.",
                    innerException: failedWalletServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<Wallet> modifyWalletTask =
                this.walletService.ModifyWalletAsync(someWallet);

            WalletServiceException actualServiceException =
             await Assert.ThrowsAsync<WalletServiceException>(
                 modifyWalletTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedWalletServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
