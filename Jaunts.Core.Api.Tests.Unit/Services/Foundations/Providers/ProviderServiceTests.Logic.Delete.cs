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
        public async Task ShouldDeleteProviderAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Provider randomProvider = CreateRandomProvider(dateTime);
            Guid inputProviderId = randomProvider.Id;
            Provider inputProvider = randomProvider;
            Provider storageProvider = randomProvider;
            Provider expectedProvider = randomProvider;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderByIdAsync(inputProviderId))
                    .ReturnsAsync(inputProvider);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteProviderAsync(inputProvider))
                    .ReturnsAsync(storageProvider);

            // when
            Provider actualProvider =
                await this.providerService.RemoveProviderByIdAsync(inputProviderId);

            // then
            actualProvider.Should().BeEquivalentTo(expectedProvider);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(inputProviderId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderAsync(inputProvider),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
