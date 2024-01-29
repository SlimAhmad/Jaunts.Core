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
        public async Task ShouldCreateProviderCategoriesAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            ProviderService randomProviderService = CreateRandomProviderService(dateTime);
            randomProviderService.UpdatedBy = randomProviderService.CreatedBy;
            randomProviderService.UpdatedDate = randomProviderService.CreatedDate;
            ProviderService inputProviderService = randomProviderService;
            ProviderService storageProviderService = randomProviderService;
            ProviderService expectedProviderService = randomProviderService;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProviderServiceAsync(inputProviderService))
                    .ReturnsAsync(storageProviderService);

            // when
            ProviderService actualProviderService =
                await this.providerServiceService.CreateProviderServiceAsync(inputProviderService);

            // then
            actualProviderService.Should().BeEquivalentTo(expectedProviderService);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderServiceAsync(inputProviderService),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
