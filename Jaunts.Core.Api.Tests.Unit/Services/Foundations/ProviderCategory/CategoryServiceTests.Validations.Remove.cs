// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderCategories
{
    public partial class ProviderCategoryServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomProviderCategoryId = default;
            Guid inputProviderCategoryId = randomProviderCategoryId;

            var invalidProviderCategoryException = new InvalidProviderCategoryException(
                message: "Invalid ProviderCategory. Please fix the errors and try again.");

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.Id),
                values: "Id is required");

            var expectedProviderCategoryValidationException =
                new ProviderCategoryValidationException(
                    message: "ProviderCategory validation error occurred, Please try again.",
                    innerException: invalidProviderCategoryException);

            // when
            ValueTask<ProviderCategory> actualProviderCategoryTask =
                this.providerCategoryService.RemoveProviderCategoryByIdAsync(inputProviderCategoryId);

            ProviderCategoryValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                 actualProviderCategoryTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderCategoryAsync(It.IsAny<ProviderCategory>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageProviderCategoryIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderCategory randomProviderCategory = CreateRandomProviderCategory(dateTime);
            Guid inputProviderCategoryId = randomProviderCategory.Id;
            ProviderCategory inputProviderCategory = randomProviderCategory;
            ProviderCategory nullStorageProviderCategory = null;

            var notFoundProviderCategoryException = new NotFoundProviderCategoryException(inputProviderCategoryId);

            var expectedProviderCategoryValidationException =
                new ProviderCategoryValidationException(
                    message: "ProviderCategory validation error occurred, Please try again.",
                    innerException: notFoundProviderCategoryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderCategoryByIdAsync(inputProviderCategoryId))
                    .ReturnsAsync(nullStorageProviderCategory);

            // when
            ValueTask<ProviderCategory> actualProviderCategoryTask =
                this.providerCategoryService.RemoveProviderCategoryByIdAsync(inputProviderCategoryId);

            ProviderCategoryValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                 actualProviderCategoryTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(inputProviderCategoryId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderCategoryAsync(It.IsAny<ProviderCategory>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
