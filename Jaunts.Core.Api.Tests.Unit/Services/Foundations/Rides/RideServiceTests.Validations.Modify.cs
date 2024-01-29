// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Rides
{
    public partial class RideServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenRideIsNullAndLogItAsync()
        {
            //given
            Ride invalidRide = null;
            var nullRideException = new NullRideException();

            var expectedRideValidationException =
                new RideValidationException(
                    message: "Ride validation error occurred, please try again.",
                    nullRideException);

            //when
            ValueTask<Ride> modifyRideTask =
                this.rideService.ModifyRideAsync(invalidRide);

            RideValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<RideValidationException>(
                     modifyRideTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRideAsync(It.IsAny<Ride>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfRideIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidRide = new Ride
            {
                Description = invalidText,
                Name = invalidText,
                Location = invalidText,
                RideStatus = RideStatus.Unavailable,
            };

            var invalidRideException = new InvalidRideException();

            invalidRideException.AddData(
                key: nameof(Ride.Id),
                values: "Id is required");

            invalidRideException.AddData(
                key: nameof(Ride.Description),
                values: "Text is required");

            invalidRideException.AddData(
                key: nameof(Ride.Name),
                values: "Text is required");

            invalidRideException.AddData(
                key: nameof(Ride.Location),
                values: "Text is required");

            invalidRideException.AddData(
                key: nameof(Ride.RideStatus),
                values: "Text is required");
 
            invalidRideException.AddData(
                key: nameof(Ride.CreatedDate),
                values: "Date is required");

            invalidRideException.AddData(
                key: nameof(Ride.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(Ride.CreatedDate)}");

            invalidRideException.AddData(
                key: nameof(Ride.CreatedBy),
                values: "Id is required");

            invalidRideException.AddData(
                key: nameof(Ride.UpdatedBy),
                values: "Id is required");

            var expectedRideValidationException =
                new RideValidationException(invalidRideException);

            // when
            ValueTask<Ride> createRideTask =
                this.rideService.ModifyRideAsync(invalidRide);

            // then
            await Assert.ThrowsAsync<RideValidationException>(() =>
                createRideTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAsync(It.IsAny<Ride>()),
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
            Ride randomRide = CreateRandomRide(dateTime);
            Ride inputRide = randomRide;

            var invalidRideException = new InvalidRideException(
                message: "Invalid ride. Please correct the errors and try again.");

            invalidRideException.AddData(
               key: nameof(Ride.UpdatedDate),
               values: $"Date is the same as {nameof(inputRide.CreatedDate)}");

            var expectedRideValidationException =
                new RideValidationException(
                    message: "Ride validation error occurred, please try again.",
                    innerException: invalidRideException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Ride> modifyRideTask =
                this.rideService.ModifyRideAsync(inputRide);

            RideValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<RideValidationException>(
                modifyRideTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRideAsync(It.IsAny<Ride>()),
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
            Ride randomRide = CreateRandomModifyRide(dateTime);
            Ride inputRide = randomRide;
            inputRide.UpdatedBy = inputRide.CreatedBy;
            inputRide.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidRideException = new InvalidRideException(
                message: "Invalid ride. Please correct the errors and try again.");

            invalidRideException.AddData(
                   key: nameof(Ride.UpdatedDate),
                   values: "Date is not recent");

            var expectedRideValidationException =
                new RideValidationException(
                    message: "Ride validation error occurred, please try again.",
                    innerException: invalidRideException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Ride> modifyRideTask =
                this.rideService.ModifyRideAsync(inputRide);

            RideValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<RideValidationException>(
                modifyRideTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRideAsync(It.IsAny<Ride>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfRideDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Ride randomRide = CreateRandomRide(dateTime);
            Ride nonExistentRide = randomRide;
            nonExistentRide.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Ride noRide = null;
            var notFoundRideException = new NotFoundRideException(nonExistentRide.Id);

            var expectedRideValidationException =
                new RideValidationException(
                    message: "Ride validation error occurred, please try again.",
                    innerException: notFoundRideException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideByIdAsync(nonExistentRide.Id))
                    .ReturnsAsync(noRide);

            // when
            ValueTask<Ride> modifyRideTask =
                this.rideService.ModifyRideAsync(nonExistentRide);

            RideValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<RideValidationException>(
                modifyRideTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(nonExistentRide.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRideAsync(It.IsAny<Ride>()),
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
            Ride randomRide = CreateRandomModifyRide(randomDateTimeOffset);
            Ride invalidRide = randomRide.DeepClone();
            Ride storageRide = invalidRide.DeepClone();
            storageRide.CreatedDate = storageRide.CreatedDate.AddMinutes(randomMinutes);
            storageRide.UpdatedDate = storageRide.UpdatedDate.AddMinutes(randomMinutes);
            Guid rideId = invalidRide.Id;
          

            var invalidRideException = new InvalidRideException(
               message: "Invalid ride. Please correct the errors and try again.");

            invalidRideException.AddData(
                 key: nameof(Ride.CreatedDate),
                 values: $"Date is not the same as {nameof(Ride.CreatedDate)}");

            var expectedRideValidationException =
              new RideValidationException(
                  message: "Ride validation error occurred, please try again.",
                  innerException: invalidRideException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideByIdAsync(rideId))
                    .ReturnsAsync(storageRide);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Ride> modifyRideTask =
                this.rideService.ModifyRideAsync(invalidRide);

            RideValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<RideValidationException>(
                modifyRideTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(invalidRide.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRideAsync(It.IsAny<Ride>()),
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
            Ride randomRide = CreateRandomModifyRide(randomDateTimeOffset);
            Ride invalidRide = randomRide.DeepClone();
            Ride storageRide = invalidRide.DeepClone();
            storageRide.UpdatedDate = storageRide.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid rideId = invalidRide.Id;
            invalidRide.CreatedBy = invalidCreatedBy;

            var invalidRideException = new InvalidRideException(
                message: "Invalid ride. Please correct the errors and try again.");

            invalidRideException.AddData(
                key: nameof(Ride.CreatedBy),
                values: $"Id is not the same as {nameof(Ride.CreatedBy)}");

            var expectedRideValidationException =
              new RideValidationException(
                  message: "Ride validation error occurred, please try again.",
                  innerException: invalidRideException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideByIdAsync(rideId))
                    .ReturnsAsync(storageRide);

            // when
            ValueTask<Ride> modifyRideTask =
                this.rideService.ModifyRideAsync(invalidRide);

            RideValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<RideValidationException>(
                modifyRideTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(invalidRide.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRideAsync(It.IsAny<Ride>()),
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
            Ride randomRide = CreateRandomModifyRide(randomDate);
            Ride invalidRide = randomRide;
            invalidRide.UpdatedDate = randomDate;
            Ride storageRide = randomRide.DeepClone();
            Guid rideId = invalidRide.Id;

            var invalidRideException = new InvalidRideException(
               message: "Invalid ride. Please correct the errors and try again.");

            invalidRideException.AddData(
               key: nameof(Ride.UpdatedDate),
               values: $"Date is the same as {nameof(invalidRide.UpdatedDate)}");

            var expectedRideValidationException =
              new RideValidationException(
                  message: "Ride validation error occurred, please try again.",
                  innerException: invalidRideException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideByIdAsync(rideId))
                    .ReturnsAsync(storageRide);

            // when
            ValueTask<Ride> modifyRideTask =
                this.rideService.ModifyRideAsync(invalidRide);

            RideValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<RideValidationException>(
                modifyRideTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(invalidRide.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRideAsync(It.IsAny<Ride>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
