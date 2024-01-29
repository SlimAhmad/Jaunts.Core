// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDeals
{
    public partial class FlightDealServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenFlightDealIsNullAndLogItAsync()
        {
            // given
            FlightDeal randomFlightDeal = null;
            FlightDeal nullFlightDeal = randomFlightDeal;

            var nullFlightDealException = new NullFlightDealException(
                message: "The flightDeal is null.");

            var expectedFlightDealValidationException =
                new FlightDealValidationException(
                    message: "FlightDeal validation error occurred, please try again.",
                    innerException: nullFlightDealException);

            // when
            ValueTask<FlightDeal> createFlightDealTask =
                this.flightDealService.CreateFlightDealAsync(nullFlightDeal);

             FlightDealValidationException actualFlightDealDependencyValidationException =
             await Assert.ThrowsAsync<FlightDealValidationException>(
                 createFlightDealTask.AsTask);

            // then
            actualFlightDealDependencyValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateIfFlightDealStatusIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            FlightDeal randomFlightDeal = CreateRandomFlightDeal(randomDateTime);
            FlightDeal invalidFlightDeal = randomFlightDeal;
            invalidFlightDeal.UpdatedBy = randomFlightDeal.CreatedBy;
            invalidFlightDeal.Status = GetInvalidEnum<FlightDealsStatus>();

            var invalidFlightDealException = new InvalidFlightDealException();

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.Status),
                values: "Value is not recognized");

            var expectedFlightDealValidationException = new FlightDealValidationException(
                message: "FlightDeal validation error occurred, please try again.",
                innerException: invalidFlightDealException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<FlightDeal> createFlightDealTask =
                this.flightDealService.CreateFlightDealAsync(invalidFlightDeal);

            FlightDealValidationException actualFlightDealDependencyValidationException =
            await Assert.ThrowsAsync<FlightDealValidationException>(
                createFlightDealTask.AsTask);

            // then
            actualFlightDealDependencyValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAsync(It.IsAny<FlightDeal>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenFlightDealIsInvalidAndLogItAsync(
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
                key: nameof(FlightDeal.CreatedBy),
                values: "Id is required");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.UpdatedBy),
                values: "Id is required");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.CreatedDate),
                values: "Date is required");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.UpdatedDate),
                values: "Date is required");

            invalidFlightDealException.AddData(
               key: nameof(FlightDeal.StartDate),
               values: "Date is required");

            invalidFlightDealException.AddData(
               key: nameof(FlightDeal.EndDate),
               values: "Date is required");

            var expectedFlightDealValidationException =
                new FlightDealValidationException(
                    message: "FlightDeal validation error occurred, please try again.",
                    innerException: invalidFlightDealException);

            // when
            ValueTask<FlightDeal> createFlightDealTask =
                this.flightDealService.CreateFlightDealAsync(invalidFlightDeal);

             FlightDealValidationException actualFlightDealDependencyValidationException =
             await Assert.ThrowsAsync<FlightDealValidationException>(
                 createFlightDealTask.AsTask);

            // then
            actualFlightDealDependencyValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
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
            FlightDeal randomFlightDeal = CreateRandomFlightDeal(dateTime);
            FlightDeal inputFlightDeal = randomFlightDeal;
            inputFlightDeal.UpdatedBy = Guid.NewGuid();

            var invalidFlightDealException = new InvalidFlightDealException();

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.UpdatedBy),
                values: $"Id is not the same as {nameof(FlightDeal.CreatedBy)}");

            var expectedFlightDealValidationException =
                new FlightDealValidationException(
                    message: "FlightDeal validation error occurred, please try again.",
                    innerException: invalidFlightDealException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<FlightDeal> createFlightDealTask =
                this.flightDealService.CreateFlightDealAsync(inputFlightDeal);

             FlightDealValidationException actualFlightDealDependencyValidationException =
             await Assert.ThrowsAsync<FlightDealValidationException>(
                 createFlightDealTask.AsTask);

            // then
            actualFlightDealDependencyValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
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
            FlightDeal randomFlightDeal = CreateRandomFlightDeal(dateTime);
            FlightDeal inputFlightDeal = randomFlightDeal;
            inputFlightDeal.UpdatedBy = randomFlightDeal.CreatedBy;
            inputFlightDeal.UpdatedDate = GetRandomDateTime();

            var invalidFlightDealException = new InvalidFlightDealException();

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.UpdatedDate),
                values: $"Date is not the same as {nameof(FlightDeal.CreatedDate)}");

            var expectedFlightDealValidationException =
                new FlightDealValidationException(
                    message: "FlightDeal validation error occurred, please try again.",
                    innerException: invalidFlightDealException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<FlightDeal> createFlightDealTask =
                this.flightDealService.CreateFlightDealAsync(inputFlightDeal);

             FlightDealValidationException actualFlightDealDependencyValidationException =
             await Assert.ThrowsAsync<FlightDealValidationException>(
                 createFlightDealTask.AsTask);

            // then
            actualFlightDealDependencyValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
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
            FlightDeal randomFlightDeal = CreateRandomFlightDeal(dateTime);
            FlightDeal inputFlightDeal = randomFlightDeal;
            inputFlightDeal.UpdatedBy = inputFlightDeal.CreatedBy;
            inputFlightDeal.CreatedDate = dateTime.AddMinutes(minutes);
            inputFlightDeal.UpdatedDate = inputFlightDeal.CreatedDate;

            var invalidFlightDealException = new InvalidFlightDealException();

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.CreatedDate),
                values: $"Date is not recent");

            var expectedFlightDealValidationException =
                new FlightDealValidationException(
                    message: "FlightDeal validation error occurred, please try again.",
                    innerException: invalidFlightDealException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<FlightDeal> createFlightDealTask =
                this.flightDealService.CreateFlightDealAsync(inputFlightDeal);

             FlightDealValidationException actualFlightDealDependencyValidationException =
             await Assert.ThrowsAsync<FlightDealValidationException>(
                 createFlightDealTask.AsTask);

            // then
            actualFlightDealDependencyValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenFlightDealAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            FlightDeal randomFlightDeal = CreateRandomFlightDeal(dateTime);
            FlightDeal alreadyExistsFlightDeal = randomFlightDeal;
            alreadyExistsFlightDeal.UpdatedBy = alreadyExistsFlightDeal.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsFlightDealException =
                new AlreadyExistsFlightDealException(
                   message: "FlightDeal with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedFlightDealValidationException =
                new FlightDealDependencyValidationException(
                    message: "FlightDeal dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsFlightDealException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertFlightDealAsync(alreadyExistsFlightDeal))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<FlightDeal> createFlightDealTask =
                this.flightDealService.CreateFlightDealAsync(alreadyExistsFlightDeal);

             FlightDealDependencyValidationException actualFlightDealDependencyValidationException =
             await Assert.ThrowsAsync<FlightDealDependencyValidationException>(
                 createFlightDealTask.AsTask);

            // then
            actualFlightDealDependencyValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAsync(alreadyExistsFlightDeal),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
