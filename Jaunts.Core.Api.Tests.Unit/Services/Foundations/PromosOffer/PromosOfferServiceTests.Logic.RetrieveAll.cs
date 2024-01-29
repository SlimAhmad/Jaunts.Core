// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOffers
{
    public partial class PromosOfferServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllPromosOffers()
        {
            // given
            IQueryable<PromosOffer> randomPromosOffers = CreateRandomPromosOffers();
            IQueryable<PromosOffer> storagePromosOffers = randomPromosOffers;
            IQueryable<PromosOffer> expectedPromosOffers = storagePromosOffers;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPromosOffers())
                    .Returns(storagePromosOffers);

            // when
            IQueryable<PromosOffer> actualPromosOffers =
                this.promosOfferService.RetrieveAllPromosOffers();

            // then
            actualPromosOffers.Should().BeEquivalentTo(expectedPromosOffers);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPromosOffers(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
