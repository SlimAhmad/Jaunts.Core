// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Hosting;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Rides
{
    public partial class RideServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomRideId = default;
            Guid inputRideId = randomRideId;

            var invalidRideException = new InvalidRideException(
                message: "Invalid ride. Please correct the errors and try again.");

            invalidRideException.AddData(
                key: nameof(Ride.Id),
                values: "Id is required");

            var expectedRideValidationException =
                new RideValidationException(
                    message: "Ride validation error occurred, please try again.", 
                    innerException: invalidRideException);

            //when
            ValueTask<Ride> retrieveRideByIdTask =
                this.rideService.RetrieveRideByIdAsync(inputRideId);

            RideValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<RideValidationException>(
                 retrieveRideByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageRideIsNullAndLogItAsync()
        {
            //given
            Guid randomRideId = Guid.NewGuid();
            Guid someRideId = randomRideId;
            Ride invalidStorageRide = null;
            var notFoundRideException = new NotFoundRideException(
                message: $"Couldn't find Ride with id: {someRideId}.");

            var expectedRideValidationException =
                new RideValidationException(
                    message: "Ride validation error occurred, please try again.",
                    innerException: notFoundRideException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageRide);

            //when
            ValueTask<Ride> retrieveRideByIdTask =
                this.rideService.RetrieveRideByIdAsync(someRideId);

            RideValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<RideValidationException>(
                 retrieveRideByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}