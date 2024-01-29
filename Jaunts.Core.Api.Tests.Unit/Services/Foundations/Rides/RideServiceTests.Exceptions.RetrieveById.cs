// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Rides
{
    public partial class RideServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someRideId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedRideStorageException =
               new FailedRideStorageException(
                   message: "Failed Ride storage error occurred, please contact support.",
                   sqlException);

            var expectedRideDependencyException =
                new RideDependencyException(
                    message: "Ride dependency error occurred, contact support.",
                    expectedFailedRideStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Ride> retrieveByIdRideTask =
                this.rideService.RetrieveRideByIdAsync(someRideId);

            RideDependencyException actualDependencyException =
             await Assert.ThrowsAsync<RideDependencyException>(
                 retrieveByIdRideTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedRideDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRideDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someRideId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedRideStorageException =
              new FailedRideStorageException(
                  message: "Failed Ride storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedRideDependencyException =
                new RideDependencyException(
                    message: "Ride dependency error occurred, contact support.",
                    expectedFailedRideStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Ride> retrieveByIdRideTask =
                this.rideService.RetrieveRideByIdAsync(someRideId);

            RideDependencyException actualDependencyException =
             await Assert.ThrowsAsync<RideDependencyException>(
                 retrieveByIdRideTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedRideDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideDependencyException))),
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
            Guid someRideId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();


            var lockedRideException = new LockedRideException(
                message: "Locked Ride record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedRideDependencyException =
                new RideDependencyException(
                    message: "Ride dependency error occurred, contact support.",
                    innerException: lockedRideException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Ride> retrieveByIdRideTask =
                this.rideService.RetrieveRideByIdAsync(someRideId);

            RideDependencyException actualDependencyException =
             await Assert.ThrowsAsync<RideDependencyException>(
                 retrieveByIdRideTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedRideDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someRideId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedRideServiceException =
                 new FailedRideServiceException(
                     message: "Failed Ride service error occurred, contact support.",
                     innerException: serviceException);

            var expectedRideServiceException =
                new RideServiceException(
                    message: "Ride service error occurred, contact support.",
                    innerException: failedRideServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Ride> retrieveByIdRideTask =
                this.rideService.RetrieveRideByIdAsync(someRideId);

            RideServiceException actualServiceException =
                 await Assert.ThrowsAsync<RideServiceException>(
                     retrieveByIdRideTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedRideServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
