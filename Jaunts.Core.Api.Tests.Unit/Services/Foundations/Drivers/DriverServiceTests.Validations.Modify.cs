// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Drivers
{
    public partial class DriverServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenDriverIsNullAndLogItAsync()
        {
            //given
            Driver invalidDriver = null;
            var nullDriverException = new NullDriverException();

            var expectedDriverValidationException =
                new DriverValidationException(
                    message: "Driver validation error occurred, please try again.",
                    nullDriverException);

            //when
            ValueTask<Driver> modifyDriverTask =
                this.driverService.ModifyDriverAsync(invalidDriver);

            DriverValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<DriverValidationException>(
                     modifyDriverTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDriverAsync(It.IsAny<Driver>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfDriverIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidDriver = new Driver
            {
                ContactNumber = invalidText,
                FirstName = invalidText,
                LastName = invalidText,
                MiddleName = invalidText,
                LicenseNumber = invalidText,
                DriverStatus = DriverStatus.Active,
            };

            var invalidDriverException = new InvalidDriverException();

            invalidDriverException.AddData(
                key: nameof(Driver.Id),
                values: "Id is required");

            invalidDriverException.AddData(
                key: nameof(Driver.ContactNumber),
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
               key: nameof(Driver.LicenseNumber),
               values: "Text is required");

            invalidDriverException.AddData(
                key: nameof(Driver.DriverStatus),
                values: "Text is required");
 
            invalidDriverException.AddData(
                key: nameof(Driver.CreatedDate),
                values: "Date is required");

            invalidDriverException.AddData(
                key: nameof(Driver.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(Driver.CreatedDate)}");

            invalidDriverException.AddData(
                key: nameof(Driver.CreatedBy),
                values: "Id is required");

            invalidDriverException.AddData(
                key: nameof(Driver.UpdatedBy),
                values: "Id is required");

            var expectedDriverValidationException =
                new DriverValidationException(invalidDriverException);

            // when
            ValueTask<Driver> createDriverTask =
                this.driverService.ModifyDriverAsync(invalidDriver);

            // then
            await Assert.ThrowsAsync<DriverValidationException>(() =>
                createDriverTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAsync(It.IsAny<Driver>()),
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
            Driver randomDriver = CreateRandomDriver(dateTime);
            Driver inputDriver = randomDriver;

            var invalidDriverException = new InvalidDriverException(
                message: "Invalid driver. Please fix the errors and try again.");

            invalidDriverException.AddData(
               key: nameof(Driver.UpdatedDate),
               values: $"Date is the same as {nameof(inputDriver.CreatedDate)}");

            var expectedDriverValidationException =
                new DriverValidationException(
                    message: "Driver validation error occurred, please try again.",
                    innerException: invalidDriverException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Driver> modifyDriverTask =
                this.driverService.ModifyDriverAsync(inputDriver);

            DriverValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<DriverValidationException>(
                modifyDriverTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDriverAsync(It.IsAny<Driver>()),
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
            Driver randomDriver = CreateRandomModifyDriver(dateTime);
            Driver inputDriver = randomDriver;
            inputDriver.UpdatedBy = inputDriver.CreatedBy;
            inputDriver.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidDriverException = new InvalidDriverException(
                message: "Invalid driver. Please fix the errors and try again.");

            invalidDriverException.AddData(
                   key: nameof(Driver.UpdatedDate),
                   values: "Date is not recent");

            var expectedDriverValidationException =
                new DriverValidationException(
                    message: "Driver validation error occurred, please try again.",
                    innerException: invalidDriverException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Driver> modifyDriverTask =
                this.driverService.ModifyDriverAsync(inputDriver);

            DriverValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<DriverValidationException>(
                modifyDriverTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDriverAsync(It.IsAny<Driver>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDriverDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Driver randomDriver = CreateRandomDriver(dateTime);
            Driver nonExistentDriver = randomDriver;
            nonExistentDriver.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Driver noDriver = null;
            var notFoundDriverException = new NotFoundDriverException(nonExistentDriver.Id);

            var expectedDriverValidationException =
                new DriverValidationException(
                    message: "Driver validation error occurred, please try again.",
                    innerException: notFoundDriverException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverByIdAsync(nonExistentDriver.Id))
                    .ReturnsAsync(noDriver);

            // when
            ValueTask<Driver> modifyDriverTask =
                this.driverService.ModifyDriverAsync(nonExistentDriver);

            DriverValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<DriverValidationException>(
                modifyDriverTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(nonExistentDriver.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDriverAsync(It.IsAny<Driver>()),
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
            Driver randomDriver = CreateRandomModifyDriver(randomDateTimeOffset);
            Driver invalidDriver = randomDriver.DeepClone();
            Driver storageDriver = invalidDriver.DeepClone();
            storageDriver.CreatedDate = storageDriver.CreatedDate.AddMinutes(randomMinutes);
            storageDriver.UpdatedDate = storageDriver.UpdatedDate.AddMinutes(randomMinutes);
            Guid DriverId = invalidDriver.Id;
          

            var invalidDriverException = new InvalidDriverException(
               message: "Invalid driver. Please fix the errors and try again.");

            invalidDriverException.AddData(
                 key: nameof(Driver.CreatedDate),
                 values: $"Date is not the same as {nameof(Driver.CreatedDate)}");

            var expectedDriverValidationException =
              new DriverValidationException(
                  message: "Driver validation error occurred, please try again.",
                  innerException: invalidDriverException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverByIdAsync(DriverId))
                    .ReturnsAsync(storageDriver);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Driver> modifyDriverTask =
                this.driverService.ModifyDriverAsync(invalidDriver);

            DriverValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<DriverValidationException>(
                modifyDriverTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(invalidDriver.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDriverAsync(It.IsAny<Driver>()),
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
            Driver randomDriver = CreateRandomModifyDriver(randomDateTimeOffset);
            Driver invalidDriver = randomDriver.DeepClone();
            Driver storageDriver = invalidDriver.DeepClone();
            storageDriver.UpdatedDate = storageDriver.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid DriverId = invalidDriver.Id;
            invalidDriver.CreatedBy = invalidCreatedBy;

            var invalidDriverException = new InvalidDriverException(
                message: "Invalid driver. Please fix the errors and try again.");

            invalidDriverException.AddData(
                key: nameof(Driver.CreatedBy),
                values: $"Id is not the same as {nameof(Driver.CreatedBy)}");

            var expectedDriverValidationException =
              new DriverValidationException(
                  message: "Driver validation error occurred, please try again.",
                  innerException: invalidDriverException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverByIdAsync(DriverId))
                    .ReturnsAsync(storageDriver);

            // when
            ValueTask<Driver> modifyDriverTask =
                this.driverService.ModifyDriverAsync(invalidDriver);

            DriverValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<DriverValidationException>(
                modifyDriverTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(invalidDriver.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDriverAsync(It.IsAny<Driver>()),
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
            Driver randomDriver = CreateRandomModifyDriver(randomDate);
            Driver invalidDriver = randomDriver;
            invalidDriver.UpdatedDate = randomDate;
            Driver storageDriver = randomDriver.DeepClone();
            Guid DriverId = invalidDriver.Id;

            var invalidDriverException = new InvalidDriverException(
               message: "Invalid driver. Please fix the errors and try again.");

            invalidDriverException.AddData(
               key: nameof(Driver.UpdatedDate),
               values: $"Date is the same as {nameof(invalidDriver.UpdatedDate)}");

            var expectedDriverValidationException =
              new DriverValidationException(
                  message: "Driver validation error occurred, please try again.",
                  innerException: invalidDriverException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverByIdAsync(DriverId))
                    .ReturnsAsync(storageDriver);

            // when
            ValueTask<Driver> modifyDriverTask =
                this.driverService.ModifyDriverAsync(invalidDriver);

            DriverValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<DriverValidationException>(
                modifyDriverTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(invalidDriver.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDriverAsync(It.IsAny<Driver>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
