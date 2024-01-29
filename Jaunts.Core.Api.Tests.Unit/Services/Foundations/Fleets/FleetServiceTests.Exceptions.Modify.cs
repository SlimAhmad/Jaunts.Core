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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Fleet someFleet = CreateRandomFleet(randomDateTime);
            someFleet.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

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
            ValueTask<Fleet> modifyFleetTask =
                this.fleetService.ModifyFleetAsync(someFleet);

                FleetDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<FleetDependencyException>(
                     modifyFleetTask.AsTask);

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
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
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
            Fleet someFleet = CreateRandomFleet(randomDateTime);
            someFleet.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Fleet> modifyFleetTask =
                this.fleetService.ModifyFleetAsync(someFleet);

            FleetDependencyException actualDependencyException =
                await Assert.ThrowsAsync<FleetDependencyException>(
                    modifyFleetTask.AsTask);

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
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
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
            Fleet randomFleet = CreateRandomFleet(randomDateTime);
            Fleet someFleet = randomFleet;
            someFleet.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedFleetException = new LockedFleetException(
                message: "Locked fleet record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedFleetDependencyException =
                new FleetDependencyException(
                    message: "Fleet dependency error occurred, contact support.",
                    innerException: lockedFleetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Fleet> modifyFleetTask =
                this.fleetService.ModifyFleetAsync(someFleet);

            FleetDependencyException actualDependencyException =
             await Assert.ThrowsAsync<FleetDependencyException>(
                 modifyFleetTask.AsTask);

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
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
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
            Fleet randomFleet = CreateRandomFleet(randomDateTime);
            Fleet someFleet = randomFleet;
            someFleet.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Fleet> modifyFleetTask =
                this.fleetService.ModifyFleetAsync(someFleet);

            FleetServiceException actualServiceException =
             await Assert.ThrowsAsync<FleetServiceException>(
                 modifyFleetTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedFleetServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
