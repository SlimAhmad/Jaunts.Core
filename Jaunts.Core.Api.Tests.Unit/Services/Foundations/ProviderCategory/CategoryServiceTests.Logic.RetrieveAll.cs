// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderCategories
{
    public partial class ProviderCategoryServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllProviderCategories()
        {
            // given
            IQueryable<ProviderCategory> randomProviderCategories = CreateRandomProviderCategories();
            IQueryable<ProviderCategory> storageProviderCategories = randomProviderCategories;
            IQueryable<ProviderCategory> expectedProviderCategories = storageProviderCategories;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProviderCategories())
                    .Returns(storageProviderCategories);

            // when
            IQueryable<ProviderCategory> actualProviderCategories =
                this.providerCategoryService.RetrieveAllProviderCategories();

            // then
            actualProviderCategories.Should().BeEquivalentTo(expectedProviderCategories);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProviderCategories(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
