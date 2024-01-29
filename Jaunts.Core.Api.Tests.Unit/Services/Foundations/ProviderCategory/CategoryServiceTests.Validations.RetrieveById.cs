// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
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

            //when
            ValueTask<ProviderCategory> retrieveProviderCategoryByIdTask =
                this.providerCategoryService.RetrieveProviderCategoryByIdAsync(inputProviderCategoryId);

            ProviderCategoryValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                 retrieveProviderCategoryByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageProviderCategoryIsNullAndLogItAsync()
        {
            //given
            Guid randomProviderCategoryId = Guid.NewGuid();
            Guid someProviderCategoryId = randomProviderCategoryId;
            ProviderCategory invalidStorageProviderCategory = null;
            var notFoundProviderCategoryException = new NotFoundProviderCategoryException(
                message: $"Couldn't find ProviderCategory with id: {someProviderCategoryId}.");

            var expectedProviderCategoryValidationException =
                new ProviderCategoryValidationException(
                    message: "ProviderCategory validation error occurred, Please try again.",
                    innerException: notFoundProviderCategoryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageProviderCategory);

            //when
            ValueTask<ProviderCategory> retrieveProviderCategoryByIdTask =
                this.providerCategoryService.RetrieveProviderCategoryByIdAsync(someProviderCategoryId);

            ProviderCategoryValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                 retrieveProviderCategoryByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}