// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDealAttachments
{
    public partial class FlightDealAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenFlightDealIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomFlightDealId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputFlightDealId = randomFlightDealId;

            var invalidFlightDealAttachmentException =
              new InvalidFlightDealAttachmentException(
                  message: "Invalid FlightDealAttachment. Please correct the errors and try again.");

            invalidFlightDealAttachmentException.AddData(
                key: nameof(FlightDealAttachment.FlightDealId),
                values: "Id is required");

            var expectedFlightDealAttachmentValidationException =
                new FlightDealAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidFlightDealAttachmentException);

            // when
            ValueTask<FlightDealAttachment> removeFlightDealAttachmentTask =
                this.flightDealAttachmentService.RemoveFlightDealAttachmentByIdAsync(inputFlightDealId, inputAttachmentId);

            FlightDealAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<FlightDealAttachmentValidationException>(
                  removeFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = default;
            Guid randomFlightDealId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputFlightDealId = randomFlightDealId;

            var invalidFlightDealAttachmentException =
              new InvalidFlightDealAttachmentException(
                  message: "Invalid FlightDealAttachment. Please correct the errors and try again.");

            invalidFlightDealAttachmentException.AddData(
              key: nameof(FlightDealAttachment.AttachmentId),
              values: "Id is required");

            var expectedFlightDealAttachmentValidationException =
                new FlightDealAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidFlightDealAttachmentException);

            // when
            ValueTask<FlightDealAttachment> removeFlightDealAttachmentTask =
                this.flightDealAttachmentService.RemoveFlightDealAttachmentByIdAsync(inputFlightDealId, inputAttachmentId);

            FlightDealAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<FlightDealAttachmentValidationException>(
                  removeFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageFlightDealAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            FlightDealAttachment randomFlightDealAttachment = CreateRandomFlightDealAttachment(randomDateTime);
            Guid inputAttachmentId = randomFlightDealAttachment.AttachmentId;
            Guid inputFlightDealId = randomFlightDealAttachment.FlightDealId;
            FlightDealAttachment nullStorageFlightDealAttachment = null;

            var notFoundFlightDealAttachmentException =
               new NotFoundFlightDealAttachmentException(
                   message: $"Couldn't find attachment with FlightDeal id: {inputFlightDealId} " +
                        $"and attachment id: {inputAttachmentId}.");

            var expectedFlightDealValidationException =
                new FlightDealAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    notFoundFlightDealAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectFlightDealAttachmentByIdAsync(inputFlightDealId, inputAttachmentId))
                    .ReturnsAsync(nullStorageFlightDealAttachment);

            // when
            ValueTask<FlightDealAttachment> removeFlightDealAttachmentTask =
                this.flightDealAttachmentService.RemoveFlightDealAttachmentByIdAsync(inputFlightDealId, inputAttachmentId);

            FlightDealAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<FlightDealAttachmentValidationException>(
                  removeFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}