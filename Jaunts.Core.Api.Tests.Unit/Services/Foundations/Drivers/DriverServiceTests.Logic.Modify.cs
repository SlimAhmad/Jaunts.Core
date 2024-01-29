// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyDriverAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Driver randomDriver = CreateRandomDriver(randomInputDate);
            Driver inputDriver = randomDriver;
            Driver afterUpdateStorageDriver = inputDriver;
            Driver expectedDriver = afterUpdateStorageDriver;
            Driver beforeUpdateStorageDriver = randomDriver.DeepClone();
            inputDriver.UpdatedDate = randomDate;
            Guid DriverId = inputDriver.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverByIdAsync(DriverId))
                    .ReturnsAsync(beforeUpdateStorageDriver);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDriverAsync(inputDriver))
                    .ReturnsAsync(afterUpdateStorageDriver);

            // when
            Driver actualDriver =
                await this.driverService.ModifyDriverAsync(inputDriver);

            // then
            actualDriver.Should().BeEquivalentTo(expectedDriver);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(DriverId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDriverAsync(inputDriver),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
