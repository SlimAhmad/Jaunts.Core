// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Hosting;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Fleets
{
    public partial class FleetServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomFleetId = default;
            Guid inputFleetId = randomFleetId;

            var invalidFleetException = new InvalidFleetException(
                message: "Invalid fleet. Please fix the errors and try again.");

            invalidFleetException.AddData(
                key: nameof(Fleet.Id),
                values: "Id is required");

            var expectedFleetValidationException =
                new FleetValidationException(
                    message: "Fleet validation error occurred, please try again.", 
                    innerException: invalidFleetException);

            //when
            ValueTask<Fleet> retrieveFleetByIdTask =
                this.fleetService.RetrieveFleetByIdAsync(inputFleetId);

            FleetValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<FleetValidationException>(
                 retrieveFleetByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageFleetIsNullAndLogItAsync()
        {
            //given
            Guid randomFleetId = Guid.NewGuid();
            Guid someFleetId = randomFleetId;
            Fleet invalidStorageFleet = null;
            var notFoundFleetException = new NotFoundFleetException(
                message: $"Couldn't find fleet with id: {someFleetId}.");

            var expectedFleetValidationException =
                new FleetValidationException(
                    message: "Fleet validation error occurred, please try again.",
                    innerException: notFoundFleetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageFleet);

            //when
            ValueTask<Fleet> retrieveFleetByIdTask =
                this.fleetService.RetrieveFleetByIdAsync(someFleetId);

            FleetValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<FleetValidationException>(
                 retrieveFleetByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}