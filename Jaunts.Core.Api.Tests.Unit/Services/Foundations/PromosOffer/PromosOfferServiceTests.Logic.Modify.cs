// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyPromosOfferAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            PromosOffer randomPromosOffer = CreateRandomPromosOffer(randomInputDate);
            PromosOffer inputPromosOffer = randomPromosOffer;
            PromosOffer afterUpdateStoragePromosOffer = inputPromosOffer;
            PromosOffer expectedPromosOffer = afterUpdateStoragePromosOffer;
            PromosOffer beforeUpdateStoragePromosOffer = randomPromosOffer.DeepClone();
            inputPromosOffer.UpdatedDate = randomDate;
            Guid PromosOfferId = inputPromosOffer.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOffersByIdAsync(PromosOfferId))
                    .ReturnsAsync(beforeUpdateStoragePromosOffer);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdatePromosOffersAsync(inputPromosOffer))
                    .ReturnsAsync(afterUpdateStoragePromosOffer);

            // when
            PromosOffer actualPromosOffer =
                await this.promosOfferService.ModifyPromosOfferAsync(inputPromosOffer);

            // then
            actualPromosOffer.Should().BeEquivalentTo(expectedPromosOffer);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(PromosOfferId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePromosOffersAsync(inputPromosOffer),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
