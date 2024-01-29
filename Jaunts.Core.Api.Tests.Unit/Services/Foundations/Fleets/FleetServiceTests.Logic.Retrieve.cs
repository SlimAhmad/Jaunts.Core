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
        async Task ShouldRetrieveFleetById()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fleet randomFleet = CreateRandomFleet(dateTime);
            Guid inputFleetId = randomFleet.Id;
            Fleet inputFleet = randomFleet;
            Fleet expectedFleet = randomFleet;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFleetByIdAsync(inputFleetId))
                    .ReturnsAsync(inputFleet);

            // when
            Fleet actualFleet =
                await this.fleetService.RetrieveFleetByIdAsync(inputFleetId);

            // then
            actualFleet.Should().BeEquivalentTo(expectedFleet);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(inputFleetId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
