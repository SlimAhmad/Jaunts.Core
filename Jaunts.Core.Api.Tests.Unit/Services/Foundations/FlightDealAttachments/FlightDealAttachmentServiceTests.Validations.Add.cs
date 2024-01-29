// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDealAttachments
{
    public partial class FlightDealAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenFlightDealAttachmentIsNullAndLogItAsync()
        {
            // given
            FlightDealAttachment invalidFlightDealAttachment = null;

            var nullFlightDealAttachmentException = new NullFlightDealAttachmentException(
                message: "The FlightDealAttachment is null.");

            var expectedFlightDealAttachmentValidationException =
                new FlightDealAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: nullFlightDealAttachmentException);

            // when
            ValueTask<FlightDealAttachment> addFlightDealAttachmentTask =
                this.flightDealAttachmentService.AddFlightDealAttachmentAsync(invalidFlightDealAttachment);

            FlightDealAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<FlightDealAttachmentValidationException>(
                  addFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenFlightDealIdIsInvalidAndLogItAsync()
        {
            // given
            FlightDealAttachment randomFlightDealAttachment = CreateRandomFlightDealAttachment();
            FlightDealAttachment inputFlightDealAttachment = randomFlightDealAttachment;
            inputFlightDealAttachment.FlightDealId = default;

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
            ValueTask<FlightDealAttachment> addFlightDealAttachmentTask =
                this.flightDealAttachmentService.AddFlightDealAttachmentAsync(inputFlightDealAttachment);

            FlightDealAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<FlightDealAttachmentValidationException>(
                  addFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            FlightDealAttachment randomFlightDealAttachment = CreateRandomFlightDealAttachment();
            FlightDealAttachment inputFlightDealAttachment = randomFlightDealAttachment;
            inputFlightDealAttachment.AttachmentId = default;

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
            ValueTask<FlightDealAttachment> addFlightDealAttachmentTask =
                this.flightDealAttachmentService.AddFlightDealAttachmentAsync(inputFlightDealAttachment);

            FlightDealAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<FlightDealAttachmentValidationException>(
                  addFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenFlightDealAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            FlightDealAttachment randomFlightDealAttachment = CreateRandomFlightDealAttachment();
            FlightDealAttachment alreadyExistsFlightDealAttachment = randomFlightDealAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsFlightDealAttachmentException =
                new AlreadyExistsFlightDealAttachmentException(
                    message: "FlightDealAttachment  with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedFlightDealAttachmentValidationException =
                new FlightDealAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: alreadyExistsFlightDealAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertFlightDealAttachmentAsync(alreadyExistsFlightDealAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<FlightDealAttachment> addFlightDealAttachmentTask =
                this.flightDealAttachmentService.AddFlightDealAttachmentAsync(alreadyExistsFlightDealAttachment);

            FlightDealAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<FlightDealAttachmentValidationException>(
                  addFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedFlightDealAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAttachmentAsync(alreadyExistsFlightDealAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferenceExceptionAndLogItAsync()
        {
            // given
            FlightDealAttachment randomFlightDealAttachment = CreateRandomFlightDealAttachment();
            FlightDealAttachment invalidFlightDealAttachment = randomFlightDealAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidFlightDealAttachmentReferenceException =
                new InvalidFlightDealAttachmentReferenceException(
                    message: "Invalid guardian attachment reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedFlightDealAttachmentValidationException =
                new FlightDealAttachmentValidationException(
                    message: "Invalid input, contact support.", 
                    innerException: invalidFlightDealAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertFlightDealAttachmentAsync(invalidFlightDealAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<FlightDealAttachment> addFlightDealAttachmentTask =
                this.flightDealAttachmentService.AddFlightDealAttachmentAsync(invalidFlightDealAttachment);

            FlightDealAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<FlightDealAttachmentValidationException>(
                  addFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedFlightDealAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAttachmentAsync(invalidFlightDealAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
