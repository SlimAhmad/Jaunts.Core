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
        public async Task ShouldCreatePromosOfferAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            PromosOffer randomPromosOffer = CreateRandomPromosOffer(dateTime);
            randomPromosOffer.UpdatedBy = randomPromosOffer.CreatedBy;
            randomPromosOffer.UpdatedDate = randomPromosOffer.CreatedDate;
            PromosOffer inputPromosOffer = randomPromosOffer;
            PromosOffer storagePromosOffer = randomPromosOffer;
            PromosOffer expectedPromosOffer = randomPromosOffer;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPromosOfferAsync(inputPromosOffer))
                    .ReturnsAsync(storagePromosOffer);

            // when
            PromosOffer actualPromosOffer =
                await this.promosOfferService.CreatePromosOfferAsync(inputPromosOffer);

            // then
            actualPromosOffer.Should().BeEquivalentTo(expectedPromosOffer);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAsync(inputPromosOffer),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
