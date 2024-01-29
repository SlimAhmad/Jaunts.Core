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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomAttachmentId = default;
            Guid inputAttachmentId = randomAttachmentId;

            var invalidAttachmentException =
             new InvalidAttachmentException(
                 message: "Invalid attachment. Please correct the errors and try again.");

            invalidAttachmentException.AddData(
                  key: nameof(Attachment.Id),
                  values: "Id is required");

            var expectedAttachmentValidationException =
                new AttachmentValidationException(invalidAttachmentException);

            //when
            ValueTask<Attachment> retrieveAttachmentByIdTask =
                this.attachmentService.RetrieveAttachmentByIdAsync(inputAttachmentId);

            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  retrieveAttachmentByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageAttachmentIsNullAndLogItAsync()
        {
            //given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Attachment invalidStorageAttachment = null;
            var notFoundAttachmentException = 
                new NotFoundAttachmentException(
                    message: $"Couldn't find attachment with id: {inputAttachmentId}.");

            var expectedAttachmentValidationException =
                new AttachmentValidationException(
                    message: "Attachment validation error occurred, please try again.",
                    innerException: notFoundAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(inputAttachmentId))
                    .ReturnsAsync(invalidStorageAttachment);

            //when
            ValueTask<Attachment> retrieveAttachmentByIdTask =
                this.attachmentService.RetrieveAttachmentByIdAsync(inputAttachmentId);

            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  retrieveAttachmentByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
