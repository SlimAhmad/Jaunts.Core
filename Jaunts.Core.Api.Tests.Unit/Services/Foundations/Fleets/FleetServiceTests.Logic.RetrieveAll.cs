// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Fleets
{
    public partial class FleetServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllFleets()
        {
            // given
            IQueryable<Fleet> randomFleets = CreateRandomFleets();
            IQueryable<Fleet> storageFleets = randomFleets;
            IQueryable<Fleet> expectedFleets = storageFleets;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFleets())
                    .Returns(storageFleets);

            // when
            IQueryable<Fleet> actualFleets =
                this.fleetService.RetrieveAllFleets();

            // then
            actualFleets.Should().BeEquivalentTo(expectedFleets);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFleets(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
