// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderCategories
{
    public partial class ProviderCategoryServiceTests
    {
        [Fact]
        public async Task ShouldCreateProviderCategoriesAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            ProviderCategory randomProviderCategory = CreateRandomProviderCategory(dateTime);
            randomProviderCategory.UpdatedBy = randomProviderCategory.CreatedBy;
            randomProviderCategory.UpdatedDate = randomProviderCategory.CreatedDate;
            ProviderCategory inputProviderCategory = randomProviderCategory;
            ProviderCategory storageProviderCategory = randomProviderCategory;
            ProviderCategory expectedProviderCategory = randomProviderCategory;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProviderCategoryAsync(inputProviderCategory))
                    .ReturnsAsync(storageProviderCategory);

            // when
            ProviderCategory actualProviderCategory =
                await this.providerCategoryService.CreateProviderCategoryAsync(inputProviderCategory);

            // then
            actualProviderCategory.Should().BeEquivalentTo(expectedProviderCategory);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderCategoryAsync(inputProviderCategory),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
