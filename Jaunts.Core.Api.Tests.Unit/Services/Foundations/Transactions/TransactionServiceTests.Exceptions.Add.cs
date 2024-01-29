// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Transaction someTransaction = CreateRandomTransaction(dateTime);
            someTransaction.UpdatedBy = someTransaction.CreatedBy;
            var sqlException = GetSqlException();

            var expectedFailedTransactionStorageException =
                new FailedTransactionStorageException(
                    message: "Failed Transaction storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedTransactionDependencyException =
                new TransactionDependencyException(
                    message: "Transaction dependency error occurred, contact support.",
                    innerException: expectedFailedTransactionStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Transaction> createTransactionTask =
                this.transactionService.CreateTransactionAsync(someTransaction);

            TransactionDependencyException actualDependencyException =
             await Assert.ThrowsAsync<TransactionDependencyException>(
                 createTransactionTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTransactionDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTransactionAsync(It.IsAny<Transaction>()),
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
            Transaction someTransaction = CreateRandomTransaction(dateTime);
            someTransaction.UpdatedBy = someTransaction.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedTransactionStorageException =
                new FailedTransactionStorageException(
                    message: "Failed Transaction storage error occurred, Please contact support.",
                    databaseUpdateException);

            var expectedTransactionDependencyException =
                new TransactionDependencyException(
                    message: "Transaction dependency error occurred, contact support.",
                    expectedFailedTransactionStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Transaction> createTransactionTask =
                this.transactionService.CreateTransactionAsync(someTransaction);

            TransactionDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<TransactionDependencyException>(
                     createTransactionTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTransactionAsync(It.IsAny<Transaction>()),
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
            Transaction someTransaction = CreateRandomTransaction(dateTime);
            someTransaction.UpdatedBy = someTransaction.CreatedBy;
            var serviceException = new Exception();

            var failedTransactionServiceException =
                new FailedTransactionServiceException(
                    message: "Failed Transaction service error occurred, contact support.",
                    innerException: serviceException);

            var expectedTransactionServiceException =
                new TransactionServiceException(
                    message: "Transaction service error occurred, contact support.",
                    innerException: failedTransactionServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<Transaction> createTransactionTask =
                 this.transactionService.CreateTransactionAsync(someTransaction);

            TransactionServiceException actualDependencyException =
                 await Assert.ThrowsAsync<TransactionServiceException>(
                     createTransactionTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTransactionAsync(It.IsAny<Transaction>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
