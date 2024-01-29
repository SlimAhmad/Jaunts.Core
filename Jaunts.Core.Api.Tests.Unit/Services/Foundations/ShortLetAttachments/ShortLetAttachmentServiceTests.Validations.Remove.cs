// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLetAttachments
{
    public partial class ShortLetAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenShortLetIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomShortLetId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputShortLetId = randomShortLetId;

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
            ValueTask<ShortLetAttachment> removeShortLetAttachmentTask =
                this.shortLetAttachmentService.RemoveShortLetAttachmentByIdAsync(inputShortLetId, inputAttachmentId);

            ShortLetAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ShortLetAttachmentValidationException>(
                  removeShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()),
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
            Guid randomShortLetId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputShortLetId = randomShortLetId;

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
            ValueTask<ShortLetAttachment> removeShortLetAttachmentTask =
                this.shortLetAttachmentService.RemoveShortLetAttachmentByIdAsync(inputShortLetId, inputAttachmentId);

            ShortLetAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ShortLetAttachmentValidationException>(
                  removeShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageShortLetAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ShortLetAttachment randomShortLetAttachment = CreateRandomShortLetAttachment(randomDateTime);
            Guid inputAttachmentId = randomShortLetAttachment.AttachmentId;
            Guid inputShortLetId = randomShortLetAttachment.ShortLetId;
            ShortLetAttachment nullStorageShortLetAttachment = null;

            var notFoundShortLetAttachmentException =
               new NotFoundShortLetAttachmentException(
                   message: $"Couldn't find attachment with ShortLet id: {inputShortLetId} " +
                        $"and attachment id: {inputAttachmentId}.");

            var expectedShortLetValidationException =
                new ShortLetAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    notFoundShortLetAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectShortLetAttachmentByIdAsync(inputShortLetId, inputAttachmentId))
                    .ReturnsAsync(nullStorageShortLetAttachment);

            // when
            ValueTask<ShortLetAttachment> removeShortLetAttachmentTask =
                this.shortLetAttachmentService.RemoveShortLetAttachmentByIdAsync(inputShortLetId, inputAttachmentId);

            ShortLetAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ShortLetAttachmentValidationException>(
                  removeShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}