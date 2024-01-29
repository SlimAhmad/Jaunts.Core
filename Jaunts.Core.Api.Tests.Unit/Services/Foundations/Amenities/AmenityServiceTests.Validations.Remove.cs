// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Amenities;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Amenities
{
    public partial class AmenityServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAmenityId = default;
            Guid inputAmenityId = randomAmenityId;

            var invalidAmenityException = new InvalidAmenityException(
                message: "Invalid Amenity. Please fix the errors and try again.");

            invalidAmenityException.AddData(
                key: nameof(Amenity.Id),
                values: "Id is required");

            var expectedAmenityValidationException =
                new AmenityValidationException(
                    message: "Amenity validation error occurred, please try again.",
                    innerException: invalidAmenityException);

            // when
            ValueTask<Amenity> actualAmenityTask =
                this.amenityService.RemoveAmenityByIdAsync(inputAmenityId);

            AmenityValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<AmenityValidationException>(
                 actualAmenityTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAmenityAsync(It.IsAny<Amenity>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageAmenityIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Amenity randomAmenity = CreateRandomAmenity(dateTime);
            Guid inputAmenityId = randomAmenity.Id;
            Amenity inputAmenity = randomAmenity;
            Amenity nullStorageAmenity = null;

            var notFoundAmenityException = new NotFoundAmenityException(inputAmenityId);

            var expectedAmenityValidationException =
                new AmenityValidationException(
                    message: "Amenity validation error occurred, please try again.",
                    innerException: notFoundAmenityException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAmenityByIdAsync(inputAmenityId))
                    .ReturnsAsync(nullStorageAmenity);

            // when
            ValueTask<Amenity> actualAmenityTask =
                this.amenityService.RemoveAmenityByIdAsync(inputAmenityId);

            AmenityValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<AmenityValidationException>(
                 actualAmenityTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(inputAmenityId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAmenityAsync(It.IsAny<Amenity>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
