// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Amenities;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Amenities
{
    public partial class AmenityServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
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

            //when
            ValueTask<Amenity> retrieveAmenityByIdTask =
                this.amenityService.RetrieveAmenityByIdAsync(inputAmenityId);

            AmenityValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<AmenityValidationException>(
                 retrieveAmenityByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageAmenityIsNullAndLogItAsync()
        {
            //given
            Guid randomAmenityId = Guid.NewGuid();
            Guid someAmenityId = randomAmenityId;
            Amenity invalidStorageAmenity = null;
            var notFoundAmenityException = new NotFoundAmenityException(
                message: $"Couldn't find Amenity with id: {someAmenityId}.");

            var expectedAmenityValidationException =
                new AmenityValidationException(
                    message: "Amenity validation error occurred, please try again.",
                    innerException: notFoundAmenityException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageAmenity);

            //when
            ValueTask<Amenity> retrieveAmenityByIdTask =
                this.amenityService.RetrieveAmenityByIdAsync(someAmenityId);

            AmenityValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<AmenityValidationException>(
                 retrieveAmenityByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}