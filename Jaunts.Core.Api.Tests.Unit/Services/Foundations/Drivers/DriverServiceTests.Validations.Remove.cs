// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Drivers
{
    public partial class DriverServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomDriverId = default;
            Guid inputDriverId = randomDriverId;

            var invalidDriverException = new InvalidDriverException(
                message: "Invalid driver. Please fix the errors and try again.");

            invalidDriverException.AddData(
                key: nameof(Driver.Id),
                values: "Id is required");

            var expectedDriverValidationException =
                new DriverValidationException(
                    message: "Driver validation error occurred, please try again.",
                    innerException: invalidDriverException);

            // when
            ValueTask<Driver> actualDriverTask =
                this.driverService.RemoveDriverByIdAsync(inputDriverId);

            DriverValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<DriverValidationException>(
                 actualDriverTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDriverAsync(It.IsAny<Driver>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageDriverIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Driver randomDriver = CreateRandomDriver(dateTime);
            Guid inputDriverId = randomDriver.Id;
            Driver inputDriver = randomDriver;
            Driver nullStorageDriver = null;

            var notFoundDriverException = new NotFoundDriverException(inputDriverId);

            var expectedDriverValidationException =
                new DriverValidationException(
                    message: "Driver validation error occurred, please try again.",
                    innerException: notFoundDriverException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverByIdAsync(inputDriverId))
                    .ReturnsAsync(nullStorageDriver);

            // when
            ValueTask<Driver> actualDriverTask =
                this.driverService.RemoveDriverByIdAsync(inputDriverId);

            DriverValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<DriverValidationException>(
                 actualDriverTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(inputDriverId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDriverAsync(It.IsAny<Driver>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
