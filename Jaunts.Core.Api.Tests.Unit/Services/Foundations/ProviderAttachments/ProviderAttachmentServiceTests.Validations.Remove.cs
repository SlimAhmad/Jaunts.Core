// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderAttachments
{
    public partial class ProviderAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenProviderIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomProviderId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputProviderId = randomProviderId;

            var invalidProviderAttachmentException =
              new InvalidProviderAttachmentException(
                  message: "Invalid providerAttachment. Please correct the errors and try again.");

            invalidProviderAttachmentException.AddData(
                key: nameof(ProviderAttachment.ProviderId),
                values: "Id is required");

            var expectedProviderAttachmentValidationException =
                new ProviderAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidProviderAttachmentException);

            // when
            ValueTask<ProviderAttachment> removeProviderAttachmentTask =
                this.providerAttachmentService.RemoveProviderAttachmentByIdAsync(inputProviderId, inputAttachmentId);

            ProviderAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProviderAttachmentValidationException>(
                  removeProviderAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderAttachmentAsync(It.IsAny<ProviderAttachment>()),
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
            Guid randomProviderId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputProviderId = randomProviderId;

            var invalidProviderAttachmentException =
              new InvalidProviderAttachmentException(
                  message: "Invalid providerAttachment. Please correct the errors and try again.");

            invalidProviderAttachmentException.AddData(
              key: nameof(ProviderAttachment.AttachmentId),
              values: "Id is required");

            var expectedProviderAttachmentValidationException =
                new ProviderAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidProviderAttachmentException);

            // when
            ValueTask<ProviderAttachment> removeProviderAttachmentTask =
                this.providerAttachmentService.RemoveProviderAttachmentByIdAsync(inputProviderId, inputAttachmentId);

            ProviderAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProviderAttachmentValidationException>(
                  removeProviderAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderAttachmentAsync(It.IsAny<ProviderAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageProviderAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ProviderAttachment randomProviderAttachment = CreateRandomProviderAttachment(randomDateTime);
            Guid inputAttachmentId = randomProviderAttachment.AttachmentId;
            Guid inputProviderId = randomProviderAttachment.ProviderId;
            ProviderAttachment nullStorageProviderAttachment = null;

            var notFoundProviderAttachmentException =
               new NotFoundProviderAttachmentException(
                   message: $"Couldn't find attachment with provider id: {inputProviderId} " +
                        $"and attachment id: {inputAttachmentId}.");

            var expectedProviderValidationException =
                new ProviderAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    notFoundProviderAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectProviderAttachmentByIdAsync(inputProviderId, inputAttachmentId))
                    .ReturnsAsync(nullStorageProviderAttachment);

            // when
            ValueTask<ProviderAttachment> removeProviderAttachmentTask =
                this.providerAttachmentService.RemoveProviderAttachmentByIdAsync(inputProviderId, inputAttachmentId);

            ProviderAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProviderAttachmentValidationException>(
                  removeProviderAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderAttachmentAsync(It.IsAny<ProviderAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}