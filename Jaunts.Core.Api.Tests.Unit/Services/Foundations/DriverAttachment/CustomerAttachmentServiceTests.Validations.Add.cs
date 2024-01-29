// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.DriverAttachments
{
    public partial class DriverAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenDriverAttachmentIsNullAndLogItAsync()
        {
            // given
            DriverAttachment invalidDriverAttachment = null;

            var nullDriverAttachmentException = new NullDriverAttachmentException(
                message: "The DriverAttachment is null.");

            var expectedDriverAttachmentValidationException =
                new DriverAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: nullDriverAttachmentException);

            // when
            ValueTask<DriverAttachment> addDriverAttachmentTask =
                this.driverAttachmentService.AddDriverAttachmentAsync(invalidDriverAttachment);

            DriverAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<DriverAttachmentValidationException>(
                  addDriverAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAttachmentAsync(It.IsAny<DriverAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenDriverIdIsInvalidAndLogItAsync()
        {
            // given
            DriverAttachment randomDriverAttachment = CreateRandomDriverAttachment();
            DriverAttachment inputDriverAttachment = randomDriverAttachment;
            inputDriverAttachment.DriverId = default;

            var invalidDriverAttachmentException =
              new InvalidDriverAttachmentException(
                  message: "Invalid DriverAttachment. Please correct the errors and try again.");

            invalidDriverAttachmentException.AddData(
                key: nameof(DriverAttachment.DriverId),
                values: "Id is required");

            var expectedDriverAttachmentValidationException =
                new DriverAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidDriverAttachmentException);

            // when
            ValueTask<DriverAttachment> addDriverAttachmentTask =
                this.driverAttachmentService.AddDriverAttachmentAsync(inputDriverAttachment);

            DriverAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<DriverAttachmentValidationException>(
                  addDriverAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAttachmentAsync(It.IsAny<DriverAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            DriverAttachment randomDriverAttachment = CreateRandomDriverAttachment();
            DriverAttachment inputDriverAttachment = randomDriverAttachment;
            inputDriverAttachment.AttachmentId = default;

            var invalidDriverAttachmentException =
                new InvalidDriverAttachmentException(
                    message: "Invalid DriverAttachment. Please correct the errors and try again.");

            invalidDriverAttachmentException.AddData(
                key: nameof(DriverAttachment.AttachmentId),
                values: "Id is required");

            var expectedDriverAttachmentValidationException =
                new DriverAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidDriverAttachmentException);

            // when
            ValueTask<DriverAttachment> addDriverAttachmentTask =
                this.driverAttachmentService.AddDriverAttachmentAsync(inputDriverAttachment);

            DriverAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<DriverAttachmentValidationException>(
                  addDriverAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAttachmentAsync(It.IsAny<DriverAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenDriverAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            DriverAttachment randomDriverAttachment = CreateRandomDriverAttachment();
            DriverAttachment alreadyExistsDriverAttachment = randomDriverAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsDriverAttachmentException =
                new AlreadyExistsDriverAttachmentException(
                    message: "DriverAttachment  with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedDriverAttachmentValidationException =
                new DriverAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: alreadyExistsDriverAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDriverAttachmentAsync(alreadyExistsDriverAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<DriverAttachment> addDriverAttachmentTask =
                this.driverAttachmentService.AddDriverAttachmentAsync(alreadyExistsDriverAttachment);

            DriverAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<DriverAttachmentValidationException>(
                  addDriverAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDriverAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAttachmentAsync(alreadyExistsDriverAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferenceExceptionAndLogItAsync()
        {
            // given
            DriverAttachment randomDriverAttachment = CreateRandomDriverAttachment();
            DriverAttachment invalidDriverAttachment = randomDriverAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidDriverAttachmentReferenceException =
                new InvalidDriverAttachmentReferenceException(
                    message: "Invalid guardian attachment reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedDriverAttachmentValidationException =
                new DriverAttachmentValidationException(
                    message: "Invalid input, contact support.", 
                    innerException: invalidDriverAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDriverAttachmentAsync(invalidDriverAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<DriverAttachment> addDriverAttachmentTask =
                this.driverAttachmentService.AddDriverAttachmentAsync(invalidDriverAttachment);

            DriverAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<DriverAttachmentValidationException>(
                  addDriverAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDriverAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAttachmentAsync(invalidDriverAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
