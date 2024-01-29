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
        public async Task ShouldCreateFlightDealAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            FlightDeal randomFlightDeal = CreateRandomFlightDeal(dateTime);
            randomFlightDeal.UpdatedBy = randomFlightDeal.CreatedBy;
            randomFlightDeal.UpdatedDate = randomFlightDeal.CreatedDate;
            FlightDeal inputFlightDeal = randomFlightDeal;
            FlightDeal storageFlightDeal = randomFlightDeal;
            FlightDeal expectedFlightDeal = randomFlightDeal;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertFlightDealAsync(inputFlightDeal))
                    .ReturnsAsync(storageFlightDeal);

            // when
            FlightDeal actualFlightDeal =
                await this.flightDealService.CreateFlightDealAsync(inputFlightDeal);

            // then
            actualFlightDeal.Should().BeEquivalentTo(expectedFlightDeal);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAsync(inputFlightDeal),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
