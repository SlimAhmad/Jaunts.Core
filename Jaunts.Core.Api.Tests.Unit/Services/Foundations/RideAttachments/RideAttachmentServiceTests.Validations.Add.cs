// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.RideAttachments
{
    public partial class RideAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenRideAttachmentIsNullAndLogItAsync()
        {
            // given
            RideAttachment invalidRideAttachment = null;

            var nullRideAttachmentException = new NullRideAttachmentException(
                message: "The RideAttachment is null.");

            var expectedRideAttachmentValidationException =
                new RideAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: nullRideAttachmentException);

            // when
            ValueTask<RideAttachment> addRideAttachmentTask =
                this.rideAttachmentService.AddRideAttachmentAsync(invalidRideAttachment);

            RideAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<RideAttachmentValidationException>(
                  addRideAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAttachmentAsync(It.IsAny<RideAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenRideIdIsInvalidAndLogItAsync()
        {
            // given
            RideAttachment randomRideAttachment = CreateRandomRideAttachment();
            RideAttachment inputRideAttachment = randomRideAttachment;
            inputRideAttachment.RideId = default;

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
            ValueTask<RideAttachment> addRideAttachmentTask =
                this.rideAttachmentService.AddRideAttachmentAsync(inputRideAttachment);

            RideAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<RideAttachmentValidationException>(
                  addRideAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAttachmentAsync(It.IsAny<RideAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            RideAttachment randomRideAttachment = CreateRandomRideAttachment();
            RideAttachment inputRideAttachment = randomRideAttachment;
            inputRideAttachment.AttachmentId = default;

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
            ValueTask<RideAttachment> addRideAttachmentTask =
                this.rideAttachmentService.AddRideAttachmentAsync(inputRideAttachment);

            RideAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<RideAttachmentValidationException>(
                  addRideAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAttachmentAsync(It.IsAny<RideAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenRideAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            RideAttachment randomRideAttachment = CreateRandomRideAttachment();
            RideAttachment alreadyExistsRideAttachment = randomRideAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsRideAttachmentException =
                new AlreadyExistsRideAttachmentException(
                    message: "RideAttachment  with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedRideAttachmentValidationException =
                new RideAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: alreadyExistsRideAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRideAttachmentAsync(alreadyExistsRideAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<RideAttachment> addRideAttachmentTask =
                this.rideAttachmentService.AddRideAttachmentAsync(alreadyExistsRideAttachment);

            RideAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<RideAttachmentValidationException>(
                  addRideAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedRideAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAttachmentAsync(alreadyExistsRideAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferenceExceptionAndLogItAsync()
        {
            // given
            RideAttachment randomRideAttachment = CreateRandomRideAttachment();
            RideAttachment invalidRideAttachment = randomRideAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidRideAttachmentReferenceException =
                new InvalidRideAttachmentReferenceException(
                    message: "Invalid ride attachment reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedRideAttachmentValidationException =
                new RideAttachmentValidationException(
                    message: "Invalid input, contact support.", 
                    innerException: invalidRideAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRideAttachmentAsync(invalidRideAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<RideAttachment> addRideAttachmentTask =
                this.rideAttachmentService.AddRideAttachmentAsync(invalidRideAttachment);

            RideAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<RideAttachmentValidationException>(
                  addRideAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedRideAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedRideAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAttachmentAsync(invalidRideAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
