// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Rides
{
    public partial class RideServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllRides()
        {
            // given
            IQueryable<Ride> randomRides = CreateRandomRides();
            IQueryable<Ride> storageRides = randomRides;
            IQueryable<Ride> expectedRides = storageRides;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRides())
                    .Returns(storageRides);

            // when
            IQueryable<Ride> actualRides =
                this.rideService.RetrieveAllRides();

            // then
            actualRides.Should().BeEquivalentTo(expectedRides);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRides(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
