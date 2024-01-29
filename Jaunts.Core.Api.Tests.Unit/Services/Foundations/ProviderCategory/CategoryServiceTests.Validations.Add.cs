// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
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
        public async void ShouldThrowValidationExceptionOnCreateWhenProviderCategoryIsNullAndLogItAsync()
        {
            // given
            ProviderCategory randomProviderCategory = null;
            ProviderCategory nullProviderCategory = randomProviderCategory;

            var nullProviderCategoryException = new NullProviderCategoryException(
                message: "The ProviderCategory is null.");

            var expectedProviderCategoryValidationException =
                new ProviderCategoryValidationException(
                    message: "ProviderCategory validation error occurred, Please try again.",
                    innerException: nullProviderCategoryException);

            // when
            ValueTask<ProviderCategory> createProviderCategoryTask =
                this.providerCategoryService.CreateProviderCategoryAsync(nullProviderCategory);

             ProviderCategoryValidationException actualProviderCategoryDependencyValidationException =
             await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                 createProviderCategoryTask.AsTask);

            // then
            actualProviderCategoryDependencyValidationException.Should().BeEquivalentTo(
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenProviderCategoryIsInvalidAndLogItAsync(
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
                key: nameof(ProviderCategory.CreatedBy),
                values: "Id is required");

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.UpdatedBy),
                values: "Id is required");

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.CreatedDate),
                values: "Date is required");

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.UpdatedDate),
                values: "Date is required");


            var expectedProviderCategoryValidationException =
                new ProviderCategoryValidationException(
                    message: "ProviderCategory validation error occurred, Please try again.",
                    innerException: invalidProviderCategoryException);

            // when
            ValueTask<ProviderCategory> createProviderCategoryTask =
                this.providerCategoryService.CreateProviderCategoryAsync(invalidProviderCategory);

             ProviderCategoryValidationException actualProviderCategoryDependencyValidationException =
             await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                 createProviderCategoryTask.AsTask);

            // then
            actualProviderCategoryDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            ProviderCategory randomProviderCategory = CreateRandomProviderCategory(dateTime);
            ProviderCategory inputProviderCategory = randomProviderCategory;
            inputProviderCategory.UpdatedBy = Guid.NewGuid();

            var invalidProviderCategoryException = new InvalidProviderCategoryException();

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.UpdatedBy),
                values: $"Id is not the same as {nameof(ProviderCategory.CreatedBy)}");

            var expectedProviderCategoryValidationException =
                new ProviderCategoryValidationException(
                    message: "ProviderCategory validation error occurred, Please try again.",
                    innerException: invalidProviderCategoryException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ProviderCategory> createProviderCategoryTask =
                this.providerCategoryService.CreateProviderCategoryAsync(inputProviderCategory);

             ProviderCategoryValidationException actualProviderCategoryDependencyValidationException =
             await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                 createProviderCategoryTask.AsTask);

            // then
            actualProviderCategoryDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderCategory randomProviderCategory = CreateRandomProviderCategory(dateTime);
            ProviderCategory inputProviderCategory = randomProviderCategory;
            inputProviderCategory.UpdatedBy = randomProviderCategory.CreatedBy;
            inputProviderCategory.UpdatedDate = GetRandomDateTime();

            var invalidProviderCategoryException = new InvalidProviderCategoryException();

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.UpdatedDate),
                values: $"Date is not the same as {nameof(ProviderCategory.CreatedDate)}");

            var expectedProviderCategoryValidationException =
                new ProviderCategoryValidationException(
                    message: "ProviderCategory validation error occurred, Please try again.",
                    innerException: invalidProviderCategoryException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<ProviderCategory> createProviderCategoryTask =
                this.providerCategoryService.CreateProviderCategoryAsync(inputProviderCategory);

             ProviderCategoryValidationException actualProviderCategoryDependencyValidationException =
             await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                 createProviderCategoryTask.AsTask);

            // then
            actualProviderCategoryDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderCategory randomProviderCategory = CreateRandomProviderCategory(dateTime);
            ProviderCategory inputProviderCategory = randomProviderCategory;
            inputProviderCategory.UpdatedBy = inputProviderCategory.CreatedBy;
            inputProviderCategory.CreatedDate = dateTime.AddMinutes(minutes);
            inputProviderCategory.UpdatedDate = inputProviderCategory.CreatedDate;

            var invalidProviderCategoryException = new InvalidProviderCategoryException();

            invalidProviderCategoryException.AddData(
                key: nameof(ProviderCategory.CreatedDate),
                values: $"Date is not recent");

            var expectedProviderCategoryValidationException =
                new ProviderCategoryValidationException(
                    message: "ProviderCategory validation error occurred, Please try again.",
                    innerException: invalidProviderCategoryException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ProviderCategory> createProviderCategoryTask =
                this.providerCategoryService.CreateProviderCategoryAsync(inputProviderCategory);

             ProviderCategoryValidationException actualProviderCategoryDependencyValidationException =
             await Assert.ThrowsAsync<ProviderCategoryValidationException>(
                 createProviderCategoryTask.AsTask);

            // then
            actualProviderCategoryDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenProviderCategoryAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderCategory randomProviderCategory = CreateRandomProviderCategory(dateTime);
            ProviderCategory alreadyExistsProviderCategory = randomProviderCategory;
            alreadyExistsProviderCategory.UpdatedBy = alreadyExistsProviderCategory.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsProviderCategoryException =
                new AlreadyExistsProviderCategoryException(
                   message: "ProviderCategory with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedProviderCategoryValidationException =
                new ProviderCategoryDependencyValidationException(
                    message: "ProviderCategory dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsProviderCategoryException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProviderCategoryAsync(alreadyExistsProviderCategory))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ProviderCategory> createProviderCategoryTask =
                this.providerCategoryService.CreateProviderCategoryAsync(alreadyExistsProviderCategory);

             ProviderCategoryDependencyValidationException actualProviderCategoryDependencyValidationException =
             await Assert.ThrowsAsync<ProviderCategoryDependencyValidationException>(
                 createProviderCategoryTask.AsTask);

            // then
            actualProviderCategoryDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderCategoryValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedProviderCategoryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderCategoryAsync(alreadyExistsProviderCategory),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
