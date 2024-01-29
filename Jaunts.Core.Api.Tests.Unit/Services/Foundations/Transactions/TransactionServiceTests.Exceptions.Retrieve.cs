// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Transactions
{
    public partial class TransactionServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllTransactions())
                    .Throws(sqlException);

            // when
            Action retrieveAllTransactionsAction = () =>
                this.transactionService.RetrieveAllTransactions();

            TransactionDependencyException actualDependencyException =
              Assert.Throws<TransactionDependencyException>(
                 retrieveAllTransactionsAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTransactions(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllTransactions())
                    .Throws(serviceException);

            // when
            Action retrieveAllTransactionsAction = () =>
                this.transactionService.RetrieveAllTransactions();

            TransactionServiceException actualServiceException =
              Assert.Throws<TransactionServiceException>(
                 retrieveAllTransactionsAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedTransactionServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTransactions(),
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
