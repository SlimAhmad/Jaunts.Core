// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Fleets
{
    public partial class FleetServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenFleetIsNullAndLogItAsync()
        {
            //given
            Fleet invalidFleet = null;
            var nullFleetException = new NullFleetException();

            var expectedFleetValidationException =
                new FleetValidationException(
                    message: "Fleet validation error occurred, please try again.",
                    nullFleetException);

            //when
            ValueTask<Fleet> modifyFleetTask =
                this.fleetService.ModifyFleetAsync(invalidFleet);

            FleetValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<FleetValidationException>(
                     modifyFleetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFleetAsync(It.IsAny<Fleet>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfFleetIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidFleet = new Fleet
            {
                Name = invalidText,
                PlateNumber = invalidText,
                TransmissionType = invalidText,
                Model = invalidText,
                FuelType = invalidText,
                Description = invalidText,

            };

            var invalidFleetException = new InvalidFleetException();

            invalidFleetException.AddData(
                key: nameof(Fleet.Id),
                values: "Id is required");

            invalidFleetException.AddData(
                key: nameof(Fleet.ProviderId),
                values: "Id is required");

            invalidFleetException.AddData(
                key: nameof(Fleet.PlateNumber),
                values: "Text is required");

            invalidFleetException.AddData(
                key: nameof(Fleet.TransmissionType),
                values: "Text is required");

            invalidFleetException.AddData(
                key: nameof(Fleet.Model),
                values: "Text is required");

            invalidFleetException.AddData(
                key: nameof(Fleet.FuelType),
                values: "Text is required");

            invalidFleetException.AddData(
                key: nameof(Fleet.Description),
                values: "Text is required");

            invalidFleetException.AddData(
                key: nameof(Fleet.CreatedDate),
                values: "Date is required");

            invalidFleetException.AddData(
                key: nameof(Fleet.UpdatedDate),
                "Date is required",
                $"Date is the same as {nameof(Fleet.CreatedDate)}");

            invalidFleetException.AddData(
                key: nameof(Fleet.CreatedBy),
                values: "Id is required");

            invalidFleetException.AddData(
                key: nameof(Fleet.UpdatedBy),
                values: "Id is required");

            var expectedFleetValidationException =
                new FleetValidationException(invalidFleetException);

            // when
            ValueTask<Fleet> createFleetTask =
                this.fleetService.ModifyFleetAsync(invalidFleet);

            // then
            await Assert.ThrowsAsync<FleetValidationException>(() =>
                createFleetTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFleetAsync(It.IsAny<Fleet>()),
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
            Fleet randomFleet = CreateRandomFleet(dateTime);
            Fleet inputFleet = randomFleet;

            var invalidFleetException = new InvalidFleetException(
                message: "Invalid fleet. Please fix the errors and try again.");

            invalidFleetException.AddData(
               key: nameof(Fleet.UpdatedDate),
               values: $"Date is the same as {nameof(inputFleet.CreatedDate)}");

            var expectedFleetValidationException =
                new FleetValidationException(
                    message: "Fleet validation error occurred, please try again.",
                    innerException: invalidFleetException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Fleet> modifyFleetTask =
                this.fleetService.ModifyFleetAsync(inputFleet);

            FleetValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<FleetValidationException>(
                modifyFleetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFleetAsync(It.IsAny<Fleet>()),
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
            Fleet randomFleet = CreateRandomModifyFleet(dateTime);
            Fleet inputFleet = randomFleet;
            inputFleet.UpdatedBy = inputFleet.CreatedBy;
            inputFleet.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidFleetException = new InvalidFleetException(
                message: "Invalid fleet. Please fix the errors and try again.");

            invalidFleetException.AddData(
                   key: nameof(Fleet.UpdatedDate),
                   values: "Date is not recent");

            var expectedFleetValidationException =
                new FleetValidationException(
                    message: "Fleet validation error occurred, please try again.",
                    innerException: invalidFleetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Fleet> modifyFleetTask =
                this.fleetService.ModifyFleetAsync(inputFleet);

            FleetValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<FleetValidationException>(
                modifyFleetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFleetAsync(It.IsAny<Fleet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfFleetDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Fleet randomFleet = CreateRandomFleet(dateTime);
            Fleet nonExistentFleet = randomFleet;
            nonExistentFleet.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Fleet noFleet = null;
            var notFoundFleetException = new NotFoundFleetException(nonExistentFleet.Id);

            var expectedFleetValidationException =
                new FleetValidationException(
                    message: "Fleet validation error occurred, please try again.",
                    innerException: notFoundFleetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFleetByIdAsync(nonExistentFleet.Id))
                    .ReturnsAsync(noFleet);

            // when
            ValueTask<Fleet> modifyFleetTask =
                this.fleetService.ModifyFleetAsync(nonExistentFleet);

            FleetValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<FleetValidationException>(
                modifyFleetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(nonExistentFleet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFleetAsync(It.IsAny<Fleet>()),
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
            Fleet randomFleet = CreateRandomModifyFleet(randomDateTimeOffset);
            Fleet invalidFleet = randomFleet.DeepClone();
            Fleet storageFleet = invalidFleet.DeepClone();
            storageFleet.CreatedDate = storageFleet.CreatedDate.AddMinutes(randomMinutes);
            storageFleet.UpdatedDate = storageFleet.UpdatedDate.AddMinutes(randomMinutes);
            Guid FleetId = invalidFleet.Id;
          

            var invalidFleetException = new InvalidFleetException(
               message: "Invalid fleet. Please fix the errors and try again.");

            invalidFleetException.AddData(
                 key: nameof(Fleet.CreatedDate),
                 values: $"Date is not the same as {nameof(Fleet.CreatedDate)}");

            var expectedFleetValidationException =
              new FleetValidationException(
                  message: "Fleet validation error occurred, please try again.",
                  innerException: invalidFleetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFleetByIdAsync(FleetId))
                    .ReturnsAsync(storageFleet);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Fleet> modifyFleetTask =
                this.fleetService.ModifyFleetAsync(invalidFleet);

            FleetValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<FleetValidationException>(
                modifyFleetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(invalidFleet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFleetAsync(It.IsAny<Fleet>()),
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
            Fleet randomFleet = CreateRandomModifyFleet(randomDateTimeOffset);
            Fleet invalidFleet = randomFleet.DeepClone();
            Fleet storageFleet = invalidFleet.DeepClone();
            storageFleet.UpdatedDate = storageFleet.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid FleetId = invalidFleet.Id;
            invalidFleet.CreatedBy = invalidCreatedBy;

            var invalidFleetException = new InvalidFleetException(
                message: "Invalid fleet. Please fix the errors and try again.");

            invalidFleetException.AddData(
                key: nameof(Fleet.CreatedBy),
                values: $"Id is not the same as {nameof(Fleet.CreatedBy)}");

            var expectedFleetValidationException =
              new FleetValidationException(
                  message: "Fleet validation error occurred, please try again.",
                  innerException: invalidFleetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFleetByIdAsync(FleetId))
                    .ReturnsAsync(storageFleet);

            // when
            ValueTask<Fleet> modifyFleetTask =
                this.fleetService.ModifyFleetAsync(invalidFleet);

            FleetValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<FleetValidationException>(
                modifyFleetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(invalidFleet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFleetAsync(It.IsAny<Fleet>()),
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
            Fleet randomFleet = CreateRandomModifyFleet(randomDate);
            Fleet invalidFleet = randomFleet;
            invalidFleet.UpdatedDate = randomDate;
            Fleet storageFleet = randomFleet.DeepClone();
            Guid FleetId = invalidFleet.Id;

            var invalidFleetException = new InvalidFleetException(
               message: "Invalid fleet. Please fix the errors and try again.");

            invalidFleetException.AddData(
               key: nameof(Fleet.UpdatedDate),
               values: $"Date is the same as {nameof(invalidFleet.UpdatedDate)}");

            var expectedFleetValidationException =
              new FleetValidationException(
                  message: "Fleet validation error occurred, please try again.",
                  innerException: invalidFleetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFleetByIdAsync(FleetId))
                    .ReturnsAsync(storageFleet);

            // when
            ValueTask<Fleet> modifyFleetTask =
                this.fleetService.ModifyFleetAsync(invalidFleet);

            FleetValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<FleetValidationException>(
                modifyFleetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(invalidFleet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFleetAsync(It.IsAny<Fleet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
