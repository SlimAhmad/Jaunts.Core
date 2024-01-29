// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Amenities;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Amenities
{
    public partial class AmenityServiceTests
    {
        [Fact]
        public async Task ShouldModifyAmenityAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Amenity randomAmenity = CreateRandomAmenity(randomInputDate);
            Amenity inputAmenity = randomAmenity;
            Amenity afterUpdateStorageAmenity = inputAmenity;
            Amenity expectedAmenity = afterUpdateStorageAmenity;
            Amenity beforeUpdateStorageAmenity = randomAmenity.DeepClone();
            inputAmenity.UpdatedDate = randomDate;
            Guid AmenityId = inputAmenity.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAmenityByIdAsync(AmenityId))
                    .ReturnsAsync(beforeUpdateStorageAmenity);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAmenityAsync(inputAmenity))
                    .ReturnsAsync(afterUpdateStorageAmenity);

            // when
            Amenity actualAmenity =
                await this.amenityService.ModifyAmenityAsync(inputAmenity);

            // then
            actualAmenity.Should().BeEquivalentTo(expectedAmenity);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(AmenityId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAmenityAsync(inputAmenity),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
