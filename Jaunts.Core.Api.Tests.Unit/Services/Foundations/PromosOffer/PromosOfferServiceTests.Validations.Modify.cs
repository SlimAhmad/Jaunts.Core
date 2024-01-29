// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOffers
{
    public partial class PromosOfferServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenPromosOfferIsNullAndLogItAsync()
        {
            //given
            PromosOffer invalidPromosOffer = null;
            var nullPromosOfferException = new NullPromosOffersException();

            var expectedPromosOffersValidationException =
                new PromosOffersValidationException(
                    message: "PromosOffer validation error occurred, please try again.",
                    nullPromosOfferException);

            //when
            ValueTask<PromosOffer> modifyPromosOfferTask =
                this.promosOfferService.ModifyPromosOfferAsync(invalidPromosOffer);

            PromosOffersValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<PromosOffersValidationException>(
                     modifyPromosOfferTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePromosOffersAsync(It.IsAny<PromosOffer>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfPromosOfferIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidPromosOffer = new PromosOffer
            {
                Description = invalidText,
                CodeOrName = invalidText

            };

            var invalidPromosOfferException = new InvalidPromosOffersException();

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.Id),
                values: "Id is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.ProviderId),
                values: "Id is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.CodeOrName),
                values: "Text is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.Description),
                values: "Text is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.StartDate),
                values: "Date is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.EndDate),
                values: "Date is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.CreatedDate),
                values: "Date is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.UpdatedDate),
                "Date is required",
                $"Date is the same as {nameof(PromosOffer.CreatedDate)}");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.CreatedBy),
                values: "Id is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.UpdatedBy),
                values: "Id is required");

            var expectedPromosOffersValidationException =
                new PromosOffersValidationException(invalidPromosOfferException);

            // when
            ValueTask<PromosOffer> createPromosOfferTask =
                this.promosOfferService.ModifyPromosOfferAsync(invalidPromosOffer);

            // then
            await Assert.ThrowsAsync<PromosOffersValidationException>(() =>
                createPromosOfferTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAsync(It.IsAny<PromosOffer>()),
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
            PromosOffer randomPromosOffer = CreateRandomPromosOffer(dateTime);
            PromosOffer inputPromosOffer = randomPromosOffer;

            var invalidPromosOfferException = new InvalidPromosOffersException(
                message: "Invalid PromosOffer. Please fix the errors and try again.");

            invalidPromosOfferException.AddData(
               key: nameof(PromosOffer.UpdatedDate),
               values: $"Date is the same as {nameof(inputPromosOffer.CreatedDate)}");

            var expectedPromosOffersValidationException =
                new PromosOffersValidationException(
                    message: "PromosOffer validation error occurred, please try again.",
                    innerException: invalidPromosOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<PromosOffer> modifyPromosOfferTask =
                this.promosOfferService.ModifyPromosOfferAsync(inputPromosOffer);

            PromosOffersValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<PromosOffersValidationException>(
                modifyPromosOfferTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePromosOffersAsync(It.IsAny<PromosOffer>()),
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
            PromosOffer randomPromosOffer = CreateRandomModifyPromosOffer(dateTime);
            PromosOffer inputPromosOffer = randomPromosOffer;
            inputPromosOffer.UpdatedBy = inputPromosOffer.CreatedBy;
            inputPromosOffer.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidPromosOfferException = new InvalidPromosOffersException(
                message: "Invalid PromosOffer. Please fix the errors and try again.");

            invalidPromosOfferException.AddData(
                   key: nameof(PromosOffer.UpdatedDate),
                   values: "Date is not recent");

            var expectedPromosOffersValidationException =
                new PromosOffersValidationException(
                    message: "PromosOffer validation error occurred, please try again.",
                    innerException: invalidPromosOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<PromosOffer> modifyPromosOfferTask =
                this.promosOfferService.ModifyPromosOfferAsync(inputPromosOffer);

            PromosOffersValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<PromosOffersValidationException>(
                modifyPromosOfferTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePromosOffersAsync(It.IsAny<PromosOffer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfPromosOfferDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            PromosOffer randomPromosOffer = CreateRandomPromosOffer(dateTime);
            PromosOffer nonExistentPromosOffer = randomPromosOffer;
            nonExistentPromosOffer.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            PromosOffer noPromosOffer = null;
            var notFoundPromosOfferException = new NotFoundPromosOffersException(nonExistentPromosOffer.Id);

            var expectedPromosOffersValidationException =
                new PromosOffersValidationException(
                    message: "PromosOffer validation error occurred, please try again.",
                    innerException: notFoundPromosOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOffersByIdAsync(nonExistentPromosOffer.Id))
                    .ReturnsAsync(noPromosOffer);

            // when
            ValueTask<PromosOffer> modifyPromosOfferTask =
                this.promosOfferService.ModifyPromosOfferAsync(nonExistentPromosOffer);

            PromosOffersValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<PromosOffersValidationException>(
                modifyPromosOfferTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(nonExistentPromosOffer.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePromosOffersAsync(It.IsAny<PromosOffer>()),
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
            PromosOffer randomPromosOffer = CreateRandomModifyPromosOffer(randomDateTimeOffset);
            PromosOffer invalidPromosOffer = randomPromosOffer.DeepClone();
            PromosOffer storagePromosOffer = invalidPromosOffer.DeepClone();
            storagePromosOffer.CreatedDate = storagePromosOffer.CreatedDate.AddMinutes(randomMinutes);
            storagePromosOffer.UpdatedDate = storagePromosOffer.UpdatedDate.AddMinutes(randomMinutes);
            Guid PromosOfferId = invalidPromosOffer.Id;
          

            var invalidPromosOfferException = new InvalidPromosOffersException(
               message: "Invalid PromosOffer. Please fix the errors and try again.");

            invalidPromosOfferException.AddData(
                 key: nameof(PromosOffer.CreatedDate),
                 values: $"Date is not the same as {nameof(PromosOffer.CreatedDate)}");

            var expectedPromosOffersValidationException =
              new PromosOffersValidationException(
                  message: "PromosOffer validation error occurred, please try again.",
                  innerException: invalidPromosOfferException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOffersByIdAsync(PromosOfferId))
                    .ReturnsAsync(storagePromosOffer);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<PromosOffer> modifyPromosOfferTask =
                this.promosOfferService.ModifyPromosOfferAsync(invalidPromosOffer);

            PromosOffersValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<PromosOffersValidationException>(
                modifyPromosOfferTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(invalidPromosOffer.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePromosOffersAsync(It.IsAny<PromosOffer>()),
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
            PromosOffer randomPromosOffer = CreateRandomModifyPromosOffer(randomDateTimeOffset);
            PromosOffer invalidPromosOffer = randomPromosOffer.DeepClone();
            PromosOffer storagePromosOffer = invalidPromosOffer.DeepClone();
            storagePromosOffer.UpdatedDate = storagePromosOffer.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid PromosOfferId = invalidPromosOffer.Id;
            invalidPromosOffer.CreatedBy = invalidCreatedBy;

            var invalidPromosOfferException = new InvalidPromosOffersException(
                message: "Invalid PromosOffer. Please fix the errors and try again.");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.CreatedBy),
                values: $"Id is not the same as {nameof(PromosOffer.CreatedBy)}");

            var expectedPromosOffersValidationException =
              new PromosOffersValidationException(
                  message: "PromosOffer validation error occurred, please try again.",
                  innerException: invalidPromosOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOffersByIdAsync(PromosOfferId))
                    .ReturnsAsync(storagePromosOffer);

            // when
            ValueTask<PromosOffer> modifyPromosOfferTask =
                this.promosOfferService.ModifyPromosOfferAsync(invalidPromosOffer);

            PromosOffersValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<PromosOffersValidationException>(
                modifyPromosOfferTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(invalidPromosOffer.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePromosOffersAsync(It.IsAny<PromosOffer>()),
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
            PromosOffer randomPromosOffer = CreateRandomModifyPromosOffer(randomDate);
            PromosOffer invalidPromosOffer = randomPromosOffer;
            invalidPromosOffer.UpdatedDate = randomDate;
            PromosOffer storagePromosOffer = randomPromosOffer.DeepClone();
            Guid PromosOfferId = invalidPromosOffer.Id;

            var invalidPromosOfferException = new InvalidPromosOffersException(
               message: "Invalid PromosOffer. Please fix the errors and try again.");

            invalidPromosOfferException.AddData(
               key: nameof(PromosOffer.UpdatedDate),
               values: $"Date is the same as {nameof(invalidPromosOffer.UpdatedDate)}");

            var expectedPromosOffersValidationException =
              new PromosOffersValidationException(
                  message: "PromosOffer validation error occurred, please try again.",
                  innerException: invalidPromosOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOffersByIdAsync(PromosOfferId))
                    .ReturnsAsync(storagePromosOffer);

            // when
            ValueTask<PromosOffer> modifyPromosOfferTask =
                this.promosOfferService.ModifyPromosOfferAsync(invalidPromosOffer);

            PromosOffersValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<PromosOffersValidationException>(
                modifyPromosOfferTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(invalidPromosOffer.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePromosOffersAsync(It.IsAny<PromosOffer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
