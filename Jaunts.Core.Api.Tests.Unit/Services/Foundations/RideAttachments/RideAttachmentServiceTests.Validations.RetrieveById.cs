// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.RideAttachments
{
    public partial class RideAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenRideIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomRideId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputRideId = randomRideId;

            var invalidRideAttachmentException =
                 new InvalidRideAttachmentException(
                     message: "Invalid RideAttachment. Please fix the errors and try again.");

            invalidRideAttachmentException.AddData(
              key: nameof(RideAttachment.RideId),
              values: "Id is required");

            var expectedRideAttachmentValidationException =
                new RideAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidRideAttachmentException);

            // when
            ValueTask<RideAttachment> actualRideAttachmentTask =
                this.rideAttachmentService.RetrieveRideAttachmentByIdAsync(inputRideId, inputAttachmentId);

            RideAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<RideAttachmentValidationException>(
                  actualRideAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = default;
            Guid randomRideId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputRideId = randomRideId;

            var invalidRideAttachmentException =
                 new InvalidRideAttachmentException(
                     message: "Invalid RideAttachment. Please fix the errors and try again.");

            invalidRideAttachmentException.AddData(
              key: nameof(RideAttachment.AttachmentId),
              values: "Id is required");

            var expectedRideAttachmentValidationException =
                new RideAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidRideAttachmentException);

            // when
            ValueTask<RideAttachment> actualRideAttachmentTask =
                this.rideAttachmentService.RetrieveRideAttachmentByIdAsync(inputRideId, inputAttachmentId);

            RideAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<RideAttachmentValidationException>(
                  actualRideAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageRideAttachmentIsInvalidAndLogItAsync()
        {
            // given
            RideAttachment randomRideAttachment = CreateRandomRideAttachment();
            Guid inputAttachmentId = randomRideAttachment.AttachmentId;
            Guid inputRideId = randomRideAttachment.RideId;
            RideAttachment nullStorageRideAttachment = null;

            var notFoundRideAttachmentException =
               new NotFoundRideAttachmentException(
                   message: $"Couldn't find attachment with Ride id: {inputRideId} " +
                        $"and attachment id: {inputAttachmentId}.");

            var expectedRideValidationException =
                new RideAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    notFoundRideAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectRideAttachmentByIdAsync(inputRideId, inputAttachmentId))
                    .ReturnsAsync(nullStorageRideAttachment);

            // when
            ValueTask<RideAttachment> actualRideAttachmentRetrieveTask =
                this.rideAttachmentService.RetrieveRideAttachmentByIdAsync(inputRideId, inputAttachmentId);

            RideAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<RideAttachmentValidationException>(
                  actualRideAttachmentRetrieveTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
