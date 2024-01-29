// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Fleets
{
    public partial class FleetServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someFleetId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedFleetStorageException =
               new FailedFleetStorageException(
                   message: "Failed fleet storage error occurred, please contact support.",
                   sqlException);

            var expectedFleetDependencyException =
                new FleetDependencyException(
                    message: "Fleet dependency error occurred, contact support.",
                    expectedFailedFleetStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Fleet> retrieveByIdFleetTask =
                this.fleetService.RetrieveFleetByIdAsync(someFleetId);

            FleetDependencyException actualDependencyException =
             await Assert.ThrowsAsync<FleetDependencyException>(
                 retrieveByIdFleetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFleetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedFleetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someFleetId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedFleetStorageException =
              new FailedFleetStorageException(
                  message: "Failed fleet storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedFleetDependencyException =
                new FleetDependencyException(
                    message: "Fleet dependency error occurred, contact support.",
                    expectedFailedFleetStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Fleet> retrieveByIdFleetTask =
                this.fleetService.RetrieveFleetByIdAsync(someFleetId);

            FleetDependencyException actualDependencyException =
             await Assert.ThrowsAsync<FleetDependencyException>(
                 retrieveByIdFleetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFleetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetDependencyException))),
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
            Guid someFleetId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();


            var lockedFleetException = new LockedFleetException(
                message: "Locked fleet record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedFleetDependencyException =
                new FleetDependencyException(
                    message: "Fleet dependency error occurred, contact support.",
                    innerException: lockedFleetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Fleet> retrieveByIdFleetTask =
                this.fleetService.RetrieveFleetByIdAsync(someFleetId);

            FleetDependencyException actualDependencyException =
             await Assert.ThrowsAsync<FleetDependencyException>(
                 retrieveByIdFleetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFleetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someFleetId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedFleetServiceException =
                 new FailedFleetServiceException(
                     message: "Failed fleet service error occurred, contact support.",
                     innerException: serviceException);

            var expectedFleetServiceException =
                new FleetServiceException(
                    message: "Fleet service error occurred, contact support.",
                    innerException: failedFleetServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Fleet> retrieveByIdFleetTask =
                this.fleetService.RetrieveFleetByIdAsync(someFleetId);

            FleetServiceException actualServiceException =
                 await Assert.ThrowsAsync<FleetServiceException>(
                     retrieveByIdFleetTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedFleetServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
