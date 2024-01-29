// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOffers
{
    public partial class PromosOfferServiceTests
    {
        [Fact]
        public async Task ShouldDeletePromosOfferAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            PromosOffer randomPromosOffer = CreateRandomPromosOffer(dateTime);
            Guid inputPromosOfferId = randomPromosOffer.Id;
            PromosOffer inputPromosOffer = randomPromosOffer;
            PromosOffer storagePromosOffer = randomPromosOffer;
            PromosOffer expectedPromosOffer = randomPromosOffer;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOffersByIdAsync(inputPromosOfferId))
                    .ReturnsAsync(inputPromosOffer);

            this.storageBrokerMock.Setup(broker =>
                broker.DeletePromosOffersAsync(inputPromosOffer))
                    .ReturnsAsync(storagePromosOffer);

            // when
            PromosOffer actualPromosOffer =
                await this.promosOfferService.RemovePromosOfferByIdAsync(inputPromosOfferId);

            // then
            actualPromosOffer.Should().BeEquivalentTo(expectedPromosOffer);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(inputPromosOfferId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePromosOffersAsync(inputPromosOffer),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
