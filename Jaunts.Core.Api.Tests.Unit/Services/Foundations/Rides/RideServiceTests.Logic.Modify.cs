// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyRideAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Ride randomRide = CreateRandomRide(randomInputDate);
            Ride inputRide = randomRide;
            Ride afterUpdateStorageRide = inputRide;
            Ride expectedRide = afterUpdateStorageRide;
            Ride beforeUpdateStorageRide = randomRide.DeepClone();
            inputRide.UpdatedDate = randomDate;
            Guid rideId = inputRide.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideByIdAsync(rideId))
                    .ReturnsAsync(beforeUpdateStorageRide);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateRideAsync(inputRide))
                    .ReturnsAsync(afterUpdateStorageRide);

            // when
            Ride actualRide =
                await this.rideService.ModifyRideAsync(inputRide);

            // then
            actualRide.Should().BeEquivalentTo(expectedRide);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(rideId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRideAsync(inputRide),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
