// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDeals
{
    public partial class FlightDealServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenFlightDealIsNullAndLogItAsync()
        {
            //given
            FlightDeal invalidFlightDeal = null;
            var nullFlightDealException = new NullFlightDealException();

            var expectedFlightDealValidationException =
                new FlightDealValidationException(
                    message: "FlightDeal validation error occurred, please try again.",
                    nullFlightDealException);

            //when
            ValueTask<FlightDeal> modifyFlightDealTask =
                this.flightDealService.ModifyFlightDealAsync(invalidFlightDeal);

            FlightDealValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<FlightDealValidationException>(
                     modifyFlightDealTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFlightDealAsync(It.IsAny<FlightDeal>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfFlightDealIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidFlightDeal = new FlightDeal
            {
                Description = invalidText,
                Airline = invalidText,
                ArrivalCity = invalidText,
                DepartureCity = invalidText

            };

            var invalidFlightDealException = new InvalidFlightDealException();

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.Id),
                values: "Id is required");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.ProviderId),
                values: "Id is required");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.DepartureCity),
                values: "Text is required");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.Description),
                values: "Text is required");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.Airline),
                values: "Text is required");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.ArrivalCity),
                values: "Text is required");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.CreatedDate),
                values: "Date is required");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(FlightDeal.CreatedDate)}");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.CreatedBy),
                values: "Id is required");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.UpdatedBy),
                values: "Id is required");

            var expectedFlightDealValidationException =
                new FlightDealValidationException(invalidFlightDealException);

            // when
            ValueTask<FlightDeal> createFlightDealTask =
                this.flightDealService.ModifyFlightDealAsync(invalidFlightDeal);

            // then
            await Assert.ThrowsAsync<FlightDealValidationException>(() =>
                createFlightDealTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAsync(It.IsAny<FlightDeal>()),
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
            FlightDeal randomFlightDeal = CreateRandomFlightDeal(dateTime);
            FlightDeal inputFlightDeal = randomFlightDeal;

            var invalidFlightDealException = new InvalidFlightDealException(
                message: "Invalid flightDeal. Please fix the errors and try again.");

            invalidFlightDealException.AddData(
               key: nameof(FlightDeal.UpdatedDate),
               values: $"Date is the same as {nameof(inputFlightDeal.CreatedDate)}");

            var expectedFlightDealValidationException =
                new FlightDealValidationException(
                    message: "FlightDeal validation error occurred, please try again.",
                    innerException: invalidFlightDealException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<FlightDeal> modifyFlightDealTask =
                this.flightDealService.ModifyFlightDealAsync(inputFlightDeal);

            FlightDealValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<FlightDealValidationException>(
                modifyFlightDealTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFlightDealAsync(It.IsAny<FlightDeal>()),
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
            FlightDeal randomFlightDeal = CreateRandomModifyFlightDeal(dateTime);
            FlightDeal inputFlightDeal = randomFlightDeal;
            inputFlightDeal.UpdatedBy = inputFlightDeal.CreatedBy;
            inputFlightDeal.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidFlightDealException = new InvalidFlightDealException(
                message: "Invalid flightDeal. Please fix the errors and try again.");

            invalidFlightDealException.AddData(
                   key: nameof(FlightDeal.UpdatedDate),
                   values: "Date is not recent");

            var expectedFlightDealValidationException =
                new FlightDealValidationException(
                    message: "FlightDeal validation error occurred, please try again.",
                    innerException: invalidFlightDealException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<FlightDeal> modifyFlightDealTask =
                this.flightDealService.ModifyFlightDealAsync(inputFlightDeal);

            FlightDealValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<FlightDealValidationException>(
                modifyFlightDealTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFlightDealAsync(It.IsAny<FlightDeal>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfFlightDealDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            FlightDeal randomFlightDeal = CreateRandomFlightDeal(dateTime);
            FlightDeal nonExistentFlightDeal = randomFlightDeal;
            nonExistentFlightDeal.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            FlightDeal noFlightDeal = null;
            var notFoundFlightDealException = new NotFoundFlightDealException(nonExistentFlightDeal.Id);

            var expectedFlightDealValidationException =
                new FlightDealValidationException(
                    message: "FlightDeal validation error occurred, please try again.",
                    innerException: notFoundFlightDealException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealByIdAsync(nonExistentFlightDeal.Id))
                    .ReturnsAsync(noFlightDeal);

            // when
            ValueTask<FlightDeal> modifyFlightDealTask =
                this.flightDealService.ModifyFlightDealAsync(nonExistentFlightDeal);

            FlightDealValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<FlightDealValidationException>(
                modifyFlightDealTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(nonExistentFlightDeal.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFlightDealAsync(It.IsAny<FlightDeal>()),
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
            FlightDeal randomFlightDeal = CreateRandomModifyFlightDeal(randomDateTimeOffset);
            FlightDeal invalidFlightDeal = randomFlightDeal.DeepClone();
            FlightDeal storageFlightDeal = invalidFlightDeal.DeepClone();
            storageFlightDeal.CreatedDate = storageFlightDeal.CreatedDate.AddMinutes(randomMinutes);
            storageFlightDeal.UpdatedDate = storageFlightDeal.UpdatedDate.AddMinutes(randomMinutes);
            Guid FlightDealId = invalidFlightDeal.Id;
          

            var invalidFlightDealException = new InvalidFlightDealException(
               message: "Invalid flightDeal. Please fix the errors and try again.");

            invalidFlightDealException.AddData(
                 key: nameof(FlightDeal.CreatedDate),
                 values: $"Date is not the same as {nameof(FlightDeal.CreatedDate)}");

            var expectedFlightDealValidationException =
              new FlightDealValidationException(
                  message: "FlightDeal validation error occurred, please try again.",
                  innerException: invalidFlightDealException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealByIdAsync(FlightDealId))
                    .ReturnsAsync(storageFlightDeal);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<FlightDeal> modifyFlightDealTask =
                this.flightDealService.ModifyFlightDealAsync(invalidFlightDeal);

            FlightDealValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<FlightDealValidationException>(
                modifyFlightDealTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(invalidFlightDeal.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFlightDealAsync(It.IsAny<FlightDeal>()),
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
            FlightDeal randomFlightDeal = CreateRandomModifyFlightDeal(randomDateTimeOffset);
            FlightDeal invalidFlightDeal = randomFlightDeal.DeepClone();
            FlightDeal storageFlightDeal = invalidFlightDeal.DeepClone();
            storageFlightDeal.UpdatedDate = storageFlightDeal.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid FlightDealId = invalidFlightDeal.Id;
            invalidFlightDeal.CreatedBy = invalidCreatedBy;

            var invalidFlightDealException = new InvalidFlightDealException(
                message: "Invalid flightDeal. Please fix the errors and try again.");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.CreatedBy),
                values: $"Id is not the same as {nameof(FlightDeal.CreatedBy)}");

            var expectedFlightDealValidationException =
              new FlightDealValidationException(
                  message: "FlightDeal validation error occurred, please try again.",
                  innerException: invalidFlightDealException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealByIdAsync(FlightDealId))
                    .ReturnsAsync(storageFlightDeal);

            // when
            ValueTask<FlightDeal> modifyFlightDealTask =
                this.flightDealService.ModifyFlightDealAsync(invalidFlightDeal);

            FlightDealValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<FlightDealValidationException>(
                modifyFlightDealTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(invalidFlightDeal.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFlightDealAsync(It.IsAny<FlightDeal>()),
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
            FlightDeal randomFlightDeal = CreateRandomModifyFlightDeal(randomDate);
            FlightDeal invalidFlightDeal = randomFlightDeal;
            invalidFlightDeal.UpdatedDate = randomDate;
            FlightDeal storageFlightDeal = randomFlightDeal.DeepClone();
            Guid FlightDealId = invalidFlightDeal.Id;

            var invalidFlightDealException = new InvalidFlightDealException(
               message: "Invalid flightDeal. Please fix the errors and try again.");

            invalidFlightDealException.AddData(
               key: nameof(FlightDeal.UpdatedDate),
               values: $"Date is the same as {nameof(invalidFlightDeal.UpdatedDate)}");

            var expectedFlightDealValidationException =
              new FlightDealValidationException(
                  message: "FlightDeal validation error occurred, please try again.",
                  innerException: invalidFlightDealException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealByIdAsync(FlightDealId))
                    .ReturnsAsync(storageFlightDeal);

            // when
            ValueTask<FlightDeal> modifyFlightDealTask =
                this.flightDealService.ModifyFlightDealAsync(invalidFlightDeal);

            FlightDealValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<FlightDealValidationException>(
                modifyFlightDealTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(invalidFlightDeal.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFlightDealAsync(It.IsAny<FlightDeal>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
