// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Drivers
{
    public partial class DriverServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllDrivers()
        {
            // given
            IQueryable<Driver> randomDrivers = CreateRandomDrivers();
            IQueryable<Driver> storageDrivers = randomDrivers;
            IQueryable<Driver> expectedDrivers = storageDrivers;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDrivers())
                    .Returns(storageDrivers);

            // when
            IQueryable<Driver> actualDrivers =
                this.driverService.RetrieveAllDrivers();

            // then
            actualDrivers.Should().BeEquivalentTo(expectedDrivers);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDrivers(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
