// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyFleetAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Fleet randomFleet = CreateRandomFleet(randomInputDate);
            Fleet inputFleet = randomFleet;
            Fleet afterUpdateStorageFleet = inputFleet;
            Fleet expectedFleet = afterUpdateStorageFleet;
            Fleet beforeUpdateStorageFleet = randomFleet.DeepClone();
            inputFleet.UpdatedDate = randomDate;
            Guid FleetId = inputFleet.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFleetByIdAsync(FleetId))
                    .ReturnsAsync(beforeUpdateStorageFleet);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateFleetAsync(inputFleet))
                    .ReturnsAsync(afterUpdateStorageFleet);

            // when
            Fleet actualFleet =
                await this.fleetService.ModifyFleetAsync(inputFleet);

            // then
            actualFleet.Should().BeEquivalentTo(expectedFleet);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(FleetId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFleetAsync(inputFleet),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
