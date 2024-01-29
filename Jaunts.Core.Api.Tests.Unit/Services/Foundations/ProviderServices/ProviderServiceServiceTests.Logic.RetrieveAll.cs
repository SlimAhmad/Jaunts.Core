// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderServices
{
    public partial class ProviderServiceServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllProviderServices()
        {
            // given
            IQueryable<ProviderService> randomProviderServices = CreateRandomProviderServices();
            IQueryable<ProviderService> storageProviderServices = randomProviderServices;
            IQueryable<ProviderService> expectedProviderServices = storageProviderServices;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProviderServices())
                    .Returns(storageProviderServices);

            // when
            IQueryable<ProviderService> actualProviderServices =
                this.providerServiceService.RetrieveAllProviderServices();

            // then
            actualProviderServices.Should().BeEquivalentTo(expectedProviderServices);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProviderServices(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
