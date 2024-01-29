// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fleet someFleet = CreateRandomFleet(dateTime);
            someFleet.UpdatedBy = someFleet.CreatedBy;
            var sqlException = GetSqlException();

            var expectedFailedFleetStorageException =
                new FailedFleetStorageException(
                    message: "Failed fleet storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedFleetDependencyException =
                new FleetDependencyException(
                    message: "Fleet dependency error occurred, contact support.",
                    innerException: expectedFailedFleetStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Fleet> createFleetTask =
                this.fleetService.CreateFleetAsync(someFleet);

            FleetDependencyException actualDependencyException =
             await Assert.ThrowsAsync<FleetDependencyException>(
                 createFleetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFleetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedFleetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFleetAsync(It.IsAny<Fleet>()),
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
            Fleet someFleet = CreateRandomFleet(dateTime);
            someFleet.UpdatedBy = someFleet.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedFleetStorageException =
                new FailedFleetStorageException(
                    message: "Failed fleet storage error occurred, please contact support.",
                    databaseUpdateException);

            var expectedFleetDependencyException =
                new FleetDependencyException(
                    message: "Fleet dependency error occurred, contact support.",
                    expectedFailedFleetStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Fleet> createFleetTask =
                this.fleetService.CreateFleetAsync(someFleet);

            FleetDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<FleetDependencyException>(
                     createFleetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFleetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFleetAsync(It.IsAny<Fleet>()),
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
            Fleet someFleet = CreateRandomFleet(dateTime);
            someFleet.UpdatedBy = someFleet.CreatedBy;
            var serviceException = new Exception();

            var failedFleetServiceException =
                new FailedFleetServiceException(
                    message: "Failed fleet service error occurred, contact support.",
                    innerException: serviceException);

            var expectedFleetServiceException =
                new FleetServiceException(
                    message: "Fleet service error occurred, contact support.",
                    innerException: failedFleetServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<Fleet> createFleetTask =
                 this.fleetService.CreateFleetAsync(someFleet);

            FleetServiceException actualDependencyException =
                 await Assert.ThrowsAsync<FleetServiceException>(
                     createFleetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFleetServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFleetAsync(It.IsAny<Fleet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
