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
        public async Task ShouldDeleteAdvertAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Advert randomAdvert = CreateRandomAdvert(dateTime);
            Guid inputAdvertId = randomAdvert.Id;
            Advert inputAdvert = randomAdvert;
            Advert storageAdvert = randomAdvert;
            Advert expectedAdvert = randomAdvert;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertByIdAsync(inputAdvertId))
                    .ReturnsAsync(inputAdvert);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAdvertAsync(inputAdvert))
                    .ReturnsAsync(storageAdvert);

            // when
            Advert actualAdvert =
                await this.advertService.RemoveAdvertByIdAsync(inputAdvertId);

            // then
            actualAdvert.Should().BeEquivalentTo(expectedAdvert);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(inputAdvertId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAdvertAsync(inputAdvert),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
