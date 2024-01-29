// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Rides
{
    public partial class RideServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenRideIsNullAndLogItAsync()
        {
            // given
            Ride randomRide = null;
            Ride nullRide = randomRide;

            var nullRideException = new NullRideException(
                message: "The Ride is null.");

            var expectedRideValidationException =
                new RideValidationException(
                    message: "Ride validation error occurred, please try again.",
                    innerException: nullRideException);

            // when
            ValueTask<Ride> createRideTask =
                this.rideService.CreateRideAsync(nullRide);

             RideValidationException actualRideDependencyValidationException =
             await Assert.ThrowsAsync<RideValidationException>(
                 createRideTask.AsTask);

            // then
            actualRideDependencyValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateIfRideStatusIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Ride randomRide = CreateRandomRide(randomDateTime);
            Ride invalidRide = randomRide;
            invalidRide.UpdatedBy = randomRide.CreatedBy;
            invalidRide.RideStatus = GetInvalidEnum<RideStatus>();

            var invalidRideException = new InvalidRideException();

            invalidRideException.AddData(
                key: nameof(Ride.RideStatus),
                values: "Value is not recognized");

            var expectedRideValidationException = new RideValidationException(
                message: "Ride validation error occurred, please try again.",
                innerException: invalidRideException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<Ride> createRideTask =
                this.rideService.CreateRideAsync(invalidRide);

            RideValidationException actualRideDependencyValidationException =
            await Assert.ThrowsAsync<RideValidationException>(
                createRideTask.AsTask);

            // then
            actualRideDependencyValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAsync(It.IsAny<Ride>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenRideIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidRide = new Ride
            {
                Name = invalidText,
                Description = invalidText,
                Location = invalidText
            };

            var invalidRideException = new InvalidRideException();

            invalidRideException.AddData(
                key: nameof(Ride.Id),
                values: "Id is required");

            invalidRideException.AddData(
                key: nameof(Ride.Name),
                values: "Text is required");

            invalidRideException.AddData(
                key: nameof(Ride.Location),
                values: "Text is required");

            invalidRideException.AddData(
                key: nameof(Ride.FleetId),
                values: "Id is required");

            invalidRideException.AddData(
                key: nameof(Ride.Description),
                values: "Text is required");

            invalidRideException.AddData(
                key: nameof(Ride.CreatedBy),
                values: "Id is required");

            invalidRideException.AddData(
                key: nameof(Ride.UpdatedBy),
                values: "Id is required");

            invalidRideException.AddData(
                key: nameof(Ride.CreatedDate),
                values: "Date is required");

            invalidRideException.AddData(
                key: nameof(Ride.UpdatedDate),
                values: "Date is required");

            var expectedRideValidationException =
                new RideValidationException(
                    message: "Ride validation error occurred, please try again.",
                    innerException: invalidRideException);

            // when
            ValueTask<Ride> createRideTask =
                this.rideService.CreateRideAsync(invalidRide);

             RideValidationException actualRideDependencyValidationException =
             await Assert.ThrowsAsync<RideValidationException>(
                 createRideTask.AsTask);

            // then
            actualRideDependencyValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
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
            Ride randomRide = CreateRandomRide(dateTime);
            Ride inputRide = randomRide;
            inputRide.UpdatedBy = Guid.NewGuid();

            var invalidRideException = new InvalidRideException();

            invalidRideException.AddData(
                key: nameof(Ride.UpdatedBy),
                values: $"Id is not the same as {nameof(Ride.CreatedBy)}");

            var expectedRideValidationException =
                new RideValidationException(
                    message: "Ride validation error occurred, please try again.",
                    innerException: invalidRideException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Ride> createRideTask =
                this.rideService.CreateRideAsync(inputRide);

             RideValidationException actualRideDependencyValidationException =
             await Assert.ThrowsAsync<RideValidationException>(
                 createRideTask.AsTask);

            // then
            actualRideDependencyValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Ride randomRide = CreateRandomRide(dateTime);
            Ride inputRide = randomRide;
            inputRide.UpdatedBy = randomRide.CreatedBy;
            inputRide.UpdatedDate = GetRandomDateTime();

            var invalidRideException = new InvalidRideException();

            invalidRideException.AddData(
                key: nameof(Ride.UpdatedDate),
                values: $"Date is not the same as {nameof(Ride.CreatedDate)}");

            var expectedRideValidationException =
                new RideValidationException(
                    message: "Ride validation error occurred, please try again.",
                    innerException: invalidRideException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Ride> createRideTask =
                this.rideService.CreateRideAsync(inputRide);

             RideValidationException actualRideDependencyValidationException =
             await Assert.ThrowsAsync<RideValidationException>(
                 createRideTask.AsTask);

            // then
            actualRideDependencyValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
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
            DateTimeOffset dateTime = GetRandomDateTime();
            Ride randomRide = CreateRandomRide(dateTime);
            Ride inputRide = randomRide;
            inputRide.UpdatedBy = inputRide.CreatedBy;
            inputRide.CreatedDate = dateTime.AddMinutes(minutes);
            inputRide.UpdatedDate = inputRide.CreatedDate;

            var invalidRideException = new InvalidRideException();

            invalidRideException.AddData(
                key: nameof(Ride.CreatedDate),
                values: $"Date is not recent");

            var expectedRideValidationException =
                new RideValidationException(
                    message: "Ride validation error occurred, please try again.",
                    innerException: invalidRideException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Ride> createRideTask =
                this.rideService.CreateRideAsync(inputRide);

             RideValidationException actualRideDependencyValidationException =
             await Assert.ThrowsAsync<RideValidationException>(
                 createRideTask.AsTask);

            // then
            actualRideDependencyValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenRideAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Ride randomRide = CreateRandomRide(dateTime);
            Ride alreadyExistsRide = randomRide;
            alreadyExistsRide.UpdatedBy = alreadyExistsRide.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsRideException =
                new AlreadyExistsRideException(
                   message: "Ride with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedRideValidationException =
                new RideDependencyValidationException(
                    message: "Ride dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsRideException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRideAsync(alreadyExistsRide))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Ride> createRideTask =
                this.rideService.CreateRideAsync(alreadyExistsRide);

             RideDependencyValidationException actualRideDependencyValidationException =
             await Assert.ThrowsAsync<RideDependencyValidationException>(
                 createRideTask.AsTask);

            // then
            actualRideDependencyValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAsync(alreadyExistsRide),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
