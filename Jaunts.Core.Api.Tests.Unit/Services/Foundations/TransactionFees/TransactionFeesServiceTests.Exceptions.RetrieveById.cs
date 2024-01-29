// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.TransactionFees
{
    public partial class TransactionFeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTransactionFeeId = Guid.NewGuid();
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
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<TransactionFee> retrieveByIdTransactionFeeTask =
                this.transactionFeeService.RetrieveTransactionFeeByIdAsync(someTransactionFeeId);

            TransactionFeeDependencyException actualDependencyException =
             await Assert.ThrowsAsync<TransactionFeeDependencyException>(
                 retrieveByIdTransactionFeeTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionFeeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTransactionFeeId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedTransactionFeeStorageException =
              new FailedTransactionFeeStorageException(
                  message: "Failed TransactionFee storage error occurred, Please contact support.",
                  databaseUpdateException);

            var expectedTransactionFeeDependencyException =
                new TransactionFeeDependencyException(
                    message: "TransactionFee dependency error occurred, contact support.",
                    expectedFailedTransactionFeeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<TransactionFee> retrieveByIdTransactionFeeTask =
                this.transactionFeeService.RetrieveTransactionFeeByIdAsync(someTransactionFeeId);

            TransactionFeeDependencyException actualDependencyException =
             await Assert.ThrowsAsync<TransactionFeeDependencyException>(
                 retrieveByIdTransactionFeeTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionFeeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeDependencyException))),
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
            Guid someTransactionFeeId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();


            var lockedTransactionFeeException = new LockedTransactionFeeException(
                message: "Locked TransactionFee record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedTransactionFeeDependencyException =
                new TransactionFeeDependencyException(
                    message: "TransactionFee dependency error occurred, contact support.",
                    innerException: lockedTransactionFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<TransactionFee> retrieveByIdTransactionFeeTask =
                this.transactionFeeService.RetrieveTransactionFeeByIdAsync(someTransactionFeeId);

            TransactionFeeDependencyException actualDependencyException =
             await Assert.ThrowsAsync<TransactionFeeDependencyException>(
                 retrieveByIdTransactionFeeTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionFeeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTransactionFeeId = Guid.NewGuid();
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
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<TransactionFee> retrieveByIdTransactionFeeTask =
                this.transactionFeeService.RetrieveTransactionFeeByIdAsync(someTransactionFeeId);

            TransactionFeeServiceException actualServiceException =
                 await Assert.ThrowsAsync<TransactionFeeServiceException>(
                     retrieveByIdTransactionFeeTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedTransactionFeeServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
