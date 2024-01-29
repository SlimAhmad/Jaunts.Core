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
        public async Task ShouldCreateFleetAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            Fleet randomFleet = CreateRandomFleet(dateTime);
            randomFleet.UpdatedBy = randomFleet.CreatedBy;
            randomFleet.UpdatedDate = randomFleet.CreatedDate;
            Fleet inputFleet = randomFleet;
            Fleet storageFleet = randomFleet;
            Fleet expectedFleet = randomFleet;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertFleetAsync(inputFleet))
                    .ReturnsAsync(storageFleet);

            // when
            Fleet actualFleet =
                await this.fleetService.CreateFleetAsync(inputFleet);

            // then
            actualFleet.Should().BeEquivalentTo(expectedFleet);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFleetAsync(inputFleet),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
