// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Drivers
{
    public partial class DriverServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenDriverIsNullAndLogItAsync()
        {
            // given
            Driver randomDriver = null;
            Driver nullDriver = randomDriver;

            var nullDriverException = new NullDriverException(
                message: "The driver is null.");

            var expectedDriverValidationException =
                new DriverValidationException(
                    message: "Driver validation error occurred, please try again.",
                    innerException: nullDriverException);

            // when
            ValueTask<Driver> createDriverTask =
                this.driverService.CreateDriverAsync(nullDriver);

             DriverValidationException actualDriverDependencyValidationException =
             await Assert.ThrowsAsync<DriverValidationException>(
                 createDriverTask.AsTask);

            // then
            actualDriverDependencyValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateIfDriverStatusIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Driver randomDriver = CreateRandomDriver(randomDateTime);
            Driver invalidDriver = randomDriver;
            invalidDriver.UpdatedBy = randomDriver.CreatedBy;
            invalidDriver.DriverStatus = GetInvalidEnum<DriverStatus>();

            var invalidDriverException = new InvalidDriverException();

            invalidDriverException.AddData(
                key: nameof(Driver.DriverStatus),
                values: "Value is not recognized");

            var expectedDriverValidationException = new DriverValidationException(
                message: "Driver validation error occurred, please try again.",
                innerException: invalidDriverException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<Driver> createDriverTask =
                this.driverService.CreateDriverAsync(invalidDriver);

            DriverValidationException actualDriverDependencyValidationException =
            await Assert.ThrowsAsync<DriverValidationException>(
                createDriverTask.AsTask);

            // then
            actualDriverDependencyValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAsync(It.IsAny<Driver>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenDriverIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidDriver = new Driver
            {
                LicenseNumber = invalidText,
                FirstName = invalidText,
                LastName = invalidText,
                MiddleName = invalidText,
                ContactNumber = invalidText,

            };

            var invalidDriverException = new InvalidDriverException();

            invalidDriverException.AddData(
                key: nameof(Driver.Id),
                values: "Id is required");

            invalidDriverException.AddData(
                key: nameof(Driver.FleetId),
                values: "Id is required");

            invalidDriverException.AddData(
                key: nameof(Driver.ProviderId),
                values: "Id is required");

            invalidDriverException.AddData(
                key: nameof(Driver.LicenseNumber),
                values: "Text is required");

            invalidDriverException.AddData(
                key: nameof(Driver.FirstName),
                values: "Text is required");

            invalidDriverException.AddData(
                key: nameof(Driver.LastName),
                values: "Text is required");

            invalidDriverException.AddData(
                key: nameof(Driver.MiddleName),
                values: "Text is required");

            invalidDriverException.AddData(
                key: nameof(Driver.ContactNumber),
                values: "Text is required");

            invalidDriverException.AddData(
                key: nameof(Driver.CreatedBy),
                values: "Id is required");

            invalidDriverException.AddData(
                key: nameof(Driver.UpdatedBy),
                values: "Id is required");

            invalidDriverException.AddData(
                key: nameof(Driver.CreatedDate),
                values: "Date is required");

            invalidDriverException.AddData(
                key: nameof(Driver.UpdatedDate),
                values: "Date is required");

            var expectedDriverValidationException =
                new DriverValidationException(
                    message: "Driver validation error occurred, please try again.",
                    innerException: invalidDriverException);

            // when
            ValueTask<Driver> createDriverTask =
                this.driverService.CreateDriverAsync(invalidDriver);

             DriverValidationException actualDriverDependencyValidationException =
             await Assert.ThrowsAsync<DriverValidationException>(
                 createDriverTask.AsTask);

            // then
            actualDriverDependencyValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(It.IsAny<Guid>()),
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
            Driver randomDriver = CreateRandomDriver(dateTime);
            Driver inputDriver = randomDriver;
            inputDriver.UpdatedBy = Guid.NewGuid();

            var invalidDriverException = new InvalidDriverException();

            invalidDriverException.AddData(
                key: nameof(Driver.UpdatedBy),
                values: $"Id is not the same as {nameof(Driver.CreatedBy)}");

            var expectedDriverValidationException =
                new DriverValidationException(
                    message: "Driver validation error occurred, please try again.",
                    innerException: invalidDriverException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Driver> createDriverTask =
                this.driverService.CreateDriverAsync(inputDriver);

             DriverValidationException actualDriverDependencyValidationException =
             await Assert.ThrowsAsync<DriverValidationException>(
                 createDriverTask.AsTask);

            // then
            actualDriverDependencyValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(It.IsAny<Guid>()),
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
            Driver randomDriver = CreateRandomDriver(dateTime);
            Driver inputDriver = randomDriver;
            inputDriver.UpdatedBy = randomDriver.CreatedBy;
            inputDriver.UpdatedDate = GetRandomDateTime();

            var invalidDriverException = new InvalidDriverException();

            invalidDriverException.AddData(
                key: nameof(Driver.UpdatedDate),
                values: $"Date is not the same as {nameof(Driver.CreatedDate)}");

            var expectedDriverValidationException =
                new DriverValidationException(
                    message: "Driver validation error occurred, please try again.",
                    innerException: invalidDriverException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Driver> createDriverTask =
                this.driverService.CreateDriverAsync(inputDriver);

             DriverValidationException actualDriverDependencyValidationException =
             await Assert.ThrowsAsync<DriverValidationException>(
                 createDriverTask.AsTask);

            // then
            actualDriverDependencyValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(It.IsAny<Guid>()),
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
            Driver randomDriver = CreateRandomDriver(dateTime);
            Driver inputDriver = randomDriver;
            inputDriver.UpdatedBy = inputDriver.CreatedBy;
            inputDriver.CreatedDate = dateTime.AddMinutes(minutes);
            inputDriver.UpdatedDate = inputDriver.CreatedDate;

            var invalidDriverException = new InvalidDriverException();

            invalidDriverException.AddData(
                key: nameof(Driver.CreatedDate),
                values: $"Date is not recent");

            var expectedDriverValidationException =
                new DriverValidationException(
                    message: "Driver validation error occurred, please try again.",
                    innerException: invalidDriverException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Driver> createDriverTask =
                this.driverService.CreateDriverAsync(inputDriver);

             DriverValidationException actualDriverDependencyValidationException =
             await Assert.ThrowsAsync<DriverValidationException>(
                 createDriverTask.AsTask);

            // then
            actualDriverDependencyValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenDriverAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Driver randomDriver = CreateRandomDriver(dateTime);
            Driver alreadyExistsDriver = randomDriver;
            alreadyExistsDriver.UpdatedBy = alreadyExistsDriver.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsDriverException =
                new AlreadyExistsDriverException(
                   message: "Driver with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedDriverValidationException =
                new DriverDependencyValidationException(
                    message: "Driver dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsDriverException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDriverAsync(alreadyExistsDriver))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Driver> createDriverTask =
                this.driverService.CreateDriverAsync(alreadyExistsDriver);

             DriverDependencyValidationException actualDriverDependencyValidationException =
             await Assert.ThrowsAsync<DriverDependencyValidationException>(
                 createDriverTask.AsTask);

            // then
            actualDriverDependencyValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAsync(alreadyExistsDriver),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
