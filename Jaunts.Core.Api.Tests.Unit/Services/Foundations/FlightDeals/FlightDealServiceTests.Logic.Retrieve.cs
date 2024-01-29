// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDeals
{
    public partial class FlightDealServiceTests
    {
        [Fact]
        async Task ShouldRetrieveFlightDealById()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            FlightDeal randomFlightDeal = CreateRandomFlightDeal(dateTime);
            Guid inputFlightDealId = randomFlightDeal.Id;
            FlightDeal inputFlightDeal = randomFlightDeal;
            FlightDeal expectedFlightDeal = randomFlightDeal;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealByIdAsync(inputFlightDealId))
                    .ReturnsAsync(inputFlightDeal);

            // when
            FlightDeal actualFlightDeal =
                await this.flightDealService.RetrieveFlightDealByIdAsync(inputFlightDealId);

            // then
            actualFlightDeal.Should().BeEquivalentTo(expectedFlightDeal);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(inputFlightDealId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
