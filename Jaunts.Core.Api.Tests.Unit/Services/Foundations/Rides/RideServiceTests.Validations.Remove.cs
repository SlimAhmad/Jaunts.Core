// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Rides
{
    public partial class RideServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
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

            // when
            ValueTask<Ride> actualRideTask =
                this.rideService.RemoveRideByIdAsync(inputRideId);

            RideValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<RideValidationException>(
                 actualRideTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRideAsync(It.IsAny<Ride>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageRideIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Ride randomRide = CreateRandomRide(dateTime);
            Guid inputRideId = randomRide.Id;
            Ride inputRide = randomRide;
            Ride nullStorageRide = null;

            var notFoundRideException = new NotFoundRideException(inputRideId);

            var expectedRideValidationException =
                new RideValidationException(
                    message: "Ride validation error occurred, please try again.",
                    innerException: notFoundRideException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideByIdAsync(inputRideId))
                    .ReturnsAsync(nullStorageRide);

            // when
            ValueTask<Ride> actualRideTask =
                this.rideService.RemoveRideByIdAsync(inputRideId);

            RideValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<RideValidationException>(
                 actualRideTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(inputRideId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRideAsync(It.IsAny<Ride>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
