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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            TransactionFee someTransactionFee = CreateRandomTransactionFee(randomDateTime);
            someTransactionFee.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedFailedTransactionFeeStorageException =
              new FailedTransactionFeeStorageException(
                  message: "Failed TransactionFee storage error occurred, Please contact support.",
                  innerException: sqlException);

            var expectedTransactionFeeDependencyException =
                new TransactionFeeDependencyException(
                    message: "TransactionFee dependency error occurred, contact support.",
                    innerException: expectedFailedTransactionFeeStorageException);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<TransactionFee> modifyTransactionFeeTask =
                this.transactionFeeService.ModifyTransactionFeeAsync(someTransactionFee);

                TransactionFeeDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<TransactionFeeDependencyException>(
                     modifyTransactionFeeTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionFeeDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTransactionFeeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
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
            TransactionFee someTransactionFee = CreateRandomTransactionFee(randomDateTime);
            someTransactionFee.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedTransactionFeeStorageException =
              new FailedTransactionFeeStorageException(
                  message: "Failed TransactionFee storage error occurred, Please contact support.",
                  databaseUpdateException);

            var expectedTransactionFeeDependencyException =
                new TransactionFeeDependencyException(
                    message: "TransactionFee dependency error occurred, contact support.",
                    expectedFailedTransactionFeeStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<TransactionFee> modifyTransactionFeeTask =
                this.transactionFeeService.ModifyTransactionFeeAsync(someTransactionFee);

            TransactionFeeDependencyException actualDependencyException =
                await Assert.ThrowsAsync<TransactionFeeDependencyException>(
                    modifyTransactionFeeTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionFeeDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
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
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(randomDateTime);
            TransactionFee someTransactionFee = randomTransactionFee;
            someTransactionFee.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTransactionFeeException = new LockedTransactionFeeException(
                message: "Locked TransactionFee record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedTransactionFeeDependencyException =
                new TransactionFeeDependencyException(
                    message: "TransactionFee dependency error occurred, contact support.",
                    innerException: lockedTransactionFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<TransactionFee> modifyTransactionFeeTask =
                this.transactionFeeService.ModifyTransactionFeeAsync(someTransactionFee);

            TransactionFeeDependencyException actualDependencyException =
             await Assert.ThrowsAsync<TransactionFeeDependencyException>(
                 modifyTransactionFeeTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionFeeDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
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
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(randomDateTime);
            TransactionFee someTransactionFee = randomTransactionFee;
            someTransactionFee.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var failedTransactionFeeServiceException =
             new FailedTransactionFeeServiceException(
                 message: "Failed TransactionFee service error occurred, contact support.",
                 innerException: serviceException);

            var expectedTransactionFeeServiceException =
                new TransactionFeeServiceException(
                    message: "TransactionFee service error occurred, contact support.",
                    innerException: failedTransactionFeeServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<TransactionFee> modifyTransactionFeeTask =
                this.transactionFeeService.ModifyTransactionFeeAsync(someTransactionFee);

            TransactionFeeServiceException actualServiceException =
             await Assert.ThrowsAsync<TransactionFeeServiceException>(
                 modifyTransactionFeeTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedTransactionFeeServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
