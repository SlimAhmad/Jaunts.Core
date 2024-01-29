// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Drivers
{
    public partial class DriverServiceTests
    {
        [Fact]
        async Task ShouldRetrieveDriverById()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Driver randomDriver = CreateRandomDriver(dateTime);
            Guid inputDriverId = randomDriver.Id;
            Driver inputDriver = randomDriver;
            Driver expectedDriver = randomDriver;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverByIdAsync(inputDriverId))
                    .ReturnsAsync(inputDriver);

            // when
            Driver actualDriver =
                await this.driverService.RetrieveDriverByIdAsync(inputDriverId);

            // then
            actualDriver.Should().BeEquivalentTo(expectedDriver);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(inputDriverId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
