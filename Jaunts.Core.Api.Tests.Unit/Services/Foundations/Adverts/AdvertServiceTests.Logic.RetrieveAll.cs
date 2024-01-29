// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Adverts
{
    public partial class AdvertServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllAdverts()
        {
            // given
            IQueryable<Advert> randomAdverts = CreateRandomAdverts();
            IQueryable<Advert> storageAdverts = randomAdverts;
            IQueryable<Advert> expectedAdverts = storageAdverts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAdverts())
                    .Returns(storageAdverts);

            // when
            IQueryable<Advert> actualAdverts =
                this.advertService.RetrieveAllAdverts();

            // then
            actualAdverts.Should().BeEquivalentTo(expectedAdverts);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAdverts(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
