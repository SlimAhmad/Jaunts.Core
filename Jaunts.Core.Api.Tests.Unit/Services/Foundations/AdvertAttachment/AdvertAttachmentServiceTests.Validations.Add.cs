// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.AdvertAttachments
{
    public partial class AdvertAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAdvertAttachmentIsNullAndLogItAsync()
        {
            // given
            AdvertAttachment invalidAdvertAttachment = null;

            var nullAdvertAttachmentException = new NullAdvertAttachmentException(
                message: "The AdvertAttachment is null.");

            var expectedAdvertAttachmentValidationException =
                new AdvertAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: nullAdvertAttachmentException);

            // when
            ValueTask<AdvertAttachment> addAdvertAttachmentTask =
                this.AdvertAttachmentService.AddAdvertAttachmentAsync(invalidAdvertAttachment);

            AdvertAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AdvertAttachmentValidationException>(
                  addAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAdvertIdIsInvalidAndLogItAsync()
        {
            // given
            AdvertAttachment randomAdvertAttachment = CreateRandomAdvertAttachment();
            AdvertAttachment inputAdvertAttachment = randomAdvertAttachment;
            inputAdvertAttachment.AdvertId = default;

            var invalidAdvertAttachmentException =
              new InvalidAdvertAttachmentException(
                  message: "Invalid AdvertAttachment. Please correct the errors and try again.");

            invalidAdvertAttachmentException.AddData(
                key: nameof(AdvertAttachment.AdvertId),
                values: "Id is required");

            var expectedAdvertAttachmentValidationException =
                new AdvertAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidAdvertAttachmentException);

            // when
            ValueTask<AdvertAttachment> addAdvertAttachmentTask =
                this.AdvertAttachmentService.AddAdvertAttachmentAsync(inputAdvertAttachment);

            AdvertAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AdvertAttachmentValidationException>(
                  addAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            AdvertAttachment randomAdvertAttachment = CreateRandomAdvertAttachment();
            AdvertAttachment inputAdvertAttachment = randomAdvertAttachment;
            inputAdvertAttachment.AttachmentId = default;

            var invalidAdvertAttachmentException =
                new InvalidAdvertAttachmentException(
                    message: "Invalid AdvertAttachment. Please correct the errors and try again.");

            invalidAdvertAttachmentException.AddData(
                key: nameof(AdvertAttachment.AttachmentId),
                values: "Id is required");

            var expectedAdvertAttachmentValidationException =
                new AdvertAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidAdvertAttachmentException);

            // when
            ValueTask<AdvertAttachment> addAdvertAttachmentTask =
                this.AdvertAttachmentService.AddAdvertAttachmentAsync(inputAdvertAttachment);

            AdvertAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AdvertAttachmentValidationException>(
                  addAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAdvertAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            AdvertAttachment randomAdvertAttachment = CreateRandomAdvertAttachment();
            AdvertAttachment alreadyExistsAdvertAttachment = randomAdvertAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsAdvertAttachmentException =
                new AlreadyExistsAdvertAttachmentException(
                    message: "AdvertAttachment  with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedAdvertAttachmentValidationException =
                new AdvertAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: alreadyExistsAdvertAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAdvertAttachmentAsync(alreadyExistsAdvertAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<AdvertAttachment> addAdvertAttachmentTask =
                this.AdvertAttachmentService.AddAdvertAttachmentAsync(alreadyExistsAdvertAttachment);

            AdvertAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AdvertAttachmentValidationException>(
                  addAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAdvertAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAttachmentAsync(alreadyExistsAdvertAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferenceExceptionAndLogItAsync()
        {
            // given
            AdvertAttachment randomAdvertAttachment = CreateRandomAdvertAttachment();
            AdvertAttachment invalidAdvertAttachment = randomAdvertAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidAdvertAttachmentReferenceException =
                new InvalidAdvertAttachmentReferenceException(
                    message: "Invalid guardian attachment reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedAdvertAttachmentValidationException =
                new AdvertAttachmentValidationException(
                    message: "Invalid input, contact support.", 
                    innerException: invalidAdvertAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAdvertAttachmentAsync(invalidAdvertAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<AdvertAttachment> addAdvertAttachmentTask =
                this.AdvertAttachmentService.AddAdvertAttachmentAsync(invalidAdvertAttachment);

            AdvertAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AdvertAttachmentValidationException>(
                  addAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAdvertAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAttachmentAsync(invalidAdvertAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
