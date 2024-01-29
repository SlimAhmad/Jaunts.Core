// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.TransactionFees
{
    public partial class TransactionFeeServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedFailedTransactionFeeStorageException =
              new FailedTransactionFeeStorageException(
                  message: "Failed TransactionFee storage error occurred, Please contact support.",
                  sqlException);

            var expectedTransactionFeeDependencyException =
                new TransactionFeeDependencyException(
                    message: "TransactionFee dependency error occurred, contact support.",
                    expectedFailedTransactionFeeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTransactionFees())
                    .Throws(sqlException);

            // when
            Action retrieveAllTransactionFeesAction = () =>
                this.transactionFeeService.RetrieveAllTransactionFees();

            TransactionFeeDependencyException actualDependencyException =
              Assert.Throws<TransactionFeeDependencyException>(
                 retrieveAllTransactionFeesAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionFeeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTransactionFees(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTransactionFeeDependencyException))),
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

            var failedTransactionFeeServiceException =
              new FailedTransactionFeeServiceException(
                  message: "Failed TransactionFee service error occurred, contact support.",
                  innerException: serviceException);

            var expectedTransactionFeeServiceException =
                new TransactionFeeServiceException(
                    message: "TransactionFee service error occurred, contact support.",
                    innerException: failedTransactionFeeServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTransactionFees())
                    .Throws(serviceException);

            // when
            Action retrieveAllTransactionFeesAction = () =>
                this.transactionFeeService.RetrieveAllTransactionFees();

            TransactionFeeServiceException actualServiceException =
              Assert.Throws<TransactionFeeServiceException>(
                 retrieveAllTransactionFeesAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedTransactionFeeServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTransactionFees(),
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
