// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Fleets
{
    public partial class FleetServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
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

            // when
            ValueTask<Fleet> actualFleetTask =
                this.fleetService.RemoveFleetByIdAsync(inputFleetId);

            FleetValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<FleetValidationException>(
                 actualFleetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFleetAsync(It.IsAny<Fleet>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageFleetIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fleet randomFleet = CreateRandomFleet(dateTime);
            Guid inputFleetId = randomFleet.Id;
            Fleet inputFleet = randomFleet;
            Fleet nullStorageFleet = null;

            var notFoundFleetException = new NotFoundFleetException(inputFleetId);

            var expectedFleetValidationException =
                new FleetValidationException(
                    message: "Fleet validation error occurred, please try again.",
                    innerException: notFoundFleetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFleetByIdAsync(inputFleetId))
                    .ReturnsAsync(nullStorageFleet);

            // when
            ValueTask<Fleet> actualFleetTask =
                this.fleetService.RemoveFleetByIdAsync(inputFleetId);

            FleetValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<FleetValidationException>(
                 actualFleetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFleetValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFleetByIdAsync(inputFleetId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFleetAsync(It.IsAny<Fleet>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
