// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Providers
{
    public partial class ProviderServiceTests
    {
        [Fact]
        public async Task ShouldCreateProviderAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            Provider randomProvider = CreateRandomProvider(dateTime);
            randomProvider.UpdatedBy = randomProvider.CreatedBy;
            randomProvider.UpdatedDate = randomProvider.CreatedDate;
            Provider inputProvider = randomProvider;
            Provider storageProvider = randomProvider;
            Provider expectedProvider = randomProvider;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProviderAsync(inputProvider))
                    .ReturnsAsync(storageProvider);

            // when
            Provider actualProvider =
                await this.providerService.CreateProviderAsync(inputProvider);

            // then
            actualProvider.Should().BeEquivalentTo(expectedProvider);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAsync(inputProvider),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
