// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldThrowValidationExceptionOnModifyWhenAmenityIsNullAndLogItAsync()
        {
            //given
            Amenity invalidAmenity = null;
            var nullAmenityException = new NullAmenityException();

            var expectedAmenityValidationException =
                new AmenityValidationException(
                    message: "Amenity validation error occurred, please try again.",
                    nullAmenityException);

            //when
            ValueTask<Amenity> modifyAmenityTask =
                this.amenityService.ModifyAmenityAsync(invalidAmenity);

            AmenityValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<AmenityValidationException>(
                     modifyAmenityTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAmenityAsync(It.IsAny<Amenity>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfAmenityIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidAmenity = new Amenity
            {
                Description = invalidText,
                Name =  invalidText,

            };

            var invalidAmenityException = new InvalidAmenityException();

            invalidAmenityException.AddData(
                key: nameof(Amenity.Id),
                values: "Id is required");

            invalidAmenityException.AddData(
               key: nameof(Amenity.ShortLetId),
               values: "Id is required");

            invalidAmenityException.AddData(
                key: nameof(Amenity.Description),
                values: "Text is required");

            invalidAmenityException.AddData(
                key: nameof(Amenity.Name),
                values: "Text is required");

            invalidAmenityException.AddData(
                key: nameof(Amenity.CreatedDate),
                values: "Date is required");

            invalidAmenityException.AddData(
                key: nameof(Amenity.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(Amenity.CreatedDate)}");

            invalidAmenityException.AddData(
                key: nameof(Amenity.CreatedBy),
                values: "Id is required");

            invalidAmenityException.AddData(
                key: nameof(Amenity.UpdatedBy),
                values: "Id is required");

            var expectedAmenityValidationException =
                new AmenityValidationException(invalidAmenityException);

            // when
            ValueTask<Amenity> createAmenityTask =
                this.amenityService.ModifyAmenityAsync(invalidAmenity);

            // then
            await Assert.ThrowsAsync<AmenityValidationException>(() =>
                createAmenityTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAmenityAsync(It.IsAny<Amenity>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            Amenity randomAmenity = CreateRandomAmenity(dateTime);
            Amenity inputAmenity = randomAmenity;

            var invalidAmenityException = new InvalidAmenityException(
                message: "Invalid Amenity. Please fix the errors and try again.");

            invalidAmenityException.AddData(
               key: nameof(Amenity.UpdatedDate),
               values: $"Date is the same as {nameof(inputAmenity.CreatedDate)}");

            var expectedAmenityValidationException =
                new AmenityValidationException(
                    message: "Amenity validation error occurred, please try again.",
                    innerException: invalidAmenityException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Amenity> modifyAmenityTask =
                this.amenityService.ModifyAmenityAsync(inputAmenity);

            AmenityValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<AmenityValidationException>(
                modifyAmenityTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAmenityAsync(It.IsAny<Amenity>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Amenity randomAmenity = CreateRandomModifyAmenity(dateTime);
            Amenity inputAmenity = randomAmenity;
            inputAmenity.UpdatedBy = inputAmenity.CreatedBy;
            inputAmenity.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidAmenityException = new InvalidAmenityException(
                message: "Invalid Amenity. Please fix the errors and try again.");

            invalidAmenityException.AddData(
                   key: nameof(Amenity.UpdatedDate),
                   values: "Date is not recent");

            var expectedAmenityValidationException =
                new AmenityValidationException(
                    message: "Amenity validation error occurred, please try again.",
                    innerException: invalidAmenityException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Amenity> modifyAmenityTask =
                this.amenityService.ModifyAmenityAsync(inputAmenity);

            AmenityValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<AmenityValidationException>(
                modifyAmenityTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAmenityAsync(It.IsAny<Amenity>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAmenityDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Amenity randomAmenity = CreateRandomAmenity(dateTime);
            Amenity nonExistentAmenity = randomAmenity;
            nonExistentAmenity.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Amenity noAmenity = null;
            var notFoundAmenityException = new NotFoundAmenityException(nonExistentAmenity.Id);

            var expectedAmenityValidationException =
                new AmenityValidationException(
                    message: "Amenity validation error occurred, please try again.",
                    innerException: notFoundAmenityException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAmenityByIdAsync(nonExistentAmenity.Id))
                    .ReturnsAsync(noAmenity);

            // when
            ValueTask<Amenity> modifyAmenityTask =
                this.amenityService.ModifyAmenityAsync(nonExistentAmenity);

            AmenityValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<AmenityValidationException>(
                modifyAmenityTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(nonExistentAmenity.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAmenityAsync(It.IsAny<Amenity>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetNegativeRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            Amenity randomAmenity = CreateRandomModifyAmenity(randomDateTimeOffset);
            Amenity invalidAmenity = randomAmenity.DeepClone();
            Amenity storageAmenity = invalidAmenity.DeepClone();
            storageAmenity.CreatedDate = storageAmenity.CreatedDate.AddMinutes(randomMinutes);
            storageAmenity.UpdatedDate = storageAmenity.UpdatedDate.AddMinutes(randomMinutes);
            Guid AmenityId = invalidAmenity.Id;
          

            var invalidAmenityException = new InvalidAmenityException(
               message: "Invalid Amenity. Please fix the errors and try again.");

            invalidAmenityException.AddData(
                 key: nameof(Amenity.CreatedDate),
                 values: $"Date is not the same as {nameof(Amenity.CreatedDate)}");

            var expectedAmenityValidationException =
              new AmenityValidationException(
                  message: "Amenity validation error occurred, please try again.",
                  innerException: invalidAmenityException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAmenityByIdAsync(AmenityId))
                    .ReturnsAsync(storageAmenity);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Amenity> modifyAmenityTask =
                this.amenityService.ModifyAmenityAsync(invalidAmenity);

            AmenityValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<AmenityValidationException>(
                modifyAmenityTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(invalidAmenity.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAmenityAsync(It.IsAny<Amenity>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int randomPositiveMinutes = GetRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            Amenity randomAmenity = CreateRandomModifyAmenity(randomDateTimeOffset);
            Amenity invalidAmenity = randomAmenity.DeepClone();
            Amenity storageAmenity = invalidAmenity.DeepClone();
            storageAmenity.UpdatedDate = storageAmenity.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid AmenityId = invalidAmenity.Id;
            invalidAmenity.CreatedBy = invalidCreatedBy;

            var invalidAmenityException = new InvalidAmenityException(
                message: "Invalid Amenity. Please fix the errors and try again.");

            invalidAmenityException.AddData(
                key: nameof(Amenity.CreatedBy),
                values: $"Id is not the same as {nameof(Amenity.CreatedBy)}");

            var expectedAmenityValidationException =
              new AmenityValidationException(
                  message: "Amenity validation error occurred, please try again.",
                  innerException: invalidAmenityException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAmenityByIdAsync(AmenityId))
                    .ReturnsAsync(storageAmenity);

            // when
            ValueTask<Amenity> modifyAmenityTask =
                this.amenityService.ModifyAmenityAsync(invalidAmenity);

            AmenityValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<AmenityValidationException>(
                modifyAmenityTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(invalidAmenity.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAmenityAsync(It.IsAny<Amenity>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetCurrentDateTime();
            Amenity randomAmenity = CreateRandomModifyAmenity(randomDate);
            Amenity invalidAmenity = randomAmenity;
            invalidAmenity.UpdatedDate = randomDate;
            Amenity storageAmenity = randomAmenity.DeepClone();
            Guid AmenityId = invalidAmenity.Id;

            var invalidAmenityException = new InvalidAmenityException(
               message: "Invalid Amenity. Please fix the errors and try again.");

            invalidAmenityException.AddData(
               key: nameof(Amenity.UpdatedDate),
               values: $"Date is the same as {nameof(invalidAmenity.UpdatedDate)}");

            var expectedAmenityValidationException =
              new AmenityValidationException(
                  message: "Amenity validation error occurred, please try again.",
                  innerException: invalidAmenityException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAmenityByIdAsync(AmenityId))
                    .ReturnsAsync(storageAmenity);

            // when
            ValueTask<Amenity> modifyAmenityTask =
                this.amenityService.ModifyAmenityAsync(invalidAmenity);

            AmenityValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<AmenityValidationException>(
                modifyAmenityTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(invalidAmenity.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAmenityAsync(It.IsAny<Amenity>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
