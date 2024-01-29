// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Amenities;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Amenities
{
    public partial class AmenityServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenAmenityIsNullAndLogItAsync()
        {
            // given
            Amenity randomAmenity = null;
            Amenity nullAmenity = randomAmenity;

            var nullAmenityException = new NullAmenityException(
                message: "The Amenity is null.");

            var expectedAmenityValidationException =
                new AmenityValidationException(
                    message: "Amenity validation error occurred, please try again.",
                    innerException: nullAmenityException);

            // when
            ValueTask<Amenity> createAmenityTask =
                this.amenityService.CreateAmenityAsync(nullAmenity);

             AmenityValidationException actualAmenityDependencyValidationException =
             await Assert.ThrowsAsync<AmenityValidationException>(
                 createAmenityTask.AsTask);

            // then
            actualAmenityDependencyValidationException.Should().BeEquivalentTo(
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


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenAmenityIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidAmenity = new Amenity
            {
                Description = invalidText,
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
                key: nameof(Amenity.CreatedBy),
                values: "Id is required");

            invalidAmenityException.AddData(
                key: nameof(Amenity.UpdatedBy),
                values: "Id is required");

            invalidAmenityException.AddData(
                key: nameof(Amenity.CreatedDate),
                values: "Date is required");

            invalidAmenityException.AddData(
                key: nameof(Amenity.UpdatedDate),
                values: "Date is required");

            var expectedAmenityValidationException =
                new AmenityValidationException(
                    message: "Amenity validation error occurred, please try again.",
                    innerException: invalidAmenityException);

            // when
            ValueTask<Amenity> createAmenityTask =
                this.amenityService.CreateAmenityAsync(invalidAmenity);

             AmenityValidationException actualAmenityDependencyValidationException =
             await Assert.ThrowsAsync<AmenityValidationException>(
                 createAmenityTask.AsTask);

            // then
            actualAmenityDependencyValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            Amenity randomAmenity = CreateRandomAmenity(dateTime);
            Amenity inputAmenity = randomAmenity;
            inputAmenity.UpdatedBy = Guid.NewGuid();

            var invalidAmenityException = new InvalidAmenityException();

            invalidAmenityException.AddData(
                key: nameof(Amenity.UpdatedBy),
                values: $"Id is not the same as {nameof(Amenity.CreatedBy)}");

            var expectedAmenityValidationException =
                new AmenityValidationException(
                    message: "Amenity validation error occurred, please try again.",
                    innerException: invalidAmenityException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Amenity> createAmenityTask =
                this.amenityService.CreateAmenityAsync(inputAmenity);

             AmenityValidationException actualAmenityDependencyValidationException =
             await Assert.ThrowsAsync<AmenityValidationException>(
                 createAmenityTask.AsTask);

            // then
            actualAmenityDependencyValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            Amenity randomAmenity = CreateRandomAmenity(dateTime);
            Amenity inputAmenity = randomAmenity;
            inputAmenity.UpdatedBy = randomAmenity.CreatedBy;
            inputAmenity.UpdatedDate = GetRandomDateTime();

            var invalidAmenityException = new InvalidAmenityException();

            invalidAmenityException.AddData(
                key: nameof(Amenity.UpdatedDate),
                values: $"Date is not the same as {nameof(Amenity.CreatedDate)}");

            var expectedAmenityValidationException =
                new AmenityValidationException(
                    message: "Amenity validation error occurred, please try again.",
                    innerException: invalidAmenityException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Amenity> createAmenityTask =
                this.amenityService.CreateAmenityAsync(inputAmenity);

             AmenityValidationException actualAmenityDependencyValidationException =
             await Assert.ThrowsAsync<AmenityValidationException>(
                 createAmenityTask.AsTask);

            // then
            actualAmenityDependencyValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            Amenity randomAmenity = CreateRandomAmenity(dateTime);
            Amenity inputAmenity = randomAmenity;
            inputAmenity.UpdatedBy = inputAmenity.CreatedBy;
            inputAmenity.CreatedDate = dateTime.AddMinutes(minutes);
            inputAmenity.UpdatedDate = inputAmenity.CreatedDate;

            var invalidAmenityException = new InvalidAmenityException();

            invalidAmenityException.AddData(
                key: nameof(Amenity.CreatedDate),
                values: $"Date is not recent");

            var expectedAmenityValidationException =
                new AmenityValidationException(
                    message: "Amenity validation error occurred, please try again.",
                    innerException: invalidAmenityException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Amenity> createAmenityTask =
                this.amenityService.CreateAmenityAsync(inputAmenity);

             AmenityValidationException actualAmenityDependencyValidationException =
             await Assert.ThrowsAsync<AmenityValidationException>(
                 createAmenityTask.AsTask);

            // then
            actualAmenityDependencyValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenAmenityAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Amenity randomAmenity = CreateRandomAmenity(dateTime);
            Amenity alreadyExistsAmenity = randomAmenity;
            alreadyExistsAmenity.UpdatedBy = alreadyExistsAmenity.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsAmenityException =
                new AlreadyExistsAmenityException(
                   message: "Amenity with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedAmenityValidationException =
                new AmenityDependencyValidationException(
                    message: "Amenity dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsAmenityException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAmenityAsync(alreadyExistsAmenity))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Amenity> createAmenityTask =
                this.amenityService.CreateAmenityAsync(alreadyExistsAmenity);

             AmenityDependencyValidationException actualAmenityDependencyValidationException =
             await Assert.ThrowsAsync<AmenityDependencyValidationException>(
                 createAmenityTask.AsTask);

            // then
            actualAmenityDependencyValidationException.Should().BeEquivalentTo(
                expectedAmenityValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAmenityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAmenityAsync(alreadyExistsAmenity),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
