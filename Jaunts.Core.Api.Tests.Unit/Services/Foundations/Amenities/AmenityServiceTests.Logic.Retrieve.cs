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
        async Task ShouldRetrieveAmenityById()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Amenity randomAmenity = CreateRandomAmenity(dateTime);
            Guid inputAmenityId = randomAmenity.Id;
            Amenity inputAmenity = randomAmenity;
            Amenity expectedAmenity = randomAmenity;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAmenityByIdAsync(inputAmenityId))
                    .ReturnsAsync(inputAmenity);

            // when
            Amenity actualAmenity =
                await this.amenityService.RetrieveAmenityByIdAsync(inputAmenityId);

            // then
            actualAmenity.Should().BeEquivalentTo(expectedAmenity);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(inputAmenityId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
