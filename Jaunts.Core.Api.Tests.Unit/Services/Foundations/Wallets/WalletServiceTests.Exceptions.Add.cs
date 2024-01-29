// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Wallet someWallet = CreateRandomWallet(dateTime);
            someWallet.UpdatedBy = someWallet.CreatedBy;
            var sqlException = GetSqlException();

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
            ValueTask<Wallet> createWalletTask =
                this.walletService.CreateWalletAsync(someWallet);

            WalletDependencyException actualDependencyException =
             await Assert.ThrowsAsync<WalletDependencyException>(
                 createWalletTask.AsTask);

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
                broker.InsertWalletAsync(It.IsAny<Wallet>()),
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
            Wallet someWallet = CreateRandomWallet(dateTime);
            someWallet.UpdatedBy = someWallet.CreatedBy;
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
            ValueTask<Wallet> createWalletTask =
                this.walletService.CreateWalletAsync(someWallet);

            WalletDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<WalletDependencyException>(
                     createWalletTask.AsTask);

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
                broker.InsertWalletAsync(It.IsAny<Wallet>()),
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
            Wallet someWallet = CreateRandomWallet(dateTime);
            someWallet.UpdatedBy = someWallet.CreatedBy;
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
            ValueTask<Wallet> createWalletTask =
                 this.walletService.CreateWalletAsync(someWallet);

            WalletServiceException actualDependencyException =
                 await Assert.ThrowsAsync<WalletServiceException>(
                     createWalletTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedWalletServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedWalletServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertWalletAsync(It.IsAny<Wallet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
