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
        async Task ShouldRetrieveRideById()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Ride randomRide = CreateRandomRide(dateTime);
            Guid inputRideId = randomRide.Id;
            Ride inputRide = randomRide;
            Ride expectedRide = randomRide;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideByIdAsync(inputRideId))
                    .ReturnsAsync(inputRide);

            // when
            Ride actualRide =
                await this.rideService.RetrieveRideByIdAsync(inputRideId);

            // then
            actualRide.Should().BeEquivalentTo(expectedRide);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(inputRideId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
