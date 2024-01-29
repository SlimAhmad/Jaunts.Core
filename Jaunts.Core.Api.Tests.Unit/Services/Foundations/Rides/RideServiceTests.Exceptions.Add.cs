// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Ride someRide = CreateRandomRide(dateTime);
            someRide.UpdatedBy = someRide.CreatedBy;
            var sqlException = GetSqlException();

            var expectedFailedRideStorageException =
                new FailedRideStorageException(
                    message: "Failed Ride storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedRideDependencyException =
                new RideDependencyException(
                    message: "Ride dependency error occurred, contact support.",
                    innerException: expectedFailedRideStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Ride> createRideTask =
                this.rideService.CreateRideAsync(someRide);

            RideDependencyException actualDependencyException =
             await Assert.ThrowsAsync<RideDependencyException>(
                 createRideTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedRideDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRideDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAsync(It.IsAny<Ride>()),
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
            Ride someRide = CreateRandomRide(dateTime);
            someRide.UpdatedBy = someRide.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedRideStorageException =
                new FailedRideStorageException(
                    message: "Failed Ride storage error occurred, please contact support.",
                    databaseUpdateException);

            var expectedRideDependencyException =
                new RideDependencyException(
                    message: "Ride dependency error occurred, contact support.",
                    expectedFailedRideStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Ride> createRideTask =
                this.rideService.CreateRideAsync(someRide);

            RideDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<RideDependencyException>(
                     createRideTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedRideDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAsync(It.IsAny<Ride>()),
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
            Ride someRide = CreateRandomRide(dateTime);
            someRide.UpdatedBy = someRide.CreatedBy;
            var serviceException = new Exception();

            var failedRideServiceException =
                new FailedRideServiceException(
                    message: "Failed Ride service error occurred, contact support.",
                    innerException: serviceException);

            var expectedRideServiceException =
                new RideServiceException(
                    message: "Ride service error occurred, contact support.",
                    innerException: failedRideServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<Ride> createRideTask =
                 this.rideService.CreateRideAsync(someRide);

            RideServiceException actualDependencyException =
                 await Assert.ThrowsAsync<RideServiceException>(
                     createRideTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedRideServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAsync(It.IsAny<Ride>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
