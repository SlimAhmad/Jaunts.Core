// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Rides
{
    public partial class RideServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllRides())
                    .Throws(sqlException);

            // when
            Action retrieveAllRidesAction = () =>
                this.rideService.RetrieveAllRides();

            RideDependencyException actualDependencyException =
              Assert.Throws<RideDependencyException>(
                 retrieveAllRidesAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedRideDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRides(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllRides())
                    .Throws(serviceException);

            // when
            Action retrieveAllRidesAction = () =>
                this.rideService.RetrieveAllRides();

            RideServiceException actualServiceException =
              Assert.Throws<RideServiceException>(
                 retrieveAllRidesAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedRideServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRides(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
