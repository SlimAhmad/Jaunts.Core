// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyProviderCategoryAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            ProviderCategory randomProviderCategory = CreateRandomProviderCategory(randomInputDate);
            ProviderCategory inputProviderCategory = randomProviderCategory;
            ProviderCategory afterUpdateStorageProviderCategory = inputProviderCategory;
            ProviderCategory expectedProviderCategory = afterUpdateStorageProviderCategory;
            ProviderCategory beforeUpdateStorageProviderCategory = randomProviderCategory.DeepClone();
            inputProviderCategory.UpdatedDate = randomDate;
            Guid ProviderCategoryId = inputProviderCategory.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderCategoryByIdAsync(ProviderCategoryId))
                    .ReturnsAsync(beforeUpdateStorageProviderCategory);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateProviderCategoryAsync(inputProviderCategory))
                    .ReturnsAsync(afterUpdateStorageProviderCategory);

            // when
            ProviderCategory actualProviderCategory =
                await this.providerCategoryService.ModifyProviderCategoryAsync(inputProviderCategory);

            // then
            actualProviderCategory.Should().BeEquivalentTo(expectedProviderCategory);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(ProviderCategoryId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderCategoryAsync(inputProviderCategory),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
