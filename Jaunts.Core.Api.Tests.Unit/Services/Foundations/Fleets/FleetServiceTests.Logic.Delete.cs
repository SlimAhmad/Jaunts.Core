// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Fleets
{
    public partial class FleetServiceTests
    {
        [Fact]
        public async Task ShouldDeleteFleetAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fleet randomFleet = CreateRandomFleet(dateTime);
            Guid inputFleetId = randomFleet.Id;
            Fleet inputFleet = randomFleet;
            Fleet storageFleet = randomFleet;
            Fleet expectedFleet = randomFleet;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFleetByIdAsync(inputFleetId))
                    .ReturnsAsync(inputFleet);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteFleetAsync(inputFleet))
                    .ReturnsAsync(storageFleet);

            // when
            Fleet actualFleet =
                await this.fleetService.RemoveFleetByIdAsync(inputFleetId);

            // then
            actualFleet.Should().BeEquivalentTo(expectedFleet);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(inputFleetId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFleetAsync(inputFleet),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
