// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLetAttachments
{
    public partial class ShortLetAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenShortLetAttachmentIsNullAndLogItAsync()
        {
            // given
            ShortLetAttachment invalidShortLetAttachment = null;

            var nullShortLetAttachmentException = new NullShortLetAttachmentException(
                message: "The ShortLetAttachment is null.");

            var expectedShortLetAttachmentValidationException =
                new ShortLetAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: nullShortLetAttachmentException);

            // when
            ValueTask<ShortLetAttachment> addShortLetAttachmentTask =
                this.shortLetAttachmentService.AddShortLetAttachmentAsync(invalidShortLetAttachment);

            ShortLetAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ShortLetAttachmentValidationException>(
                  addShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenShortLetIdIsInvalidAndLogItAsync()
        {
            // given
            ShortLetAttachment randomShortLetAttachment = CreateRandomShortLetAttachment();
            ShortLetAttachment inputShortLetAttachment = randomShortLetAttachment;
            inputShortLetAttachment.ShortLetId = default;

            var invalidShortLetAttachmentException =
              new InvalidShortLetAttachmentException(
                  message: "Invalid ShortLetAttachment. Please fix the errors and try again.");

            invalidShortLetAttachmentException.AddData(
                key: nameof(ShortLetAttachment.ShortLetId),
                values: "Id is required");

            var expectedShortLetAttachmentValidationException =
                new ShortLetAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidShortLetAttachmentException);

            // when
            ValueTask<ShortLetAttachment> addShortLetAttachmentTask =
                this.shortLetAttachmentService.AddShortLetAttachmentAsync(inputShortLetAttachment);

            ShortLetAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ShortLetAttachmentValidationException>(
                  addShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            ShortLetAttachment randomShortLetAttachment = CreateRandomShortLetAttachment();
            ShortLetAttachment inputShortLetAttachment = randomShortLetAttachment;
            inputShortLetAttachment.AttachmentId = default;

            var invalidShortLetAttachmentException =
                new InvalidShortLetAttachmentException(
                    message: "Invalid ShortLetAttachment. Please fix the errors and try again.");

            invalidShortLetAttachmentException.AddData(
                key: nameof(ShortLetAttachment.AttachmentId),
                values: "Id is required");

            var expectedShortLetAttachmentValidationException =
                new ShortLetAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidShortLetAttachmentException);

            // when
            ValueTask<ShortLetAttachment> addShortLetAttachmentTask =
                this.shortLetAttachmentService.AddShortLetAttachmentAsync(inputShortLetAttachment);

            ShortLetAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ShortLetAttachmentValidationException>(
                  addShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenShortLetAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            ShortLetAttachment randomShortLetAttachment = CreateRandomShortLetAttachment();
            ShortLetAttachment alreadyExistsShortLetAttachment = randomShortLetAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsShortLetAttachmentException =
                new AlreadyExistsShortLetAttachmentException(
                    message: "ShortLetAttachment  with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedShortLetAttachmentValidationException =
                new ShortLetAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: alreadyExistsShortLetAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertShortLetAttachmentAsync(alreadyExistsShortLetAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ShortLetAttachment> addShortLetAttachmentTask =
                this.shortLetAttachmentService.AddShortLetAttachmentAsync(alreadyExistsShortLetAttachment);

            ShortLetAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ShortLetAttachmentValidationException>(
                  addShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedShortLetAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAttachmentAsync(alreadyExistsShortLetAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferenceExceptionAndLogItAsync()
        {
            // given
            ShortLetAttachment randomShortLetAttachment = CreateRandomShortLetAttachment();
            ShortLetAttachment invalidShortLetAttachment = randomShortLetAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidShortLetAttachmentReferenceException =
                new InvalidShortLetAttachmentReferenceException(
                    message: "Invalid ShortLet attachment reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedShortLetAttachmentValidationException =
                new ShortLetAttachmentValidationException(
                    message: "Invalid input, contact support.", 
                    innerException: invalidShortLetAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertShortLetAttachmentAsync(invalidShortLetAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<ShortLetAttachment> addShortLetAttachmentTask =
                this.shortLetAttachmentService.AddShortLetAttachmentAsync(invalidShortLetAttachment);

            ShortLetAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ShortLetAttachmentValidationException>(
                  addShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedShortLetAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAttachmentAsync(invalidShortLetAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
