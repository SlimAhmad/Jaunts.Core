// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderServices
{
    public partial class ProviderServiceServiceTests
    {
        [Fact]
        public async Task ShouldModifyProviderServiceAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            ProviderService randomProviderService = CreateRandomProviderService(randomInputDate);
            ProviderService inputProviderService = randomProviderService;
            ProviderService afterUpdateStorageProviderService = inputProviderService;
            ProviderService expectedProviderService = afterUpdateStorageProviderService;
            ProviderService beforeUpdateStorageProviderService = randomProviderService.DeepClone();
            inputProviderService.UpdatedDate = randomDate;
            Guid ProviderServiceId = inputProviderService.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderServiceByIdAsync(ProviderServiceId))
                    .ReturnsAsync(beforeUpdateStorageProviderService);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateProviderServiceAsync(inputProviderService))
                    .ReturnsAsync(afterUpdateStorageProviderService);

            // when
            ProviderService actualProviderService =
                await this.providerServiceService.ModifyProviderServiceAsync(inputProviderService);

            // then
            actualProviderService.Should().BeEquivalentTo(expectedProviderService);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(ProviderServiceId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderServiceAsync(inputProviderService),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
