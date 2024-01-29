// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Amenities;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Amenities
{
    public partial class AmenityServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllAmenities()
        {
            // given
            IQueryable<Amenity> randomAmenities = CreateRandomAmenities();
            IQueryable<Amenity> storageAmenities = randomAmenities;
            IQueryable<Amenity> expectedAmenities = storageAmenities;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAmenities())
                    .Returns(storageAmenities);

            // when
            IQueryable<Amenity> actualAmenities =
                this.amenityService.RetrieveAllAmenities();

            // then
            actualAmenities.Should().BeEquivalentTo(expectedAmenities);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAmenities(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
