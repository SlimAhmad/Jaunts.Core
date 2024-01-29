// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Providers
{
    public partial class ProviderServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllProviders()
        {
            // given
            IQueryable<Provider> randomProviders = CreateRandomProviders();
            IQueryable<Provider> storageProviders = randomProviders;
            IQueryable<Provider> expectedProviders = storageProviders;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProviders())
                    .Returns(storageProviders);

            // when
            IQueryable<Provider> actualProviders =
                this.providerService.RetrieveAllProviders();

            // then
            actualProviders.Should().BeEquivalentTo(expectedProviders);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProviders(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
