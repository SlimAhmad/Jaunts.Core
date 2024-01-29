// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = default;
            Guid inputAttachmentId = randomAttachmentId;

            var invalidAttachmentException =
              new InvalidAttachmentException(
                  message: "Invalid attachment. Please correct the errors and try again.");

            invalidAttachmentException.AddData(
                  key: nameof(Attachment.Id),
                  values: "Id is required");

            var expectedAttachmentValidationException =
                new AttachmentValidationException(
                    message: "Attachment validation error occurred, please try again.",
                    invalidAttachmentException);

            // when
            ValueTask<Attachment> actualAttachmentTask =
                this.attachmentService.RemoveAttachmentByIdAsync(inputAttachmentId);

            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  actualAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAttachmentAsync(It.IsAny<Attachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomAttachment(dates: dateTimeOffset);
            Guid inputAttachmentId = randomAttachment.Id;
            Attachment inputAttachment = randomAttachment;
            Attachment nullStorageAttachment = null;

            var notFoundAttachmentException = 
                new NotFoundAttachmentException(
                    message: $"Couldn't find attachment with id: {inputAttachment.Id}.");

            var expectedAttachmentValidationException =
                new AttachmentValidationException(
                    message: "Attachment validation error occurred, please try again.",
                    innerException: notFoundAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(inputAttachmentId))
                    .ReturnsAsync(nullStorageAttachment);

            // when
            ValueTask<Attachment> actualAttachmentTask =
                this.attachmentService.RemoveAttachmentByIdAsync(inputAttachmentId);

            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  actualAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAttachmentAsync(It.IsAny<Attachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
