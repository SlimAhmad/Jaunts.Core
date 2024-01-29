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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Transaction someTransaction = CreateRandomTransaction(randomDateTime);
            someTransaction.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

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
            ValueTask<Transaction> modifyTransactionTask =
                this.transactionService.ModifyTransactionAsync(someTransaction);

                TransactionDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<TransactionDependencyException>(
                     modifyTransactionTask.AsTask);

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
                broker.SelectTransactionByIdAsync(It.IsAny<Guid>()),
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
            Transaction someTransaction = CreateRandomTransaction(randomDateTime);
            someTransaction.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Transaction> modifyTransactionTask =
                this.transactionService.ModifyTransactionAsync(someTransaction);

            TransactionDependencyException actualDependencyException =
                await Assert.ThrowsAsync<TransactionDependencyException>(
                    modifyTransactionTask.AsTask);

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
                broker.SelectTransactionByIdAsync(It.IsAny<Guid>()),
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
            Transaction randomTransaction = CreateRandomTransaction(randomDateTime);
            Transaction someTransaction = randomTransaction;
            someTransaction.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTransactionException = new LockedTransactionException(
                message: "Locked Transaction record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedTransactionDependencyException =
                new TransactionDependencyException(
                    message: "Transaction dependency error occurred, contact support.",
                    innerException: lockedTransactionException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Transaction> modifyTransactionTask =
                this.transactionService.ModifyTransactionAsync(someTransaction);

            TransactionDependencyException actualDependencyException =
             await Assert.ThrowsAsync<TransactionDependencyException>(
                 modifyTransactionTask.AsTask);

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
                broker.SelectTransactionByIdAsync(It.IsAny<Guid>()),
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
            Transaction randomTransaction = CreateRandomTransaction(randomDateTime);
            Transaction someTransaction = randomTransaction;
            someTransaction.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Transaction> modifyTransactionTask =
                this.transactionService.ModifyTransactionAsync(someTransaction);

            TransactionServiceException actualServiceException =
             await Assert.ThrowsAsync<TransactionServiceException>(
                 modifyTransactionTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedTransactionServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
