// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Rides
{
    public partial class RideServiceTests
    {
        [Fact]
        public async Task ShouldDeleteRideAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Ride randomRide = CreateRandomRide(dateTime);
            Guid inputRideId = randomRide.Id;
            Ride inputRide = randomRide;
            Ride storageRide = randomRide;
            Ride expectedRide = randomRide;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideByIdAsync(inputRideId))
                    .ReturnsAsync(inputRide);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteRideAsync(inputRide))
                    .ReturnsAsync(storageRide);

            // when
            Ride actualRide =
                await this.rideService.RemoveRideByIdAsync(inputRideId);

            // then
            actualRide.Should().BeEquivalentTo(expectedRide);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(inputRideId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRideAsync(inputRide),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
