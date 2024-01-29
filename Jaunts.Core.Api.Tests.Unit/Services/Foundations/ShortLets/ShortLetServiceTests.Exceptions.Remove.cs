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
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someShortLetId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedShortLetStorageException =
              new FailedShortLetStorageException(
                  message: "Failed ShortLet storage error occurred, please contact support.",
                  sqlException);

            var expectedShortLetDependencyException =
                new ShortLetDependencyException(
                    message: "ShortLet dependency error occurred, contact support.",
                    expectedFailedShortLetStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetByIdAsync(someShortLetId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ShortLet> deleteShortLetTask =
                this.shortLetService.RemoveShortLetByIdAsync(someShortLetId);

            ShortLetDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ShortLetDependencyException>(
                    deleteShortLetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedShortLetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(someShortLetId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShortLetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someShortLetId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedShortLetStorageException =
              new FailedShortLetStorageException(
                  message: "Failed ShortLet storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedShortLetDependencyException =
                new ShortLetDependencyException(
                    message: "ShortLet dependency error occurred, contact support.",
                    expectedFailedShortLetStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetByIdAsync(someShortLetId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ShortLet> deleteShortLetTask =
                this.shortLetService.RemoveShortLetByIdAsync(someShortLetId);

            ShortLetDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ShortLetDependencyException>(
                    deleteShortLetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedShortLetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(someShortLetId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someShortLetId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedShortLetException = new LockedShortLetException(
                message: "Locked ShortLet record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedShortLetDependencyException =
                new ShortLetDependencyException(
                    message: "ShortLet dependency error occurred, contact support.",
                    innerException: lockedShortLetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetByIdAsync(someShortLetId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ShortLet> deleteShortLetTask =
                this.shortLetService.RemoveShortLetByIdAsync(someShortLetId);

            ShortLetDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ShortLetDependencyException>(
                    deleteShortLetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedShortLetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(someShortLetId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someShortLetId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedShortLetServiceException =
             new FailedShortLetServiceException(
                 message: "Failed ShortLet service error occurred, contact support.",
                 innerException: serviceException);

            var expectedShortLetServiceException =
                new ShortLetServiceException(
                    message: "ShortLet service error occurred, contact support.",
                    innerException: failedShortLetServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetByIdAsync(someShortLetId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ShortLet> deleteShortLetTask =
                this.shortLetService.RemoveShortLetByIdAsync(someShortLetId);

            ShortLetServiceException actualServiceException =
             await Assert.ThrowsAsync<ShortLetServiceException>(
                 deleteShortLetTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedShortLetServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(someShortLetId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
