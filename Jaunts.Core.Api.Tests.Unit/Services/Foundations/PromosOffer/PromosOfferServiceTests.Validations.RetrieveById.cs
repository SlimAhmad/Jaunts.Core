// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Hosting;
using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOffers
{
    public partial class PromosOfferServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
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

            //when
            ValueTask<PromosOffer> retrievePromosOfferByIdTask =
                this.promosOfferService.RetrievePromosOfferByIdAsync(inputPromosOfferId);

            PromosOffersValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<PromosOffersValidationException>(
                 retrievePromosOfferByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStoragePromosOfferIsNullAndLogItAsync()
        {
            //given
            Guid randomPromosOfferId = Guid.NewGuid();
            Guid somePromosOfferId = randomPromosOfferId;
            PromosOffer invalidStoragePromosOffer = null;
            var notFoundPromosOfferException = new NotFoundPromosOffersException(
                message: $"Couldn't find PromosOffer with id: {somePromosOfferId}.");

            var expectedPromosOffersValidationException =
                new PromosOffersValidationException(
                    message: "PromosOffer validation error occurred, please try again.",
                    innerException: notFoundPromosOfferException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOffersByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStoragePromosOffer);

            //when
            ValueTask<PromosOffer> retrievePromosOfferByIdTask =
                this.promosOfferService.RetrievePromosOfferByIdAsync(somePromosOfferId);

            PromosOffersValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<PromosOffersValidationException>(
                 retrievePromosOfferByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}