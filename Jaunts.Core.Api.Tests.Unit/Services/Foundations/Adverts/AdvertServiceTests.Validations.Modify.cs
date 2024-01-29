// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Adverts
{
    public partial class AdvertServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenAdvertIsNullAndLogItAsync()
        {
            //given
            Advert invalidAdvert = null;
            var nullAdvertException = new NullAdvertException();

            var expectedAdvertValidationException =
                new AdvertValidationException(
                    message: "Advert validation error occurred, please try again.",
                    nullAdvertException);

            //when
            ValueTask<Advert> modifyAdvertTask =
                this.advertService.ModifyAdvertAsync(invalidAdvert);

            AdvertValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<AdvertValidationException>(
                     modifyAdvertTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAdvertAsync(It.IsAny<Advert>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfAdvertIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidAdvert = new Advert
            {
                Description = invalidText,
                Placement =  AdvertsPlacement.Banner,
                Status = AdvertStatus.Approved,
            };

            var invalidAdvertException = new InvalidAdvertException();

            invalidAdvertException.AddData(
                key: nameof(Advert.Id),
                values: "Id is required");

            invalidAdvertException.AddData(
               key: nameof(Advert.TransactionFeeId),
               values: "Id is required");

            invalidAdvertException.AddData(
               key: nameof(Advert.ProviderId),
               values: "Id is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.Description),
                values: "Text is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.Placement),
                values: "Text is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.Status),
                values: "Text is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.StartDate),
                values: "Date is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.EndDate),
                values: "Date is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.CreatedDate),
                values: "Date is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(Advert.CreatedDate)}");

            invalidAdvertException.AddData(
                key: nameof(Advert.CreatedBy),
                values: "Id is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.UpdatedBy),
                values: "Id is required");

            var expectedAdvertValidationException =
                new AdvertValidationException(invalidAdvertException);

            // when
            ValueTask<Advert> createAdvertTask =
                this.advertService.ModifyAdvertAsync(invalidAdvert);

            // then
            await Assert.ThrowsAsync<AdvertValidationException>(() =>
                createAdvertTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAsync(It.IsAny<Advert>()),
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
            Advert randomAdvert = CreateRandomAdvert(dateTime);
            Advert inputAdvert = randomAdvert;

            var invalidAdvertException = new InvalidAdvertException(
                message: "Invalid advert. Please fix the errors and try again.");

            invalidAdvertException.AddData(
               key: nameof(Advert.UpdatedDate),
               values: $"Date is the same as {nameof(inputAdvert.CreatedDate)}");

            var expectedAdvertValidationException =
                new AdvertValidationException(
                    message: "Advert validation error occurred, please try again.",
                    innerException: invalidAdvertException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Advert> modifyAdvertTask =
                this.advertService.ModifyAdvertAsync(inputAdvert);

            AdvertValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<AdvertValidationException>(
                modifyAdvertTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAdvertAsync(It.IsAny<Advert>()),
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
            Advert randomAdvert = CreateRandomModifyAdvert(dateTime);
            Advert inputAdvert = randomAdvert;
            inputAdvert.UpdatedBy = inputAdvert.CreatedBy;
            inputAdvert.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidAdvertException = new InvalidAdvertException(
                message: "Invalid advert. Please fix the errors and try again.");

            invalidAdvertException.AddData(
                   key: nameof(Advert.UpdatedDate),
                   values: "Date is not recent");

            var expectedAdvertValidationException =
                new AdvertValidationException(
                    message: "Advert validation error occurred, please try again.",
                    innerException: invalidAdvertException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Advert> modifyAdvertTask =
                this.advertService.ModifyAdvertAsync(inputAdvert);

            AdvertValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<AdvertValidationException>(
                modifyAdvertTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAdvertAsync(It.IsAny<Advert>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAdvertDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Advert randomAdvert = CreateRandomAdvert(dateTime);
            Advert nonExistentAdvert = randomAdvert;
            nonExistentAdvert.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Advert noAdvert = null;
            var notFoundAdvertException = new NotFoundAdvertException(nonExistentAdvert.Id);

            var expectedAdvertValidationException =
                new AdvertValidationException(
                    message: "Advert validation error occurred, please try again.",
                    innerException: notFoundAdvertException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertByIdAsync(nonExistentAdvert.Id))
                    .ReturnsAsync(noAdvert);

            // when
            ValueTask<Advert> modifyAdvertTask =
                this.advertService.ModifyAdvertAsync(nonExistentAdvert);

            AdvertValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<AdvertValidationException>(
                modifyAdvertTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(nonExistentAdvert.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAdvertAsync(It.IsAny<Advert>()),
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
            Advert randomAdvert = CreateRandomModifyAdvert(randomDateTimeOffset);
            Advert invalidAdvert = randomAdvert.DeepClone();
            Advert storageAdvert = invalidAdvert.DeepClone();
            storageAdvert.CreatedDate = storageAdvert.CreatedDate.AddMinutes(randomMinutes);
            storageAdvert.UpdatedDate = storageAdvert.UpdatedDate.AddMinutes(randomMinutes);
            Guid AdvertId = invalidAdvert.Id;
          

            var invalidAdvertException = new InvalidAdvertException(
               message: "Invalid advert. Please fix the errors and try again.");

            invalidAdvertException.AddData(
                 key: nameof(Advert.CreatedDate),
                 values: $"Date is not the same as {nameof(Advert.CreatedDate)}");

            var expectedAdvertValidationException =
              new AdvertValidationException(
                  message: "Advert validation error occurred, please try again.",
                  innerException: invalidAdvertException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertByIdAsync(AdvertId))
                    .ReturnsAsync(storageAdvert);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Advert> modifyAdvertTask =
                this.advertService.ModifyAdvertAsync(invalidAdvert);

            AdvertValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<AdvertValidationException>(
                modifyAdvertTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(invalidAdvert.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAdvertAsync(It.IsAny<Advert>()),
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
            Advert randomAdvert = CreateRandomModifyAdvert(randomDateTimeOffset);
            Advert invalidAdvert = randomAdvert.DeepClone();
            Advert storageAdvert = invalidAdvert.DeepClone();
            storageAdvert.UpdatedDate = storageAdvert.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid AdvertId = invalidAdvert.Id;
            invalidAdvert.CreatedBy = invalidCreatedBy;

            var invalidAdvertException = new InvalidAdvertException(
                message: "Invalid advert. Please fix the errors and try again.");

            invalidAdvertException.AddData(
                key: nameof(Advert.CreatedBy),
                values: $"Id is not the same as {nameof(Advert.CreatedBy)}");

            var expectedAdvertValidationException =
              new AdvertValidationException(
                  message: "Advert validation error occurred, please try again.",
                  innerException: invalidAdvertException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertByIdAsync(AdvertId))
                    .ReturnsAsync(storageAdvert);

            // when
            ValueTask<Advert> modifyAdvertTask =
                this.advertService.ModifyAdvertAsync(invalidAdvert);

            AdvertValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<AdvertValidationException>(
                modifyAdvertTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(invalidAdvert.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAdvertAsync(It.IsAny<Advert>()),
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
            Advert randomAdvert = CreateRandomModifyAdvert(randomDate);
            Advert invalidAdvert = randomAdvert;
            invalidAdvert.UpdatedDate = randomDate;
            Advert storageAdvert = randomAdvert.DeepClone();
            Guid AdvertId = invalidAdvert.Id;

            var invalidAdvertException = new InvalidAdvertException(
               message: "Invalid advert. Please fix the errors and try again.");

            invalidAdvertException.AddData(
               key: nameof(Advert.UpdatedDate),
               values: $"Date is the same as {nameof(invalidAdvert.UpdatedDate)}");

            var expectedAdvertValidationException =
              new AdvertValidationException(
                  message: "Advert validation error occurred, please try again.",
                  innerException: invalidAdvertException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertByIdAsync(AdvertId))
                    .ReturnsAsync(storageAdvert);

            // when
            ValueTask<Advert> modifyAdvertTask =
                this.advertService.ModifyAdvertAsync(invalidAdvert);

            AdvertValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<AdvertValidationException>(
                modifyAdvertTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(invalidAdvert.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAdvertAsync(It.IsAny<Advert>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
