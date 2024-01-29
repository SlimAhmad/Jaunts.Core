// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Adverts
{
    public partial class AdvertServiceTests
    {
        [Fact]
        public async Task ShouldCreateAdvertAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            Advert randomAdvert = CreateRandomAdvert(dateTime);
            randomAdvert.UpdatedBy = randomAdvert.CreatedBy;
            randomAdvert.UpdatedDate = randomAdvert.CreatedDate;
            Advert inputAdvert = randomAdvert;
            Advert storageAdvert = randomAdvert;
            Advert expectedAdvert = randomAdvert;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAdvertAsync(inputAdvert))
                    .ReturnsAsync(storageAdvert);

            // when
            Advert actualAdvert =
                await this.advertService.CreateAdvertAsync(inputAdvert);

            // then
            actualAdvert.Should().BeEquivalentTo(expectedAdvert);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAsync(inputAdvert),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
