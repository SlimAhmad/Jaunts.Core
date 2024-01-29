// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Fleets
{
    public partial class FleetServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenFleetIsNullAndLogItAsync()
        {
            // given
            Fleet randomFleet = null;
            Fleet nullFleet = randomFleet;

            var nullFleetException = new NullFleetException(
                message: "The fleet is null.");

            var expectedFleetValidationException =
                new FleetValidationException(
                    message: "Fleet validation error occurred, please try again.",
                    innerException: nullFleetException);

            // when
            ValueTask<Fleet> createFleetTask =
                this.fleetService.CreateFleetAsync(nullFleet);

             FleetValidationException actualFleetDependencyValidationException =
             await Assert.ThrowsAsync<FleetValidationException>(
                 createFleetTask.AsTask);

            // then
            actualFleetDependencyValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateIfFleetStatusIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Fleet randomFleet = CreateRandomFleet(randomDateTime);
            Fleet invalidFleet = randomFleet;
            invalidFleet.UpdatedBy = randomFleet.CreatedBy;
            invalidFleet.Status = GetInvalidEnum<FleetStatus>();

            var invalidFleetException = new InvalidFleetException();

            invalidFleetException.AddData(
                key: nameof(Fleet.Status),
                values: "Value is not recognized");

            var expectedFleetValidationException = new FleetValidationException(
                message: "Fleet validation error occurred, please try again.",
                innerException: invalidFleetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<Fleet> createFleetTask =
                this.fleetService.CreateFleetAsync(invalidFleet);

            FleetValidationException actualFleetDependencyValidationException =
            await Assert.ThrowsAsync<FleetValidationException>(
                createFleetTask.AsTask);

            // then
            actualFleetDependencyValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFleetAsync(It.IsAny<Fleet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenFleetIsInvalidAndLogItAsync(
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
               key: nameof(Fleet.Name),
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
                key: nameof(Fleet.CreatedBy),
                values: "Id is required");

            invalidFleetException.AddData(
                key: nameof(Fleet.UpdatedBy),
                values: "Id is required");

            invalidFleetException.AddData(
                key: nameof(Fleet.CreatedDate),
                values: "Date is required");

            invalidFleetException.AddData(
                key: nameof(Fleet.UpdatedDate),
                values: "Date is required");

            var expectedFleetValidationException =
                new FleetValidationException(
                    message: "Fleet validation error occurred, please try again.",
                    innerException: invalidFleetException);

            // when
            ValueTask<Fleet> createFleetTask =
                this.fleetService.CreateFleetAsync(invalidFleet);

             FleetValidationException actualFleetDependencyValidationException =
             await Assert.ThrowsAsync<FleetValidationException>(
                 createFleetTask.AsTask);

            // then
            actualFleetDependencyValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
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
            Fleet randomFleet = CreateRandomFleet(dateTime);
            Fleet inputFleet = randomFleet;
            inputFleet.UpdatedBy = Guid.NewGuid();

            var invalidFleetException = new InvalidFleetException();

            invalidFleetException.AddData(
                key: nameof(Fleet.UpdatedBy),
                values: $"Id is not the same as {nameof(Fleet.CreatedBy)}");

            var expectedFleetValidationException =
                new FleetValidationException(
                    message: "Fleet validation error occurred, please try again.",
                    innerException: invalidFleetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Fleet> createFleetTask =
                this.fleetService.CreateFleetAsync(inputFleet);

             FleetValidationException actualFleetDependencyValidationException =
             await Assert.ThrowsAsync<FleetValidationException>(
                 createFleetTask.AsTask);

            // then
            actualFleetDependencyValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
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
            Fleet randomFleet = CreateRandomFleet(dateTime);
            Fleet inputFleet = randomFleet;
            inputFleet.UpdatedBy = randomFleet.CreatedBy;
            inputFleet.UpdatedDate = GetRandomDateTime();

            var invalidFleetException = new InvalidFleetException();

            invalidFleetException.AddData(
                key: nameof(Fleet.UpdatedDate),
                values: $"Date is not the same as {nameof(Fleet.CreatedDate)}");

            var expectedFleetValidationException =
                new FleetValidationException(
                    message: "Fleet validation error occurred, please try again.",
                    innerException: invalidFleetException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Fleet> createFleetTask =
                this.fleetService.CreateFleetAsync(inputFleet);

             FleetValidationException actualFleetDependencyValidationException =
             await Assert.ThrowsAsync<FleetValidationException>(
                 createFleetTask.AsTask);

            // then
            actualFleetDependencyValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
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
            Fleet randomFleet = CreateRandomFleet(dateTime);
            Fleet inputFleet = randomFleet;
            inputFleet.UpdatedBy = inputFleet.CreatedBy;
            inputFleet.CreatedDate = dateTime.AddMinutes(minutes);
            inputFleet.UpdatedDate = inputFleet.CreatedDate;

            var invalidFleetException = new InvalidFleetException();

            invalidFleetException.AddData(
                key: nameof(Fleet.CreatedDate),
                values: $"Date is not recent");

            var expectedFleetValidationException =
                new FleetValidationException(
                    message: "Fleet validation error occurred, please try again.",
                    innerException: invalidFleetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Fleet> createFleetTask =
                this.fleetService.CreateFleetAsync(inputFleet);

             FleetValidationException actualFleetDependencyValidationException =
             await Assert.ThrowsAsync<FleetValidationException>(
                 createFleetTask.AsTask);

            // then
            actualFleetDependencyValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenFleetAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fleet randomFleet = CreateRandomFleet(dateTime);
            Fleet alreadyExistsFleet = randomFleet;
            alreadyExistsFleet.UpdatedBy = alreadyExistsFleet.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsFleetException =
                new AlreadyExistsFleetException(
                   message: "Fleet with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedFleetValidationException =
                new FleetDependencyValidationException(
                    message: "Fleet dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsFleetException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertFleetAsync(alreadyExistsFleet))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Fleet> createFleetTask =
                this.fleetService.CreateFleetAsync(alreadyExistsFleet);

             FleetDependencyValidationException actualFleetDependencyValidationException =
             await Assert.ThrowsAsync<FleetDependencyValidationException>(
                 createFleetTask.AsTask);

            // then
            actualFleetDependencyValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFleetAsync(alreadyExistsFleet),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
