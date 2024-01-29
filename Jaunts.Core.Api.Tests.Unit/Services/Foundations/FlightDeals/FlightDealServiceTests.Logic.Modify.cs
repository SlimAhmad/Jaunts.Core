// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyFlightDealAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            FlightDeal randomFlightDeal = CreateRandomFlightDeal(randomInputDate);
            FlightDeal inputFlightDeal = randomFlightDeal;
            FlightDeal afterUpdateStorageFlightDeal = inputFlightDeal;
            FlightDeal expectedFlightDeal = afterUpdateStorageFlightDeal;
            FlightDeal beforeUpdateStorageFlightDeal = randomFlightDeal.DeepClone();
            inputFlightDeal.UpdatedDate = randomDate;
            Guid FlightDealId = inputFlightDeal.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealByIdAsync(FlightDealId))
                    .ReturnsAsync(beforeUpdateStorageFlightDeal);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateFlightDealAsync(inputFlightDeal))
                    .ReturnsAsync(afterUpdateStorageFlightDeal);

            // when
            FlightDeal actualFlightDeal =
                await this.flightDealService.ModifyFlightDealAsync(inputFlightDeal);

            // then
            actualFlightDeal.Should().BeEquivalentTo(expectedFlightDeal);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(FlightDealId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFlightDealAsync(inputFlightDeal),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
