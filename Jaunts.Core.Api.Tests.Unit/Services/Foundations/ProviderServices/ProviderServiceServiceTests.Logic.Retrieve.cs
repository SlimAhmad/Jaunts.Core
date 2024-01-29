// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
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
        async Task ShouldRetrieveProviderServiceById()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderService randomProviderService = CreateRandomProviderService(dateTime);
            Guid inputProviderServiceId = randomProviderService.Id;
            ProviderService inputProviderService = randomProviderService;
            ProviderService expectedProviderService = randomProviderService;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderServiceByIdAsync(inputProviderServiceId))
                    .ReturnsAsync(inputProviderService);

            // when
            ProviderService actualProviderService =
                await this.providerServiceService.RetrieveProviderServiceByIdAsync(inputProviderServiceId);

            // then
            actualProviderService.Should().BeEquivalentTo(expectedProviderService);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(inputProviderServiceId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
