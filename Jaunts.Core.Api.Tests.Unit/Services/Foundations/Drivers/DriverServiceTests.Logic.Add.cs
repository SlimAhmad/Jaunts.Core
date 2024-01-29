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
        public async Task ShouldCreateDriverAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            Driver randomDriver = CreateRandomDriver(dateTime);
            randomDriver.UpdatedBy = randomDriver.CreatedBy;
            randomDriver.UpdatedDate = randomDriver.CreatedDate;
            Driver inputDriver = randomDriver;
            Driver storageDriver = randomDriver;
            Driver expectedDriver = randomDriver;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDriverAsync(inputDriver))
                    .ReturnsAsync(storageDriver);

            // when
            Driver actualDriver =
                await this.driverService.CreateDriverAsync(inputDriver);

            // then
            actualDriver.Should().BeEquivalentTo(expectedDriver);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAsync(inputDriver),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
