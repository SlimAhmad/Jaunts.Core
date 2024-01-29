// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldThrowValidationExceptionOnModifyWhenProviderCategoryIsNullAndLogItAsync()
        {
            //given
            ProviderCategory invalidProviderCategory = null;
            var nullProviderCategoryException = new NullProviderCategoryException();

            var expectedProviderCategoryValidationException =
                new ProviderCategoryValidationException(
                    message: "ProviderCategory validation error occurred, Please try again.",
                    nullProviderCategoryException);

            //when
            ValueTask<ProviderCategory> modifyProviderCategoryTask =
                this.providerCategoryService.ModifyProviderCategoryAsync(invalidProviderCategory);

            ProviderCategoryValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                     modifyProviderCategoryTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderCategoryAsync(It.IsAny<ProviderCategory>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfProviderCategoryIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidProviderCategory = new ProviderCategory
            {
                Description = invalidText,
                Name = invalidText,

            };

            var invalidProviderCategoryException = new InvalidProviderCategoryException();

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.Id),
                values: "Id is required");

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.Name),
                values: "Text is required");

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.Description),
                values: "Text is required");

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.CreatedDate),
                values: "Date is required");

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(ProviderCategory.CreatedDate)}");

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.CreatedBy),
                values: "Id is required");

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.UpdatedBy),
                values: "Id is required");

            var expectedProviderCategoryValidationException =
                new ProviderCategoryValidationException(invalidProviderCategoryException);

            // when
            ValueTask<ProviderCategory> createProviderCategoryTask =
                this.providerCategoryService.ModifyProviderCategoryAsync(invalidProviderCategory);

            // then
            await Assert.ThrowsAsync<ProviderCategoryValidationException>(() =>
                createProviderCategoryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderCategoryAsync(It.IsAny<ProviderCategory>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            ProviderCategory randomProviderCategory = CreateRandomProviderCategory(dateTime);
            ProviderCategory inputProviderCategory = randomProviderCategory;

            var invalidProviderCategoryException = new InvalidProviderCategoryException(
                message: "Invalid ProviderCategory. Please fix the errors and try again.");

            invalidProviderCategoryException.AddData(
               key: nameof(ProviderCategory.UpdatedDate),
               values: $"Date is the same as {nameof(inputProviderCategory.CreatedDate)}");

            var expectedProviderCategoryValidationException =
                new ProviderCategoryValidationException(
                    message: "ProviderCategory validation error occurred, Please try again.",
                    innerException: invalidProviderCategoryException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<ProviderCategory> modifyProviderCategoryTask =
                this.providerCategoryService.ModifyProviderCategoryAsync(inputProviderCategory);

            ProviderCategoryValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                modifyProviderCategoryTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderCategoryAsync(It.IsAny<ProviderCategory>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderCategory randomProviderCategory = CreateRandomModifyProviderCategory(dateTime);
            ProviderCategory inputProviderCategory = randomProviderCategory;
            inputProviderCategory.UpdatedBy = inputProviderCategory.CreatedBy;
            inputProviderCategory.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidProviderCategoryException = new InvalidProviderCategoryException(
                message: "Invalid ProviderCategory. Please fix the errors and try again.");

            invalidProviderCategoryException.AddData(
                   key: nameof(ProviderCategory.UpdatedDate),
                   values: "Date is not recent");

            var expectedProviderCategoryValidationException =
                new ProviderCategoryValidationException(
                    message: "ProviderCategory validation error occurred, Please try again.",
                    innerException: invalidProviderCategoryException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ProviderCategory> modifyProviderCategoryTask =
                this.providerCategoryService.ModifyProviderCategoryAsync(inputProviderCategory);

            ProviderCategoryValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                modifyProviderCategoryTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderCategoryAsync(It.IsAny<ProviderCategory>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfProviderCategoryDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderCategory randomProviderCategory = CreateRandomProviderCategory(dateTime);
            ProviderCategory nonExistentProviderCategory = randomProviderCategory;
            nonExistentProviderCategory.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            ProviderCategory noProviderCategory = null;
            var notFoundProviderCategoryException = new NotFoundProviderCategoryException(nonExistentProviderCategory.Id);

            var expectedProviderCategoryValidationException =
                new ProviderCategoryValidationException(
                    message: "ProviderCategory validation error occurred, Please try again.",
                    innerException: notFoundProviderCategoryException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderCategoryByIdAsync(nonExistentProviderCategory.Id))
                    .ReturnsAsync(noProviderCategory);

            // when
            ValueTask<ProviderCategory> modifyProviderCategoryTask =
                this.providerCategoryService.ModifyProviderCategoryAsync(nonExistentProviderCategory);

            ProviderCategoryValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                modifyProviderCategoryTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(nonExistentProviderCategory.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderCategoryAsync(It.IsAny<ProviderCategory>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetNegativeRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            ProviderCategory randomProviderCategory = CreateRandomModifyProviderCategory(randomDateTimeOffset);
            ProviderCategory invalidProviderCategory = randomProviderCategory.DeepClone();
            ProviderCategory storageProviderCategory = invalidProviderCategory.DeepClone();
            storageProviderCategory.CreatedDate = storageProviderCategory.CreatedDate.AddMinutes(randomMinutes);
            storageProviderCategory.UpdatedDate = storageProviderCategory.UpdatedDate.AddMinutes(randomMinutes);
            Guid ProviderCategoryId = invalidProviderCategory.Id;
          

            var invalidProviderCategoryException = new InvalidProviderCategoryException(
               message: "Invalid ProviderCategory. Please fix the errors and try again.");

            invalidProviderCategoryException.AddData(
                 key: nameof(ProviderCategory.CreatedDate),
                 values: $"Date is not the same as {nameof(ProviderCategory.CreatedDate)}");

            var expectedProviderCategoryValidationException =
              new ProviderCategoryValidationException(
                  message: "ProviderCategory validation error occurred, Please try again.",
                  innerException: invalidProviderCategoryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderCategoryByIdAsync(ProviderCategoryId))
                    .ReturnsAsync(storageProviderCategory);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ProviderCategory> modifyProviderCategoryTask =
                this.providerCategoryService.ModifyProviderCategoryAsync(invalidProviderCategory);

            ProviderCategoryValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                modifyProviderCategoryTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(invalidProviderCategory.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderCategoryAsync(It.IsAny<ProviderCategory>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int randomPositiveMinutes = GetRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            ProviderCategory randomProviderCategory = CreateRandomModifyProviderCategory(randomDateTimeOffset);
            ProviderCategory invalidProviderCategory = randomProviderCategory.DeepClone();
            ProviderCategory storageProviderCategory = invalidProviderCategory.DeepClone();
            storageProviderCategory.UpdatedDate = storageProviderCategory.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid ProviderCategoryId = invalidProviderCategory.Id;
            invalidProviderCategory.CreatedBy = invalidCreatedBy;

            var invalidProviderCategoryException = new InvalidProviderCategoryException(
                message: "Invalid ProviderCategory. Please fix the errors and try again.");

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.CreatedBy),
                values: $"Id is not the same as {nameof(ProviderCategory.CreatedBy)}");

            var expectedProviderCategoryValidationException =
              new ProviderCategoryValidationException(
                  message: "ProviderCategory validation error occurred, Please try again.",
                  innerException: invalidProviderCategoryException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderCategoryByIdAsync(ProviderCategoryId))
                    .ReturnsAsync(storageProviderCategory);

            // when
            ValueTask<ProviderCategory> modifyProviderCategoryTask =
                this.providerCategoryService.ModifyProviderCategoryAsync(invalidProviderCategory);

            ProviderCategoryValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                modifyProviderCategoryTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(invalidProviderCategory.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderCategoryAsync(It.IsAny<ProviderCategory>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetCurrentDateTime();
            ProviderCategory randomProviderCategory = CreateRandomModifyProviderCategory(randomDate);
            ProviderCategory invalidProviderCategory = randomProviderCategory;
            invalidProviderCategory.UpdatedDate = randomDate;
            ProviderCategory storageProviderCategory = randomProviderCategory.DeepClone();
            Guid ProviderCategoryId = invalidProviderCategory.Id;

            var invalidProviderCategoryException = new InvalidProviderCategoryException(
               message: "Invalid ProviderCategory. Please fix the errors and try again.");

            invalidProviderCategoryException.AddData(
               key: nameof(ProviderCategory.UpdatedDate),
               values: $"Date is the same as {nameof(invalidProviderCategory.UpdatedDate)}");

            var expectedProviderCategoryValidationException =
              new ProviderCategoryValidationException(
                  message: "ProviderCategory validation error occurred, Please try again.",
                  innerException: invalidProviderCategoryException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderCategoryByIdAsync(ProviderCategoryId))
                    .ReturnsAsync(storageProviderCategory);

            // when
            ValueTask<ProviderCategory> modifyProviderCategoryTask =
                this.providerCategoryService.ModifyProviderCategoryAsync(invalidProviderCategory);

            ProviderCategoryValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                modifyProviderCategoryTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(invalidProviderCategory.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderCategoryAsync(It.IsAny<ProviderCategory>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
