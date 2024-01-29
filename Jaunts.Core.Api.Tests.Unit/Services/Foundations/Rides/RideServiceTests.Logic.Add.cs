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
        public async Task ShouldCreateRideAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            Ride randomRide = CreateRandomRide(dateTime);
            randomRide.UpdatedBy = randomRide.CreatedBy;
            randomRide.UpdatedDate = randomRide.CreatedDate;
            Ride inputRide = randomRide;
            Ride storageRide = randomRide;
            Ride expectedRide = randomRide;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRideAsync(inputRide))
                    .ReturnsAsync(storageRide);

            // when
            Ride actualRide =
                await this.rideService.CreateRideAsync(inputRide);

            // then
            actualRide.Should().BeEquivalentTo(expectedRide);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAsync(inputRide),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
