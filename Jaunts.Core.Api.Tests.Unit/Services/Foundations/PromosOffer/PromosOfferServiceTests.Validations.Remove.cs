// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers;
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
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomPromosOfferId = default;
            Guid inputPromosOfferId = randomPromosOfferId;

            var invalidPromosOfferException = new InvalidPromosOffersException(
                message: "Invalid PromosOffer. Please fix the errors and try again.");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.Id),
                values: "Id is required");

            var expectedPromosOffersValidationException =
                new PromosOffersValidationException(
                    message: "PromosOffer validation error occurred, please try again.",
                    innerException: invalidPromosOfferException);

            // when
            ValueTask<PromosOffer> actualPromosOfferTask =
                this.promosOfferService.RemovePromosOfferByIdAsync(inputPromosOfferId);

            PromosOffersValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<PromosOffersValidationException>(
                 actualPromosOfferTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePromosOffersAsync(It.IsAny<PromosOffer>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStoragePromosOfferIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            PromosOffer randomPromosOffer = CreateRandomPromosOffer(dateTime);
            Guid inputPromosOfferId = randomPromosOffer.Id;
            PromosOffer inputPromosOffer = randomPromosOffer;
            PromosOffer nullStoragePromosOffer = null;

            var notFoundPromosOfferException = new NotFoundPromosOffersException(inputPromosOfferId);

            var expectedPromosOffersValidationException =
                new PromosOffersValidationException(
                    message: "PromosOffer validation error occurred, please try again.",
                    innerException: notFoundPromosOfferException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOffersByIdAsync(inputPromosOfferId))
                    .ReturnsAsync(nullStoragePromosOffer);

            // when
            ValueTask<PromosOffer> actualPromosOfferTask =
                this.promosOfferService.RemovePromosOfferByIdAsync(inputPromosOfferId);

            PromosOffersValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<PromosOffersValidationException>(
                 actualPromosOfferTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(inputPromosOfferId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePromosOffersAsync(It.IsAny<PromosOffer>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
