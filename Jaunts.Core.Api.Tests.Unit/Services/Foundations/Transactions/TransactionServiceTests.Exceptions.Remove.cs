// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Transactions
{
    public partial class TransactionServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTransactionId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedTransactionStorageException =
              new FailedTransactionStorageException(
                  message: "Failed Transaction storage error occurred, Please contact support.",
                  sqlException);

            var expectedTransactionDependencyException =
                new TransactionDependencyException(
                    message: "Transaction dependency error occurred, contact support.",
                    expectedFailedTransactionStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionByIdAsync(someTransactionId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Transaction> deleteTransactionTask =
                this.transactionService.RemoveTransactionByIdAsync(someTransactionId);

            TransactionDependencyException actualDependencyException =
                await Assert.ThrowsAsync<TransactionDependencyException>(
                    deleteTransactionTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(someTransactionId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTransactionDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTransactionId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedTransactionStorageException =
              new FailedTransactionStorageException(
                  message: "Failed Transaction storage error occurred, Please contact support.",
                  databaseUpdateException);

            var expectedTransactionDependencyException =
                new TransactionDependencyException(
                    message: "Transaction dependency error occurred, contact support.",
                    expectedFailedTransactionStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionByIdAsync(someTransactionId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Transaction> deleteTransactionTask =
                this.transactionService.RemoveTransactionByIdAsync(someTransactionId);

            TransactionDependencyException actualDependencyException =
                await Assert.ThrowsAsync<TransactionDependencyException>(
                    deleteTransactionTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(someTransactionId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTransactionId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTransactionException = new LockedTransactionException(
                message: "Locked Transaction record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedTransactionDependencyException =
                new TransactionDependencyException(
                    message: "Transaction dependency error occurred, contact support.",
                    innerException: lockedTransactionException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionByIdAsync(someTransactionId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Transaction> deleteTransactionTask =
                this.transactionService.RemoveTransactionByIdAsync(someTransactionId);

            TransactionDependencyException actualDependencyException =
                await Assert.ThrowsAsync<TransactionDependencyException>(
                    deleteTransactionTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(someTransactionId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTransactionId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedTransactionServiceException =
             new FailedTransactionServiceException(
                 message: "Failed Transaction service error occurred, contact support.",
                 innerException: serviceException);

            var expectedTransactionServiceException =
                new TransactionServiceException(
                    message: "Transaction service error occurred, contact support.",
                    innerException: failedTransactionServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionByIdAsync(someTransactionId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Transaction> deleteTransactionTask =
                this.transactionService.RemoveTransactionByIdAsync(someTransactionId);

            TransactionServiceException actualServiceException =
             await Assert.ThrowsAsync<TransactionServiceException>(
                 deleteTransactionTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedTransactionServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(someTransactionId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
