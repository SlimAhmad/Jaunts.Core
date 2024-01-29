// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            TransactionFee someTransactionFee = CreateRandomTransactionFee(dateTime);
            someTransactionFee.UpdatedBy = someTransactionFee.CreatedBy;
            var sqlException = GetSqlException();

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
            ValueTask<TransactionFee> createTransactionFeeTask =
                this.transactionFeeService.CreateTransactionFeeAsync(someTransactionFee);

            TransactionFeeDependencyException actualDependencyException =
             await Assert.ThrowsAsync<TransactionFeeDependencyException>(
                 createTransactionFeeTask.AsTask);

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
                broker.InsertTransactionFeeAsync(It.IsAny<TransactionFee>()),
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
            TransactionFee someTransactionFee = CreateRandomTransactionFee(dateTime);
            someTransactionFee.UpdatedBy = someTransactionFee.CreatedBy;
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
            ValueTask<TransactionFee> createTransactionFeeTask =
                this.transactionFeeService.CreateTransactionFeeAsync(someTransactionFee);

            TransactionFeeDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<TransactionFeeDependencyException>(
                     createTransactionFeeTask.AsTask);

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
                broker.InsertTransactionFeeAsync(It.IsAny<TransactionFee>()),
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
            TransactionFee someTransactionFee = CreateRandomTransactionFee(dateTime);
            someTransactionFee.UpdatedBy = someTransactionFee.CreatedBy;
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
            ValueTask<TransactionFee> createTransactionFeeTask =
                 this.transactionFeeService.CreateTransactionFeeAsync(someTransactionFee);

            TransactionFeeServiceException actualDependencyException =
                 await Assert.ThrowsAsync<TransactionFeeServiceException>(
                     createTransactionFeeTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedTransactionFeeServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTransactionFeeAsync(It.IsAny<TransactionFee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
