// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderCategories
{
    public partial class ProviderCategoryServiceTests
    {
        [Fact]
        public async Task ShouldDeleteProviderCategoryAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderCategory randomProviderCategory = CreateRandomProviderCategory(dateTime);
            Guid inputProviderCategoryId = randomProviderCategory.Id;
            ProviderCategory inputProviderCategory = randomProviderCategory;
            ProviderCategory storageProviderCategory = randomProviderCategory;
            ProviderCategory expectedProviderCategory = randomProviderCategory;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderCategoryByIdAsync(inputProviderCategoryId))
                    .ReturnsAsync(inputProviderCategory);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteProviderCategoryAsync(inputProviderCategory))
                    .ReturnsAsync(storageProviderCategory);

            // when
            ProviderCategory actualProviderCategory =
                await this.providerCategoryService.RemoveProviderCategoryByIdAsync(inputProviderCategoryId);

            // then
            actualProviderCategory.Should().BeEquivalentTo(expectedProviderCategory);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(inputProviderCategoryId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderCategoryAsync(inputProviderCategory),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
