// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
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
        public async Task ShouldCreateAmenityAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            Amenity randomAmenity = CreateRandomAmenity(dateTime);
            randomAmenity.UpdatedBy = randomAmenity.CreatedBy;
            randomAmenity.UpdatedDate = randomAmenity.CreatedDate;
            Amenity inputAmenity = randomAmenity;
            Amenity storageAmenity = randomAmenity;
            Amenity expectedAmenity = randomAmenity;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAmenityAsync(inputAmenity))
                    .ReturnsAsync(storageAmenity);

            // when
            Amenity actualAmenity =
                await this.amenityService.CreateAmenityAsync(inputAmenity);

            // then
            actualAmenity.Should().BeEquivalentTo(expectedAmenity);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAmenityAsync(inputAmenity),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
