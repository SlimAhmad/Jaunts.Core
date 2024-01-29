// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLets
{
    public partial class ShortLetServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ShortLet someShortLet = CreateRandomShortLet(randomDateTime);
            someShortLet.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedFailedShortLetStorageException =
              new FailedShortLetStorageException(
                  message: "Failed ShortLet storage error occurred, please contact support.",
                  innerException: sqlException);

            var expectedShortLetDependencyException =
                new ShortLetDependencyException(
                    message: "ShortLet dependency error occurred, contact support.",
                    innerException: expectedFailedShortLetStorageException);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<ShortLet> modifyShortLetTask =
                this.shortLetService.ModifyShortLetAsync(someShortLet);

                ShortLetDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<ShortLetDependencyException>(
                     modifyShortLetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedShortLetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShortLetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(It.IsAny<Guid>()),
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
            ShortLet someShortLet = CreateRandomShortLet(randomDateTime);
            someShortLet.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedShortLetStorageException =
              new FailedShortLetStorageException(
                  message: "Failed ShortLet storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedShortLetDependencyException =
                new ShortLetDependencyException(
                    message: "ShortLet dependency error occurred, contact support.",
                    expectedFailedShortLetStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<ShortLet> modifyShortLetTask =
                this.shortLetService.ModifyShortLetAsync(someShortLet);

            ShortLetDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ShortLetDependencyException>(
                    modifyShortLetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedShortLetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(It.IsAny<Guid>()),
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
            ShortLet randomShortLet = CreateRandomShortLet(randomDateTime);
            ShortLet someShortLet = randomShortLet;
            someShortLet.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedShortLetException = new LockedShortLetException(
                message: "Locked ShortLet record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedShortLetDependencyException =
                new ShortLetDependencyException(
                    message: "ShortLet dependency error occurred, contact support.",
                    innerException: lockedShortLetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<ShortLet> modifyShortLetTask =
                this.shortLetService.ModifyShortLetAsync(someShortLet);

            ShortLetDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ShortLetDependencyException>(
                 modifyShortLetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedShortLetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(It.IsAny<Guid>()),
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
            ShortLet randomShortLet = CreateRandomShortLet(randomDateTime);
            ShortLet someShortLet = randomShortLet;
            someShortLet.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var failedShortLetServiceException =
             new FailedShortLetServiceException(
                 message: "Failed ShortLet service error occurred, contact support.",
                 innerException: serviceException);

            var expectedShortLetServiceException =
                new ShortLetServiceException(
                    message: "ShortLet service error occurred, contact support.",
                    innerException: failedShortLetServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<ShortLet> modifyShortLetTask =
                this.shortLetService.ModifyShortLetAsync(someShortLet);

            ShortLetServiceException actualServiceException =
             await Assert.ThrowsAsync<ShortLetServiceException>(
                 modifyShortLetTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedShortLetServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
