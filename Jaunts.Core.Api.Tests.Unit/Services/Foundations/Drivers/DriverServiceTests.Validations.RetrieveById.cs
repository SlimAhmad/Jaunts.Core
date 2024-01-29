// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Hosting;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Drivers
{
    public partial class DriverServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
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

            //when
            ValueTask<Driver> retrieveDriverByIdTask =
                this.driverService.RetrieveDriverByIdAsync(inputDriverId);

            DriverValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<DriverValidationException>(
                 retrieveDriverByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageDriverIsNullAndLogItAsync()
        {
            //given
            Guid randomDriverId = Guid.NewGuid();
            Guid someDriverId = randomDriverId;
            Driver invalidStorageDriver = null;
            var notFoundDriverException = new NotFoundDriverException(
                message: $"Couldn't find driver with id: {someDriverId}.");

            var expectedDriverValidationException =
                new DriverValidationException(
                    message: "Driver validation error occurred, please try again.",
                    innerException: notFoundDriverException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageDriver);

            //when
            ValueTask<Driver> retrieveDriverByIdTask =
                this.driverService.RetrieveDriverByIdAsync(someDriverId);

            DriverValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<DriverValidationException>(
                 retrieveDriverByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}