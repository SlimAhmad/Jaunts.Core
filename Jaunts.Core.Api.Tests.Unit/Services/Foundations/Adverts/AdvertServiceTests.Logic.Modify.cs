// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyAdvertAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Advert randomAdvert = CreateRandomAdvert(randomInputDate);
            Advert inputAdvert = randomAdvert;
            Advert afterUpdateStorageAdvert = inputAdvert;
            Advert expectedAdvert = afterUpdateStorageAdvert;
            Advert beforeUpdateStorageAdvert = randomAdvert.DeepClone();
            inputAdvert.UpdatedDate = randomDate;
            Guid AdvertId = inputAdvert.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertByIdAsync(AdvertId))
                    .ReturnsAsync(beforeUpdateStorageAdvert);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAdvertAsync(inputAdvert))
                    .ReturnsAsync(afterUpdateStorageAdvert);

            // when
            Advert actualAdvert =
                await this.advertService.ModifyAdvertAsync(inputAdvert);

            // then
            actualAdvert.Should().BeEquivalentTo(expectedAdvert);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(AdvertId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAdvertAsync(inputAdvert),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
