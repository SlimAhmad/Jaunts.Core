// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDeals
{
    public partial class FlightDealServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllFlightDeals()
        {
            // given
            IQueryable<FlightDeal> randomFlightDeals = CreateRandomFlightDeals();
            IQueryable<FlightDeal> storageFlightDeals = randomFlightDeals;
            IQueryable<FlightDeal> expectedFlightDeals = storageFlightDeals;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFlightDeals())
                    .Returns(storageFlightDeals);

            // when
            IQueryable<FlightDeal> actualFlightDeals =
                this.flightDealService.RetrieveAllFlightDeals();

            // then
            actualFlightDeals.Should().BeEquivalentTo(expectedFlightDeals);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFlightDeals(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
