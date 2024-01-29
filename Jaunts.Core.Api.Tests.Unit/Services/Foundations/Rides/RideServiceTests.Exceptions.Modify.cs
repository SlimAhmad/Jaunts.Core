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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Ride someRide = CreateRandomRide(randomDateTime);
            someRide.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

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
            ValueTask<Ride> modifyRideTask =
                this.rideService.ModifyRideAsync(someRide);

                RideDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<RideDependencyException>(
                     modifyRideTask.AsTask);

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
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
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
            Ride someRide = CreateRandomRide(randomDateTime);
            someRide.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Ride> modifyRideTask =
                this.rideService.ModifyRideAsync(someRide);

            RideDependencyException actualDependencyException =
                await Assert.ThrowsAsync<RideDependencyException>(
                    modifyRideTask.AsTask);

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
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
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
            Ride randomRide = CreateRandomRide(randomDateTime);
            Ride someRide = randomRide;
            someRide.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedRideException = new LockedRideException(
                message: "Locked Ride record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedRideDependencyException =
                new RideDependencyException(
                    message: "Ride dependency error occurred, contact support.",
                    innerException: lockedRideException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Ride> modifyRideTask =
                this.rideService.ModifyRideAsync(someRide);

            RideDependencyException actualDependencyException =
             await Assert.ThrowsAsync<RideDependencyException>(
                 modifyRideTask.AsTask);

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
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
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
            Ride randomRide = CreateRandomRide(randomDateTime);
            Ride someRide = randomRide;
            someRide.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Ride> modifyRideTask =
                this.rideService.ModifyRideAsync(someRide);

            RideServiceException actualServiceException =
             await Assert.ThrowsAsync<RideServiceException>(
                 modifyRideTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedRideServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
