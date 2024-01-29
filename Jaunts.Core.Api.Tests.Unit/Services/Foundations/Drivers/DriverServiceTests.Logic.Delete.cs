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
        public async Task ShouldDeleteDriverAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Driver randomDriver = CreateRandomDriver(dateTime);
            Guid inputDriverId = randomDriver.Id;
            Driver inputDriver = randomDriver;
            Driver storageDriver = randomDriver;
            Driver expectedDriver = randomDriver;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverByIdAsync(inputDriverId))
                    .ReturnsAsync(inputDriver);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteDriverAsync(inputDriver))
                    .ReturnsAsync(storageDriver);

            // when
            Driver actualDriver =
                await this.driverService.RemoveDriverByIdAsync(inputDriverId);

            // then
            actualDriver.Should().BeEquivalentTo(expectedDriver);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(inputDriverId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDriverAsync(inputDriver),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
