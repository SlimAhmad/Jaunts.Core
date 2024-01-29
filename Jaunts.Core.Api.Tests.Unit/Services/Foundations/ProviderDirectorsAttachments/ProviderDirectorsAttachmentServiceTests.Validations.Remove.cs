// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectorAttachments
{
    public partial class ProvidersDirectorAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenProviderDirectorIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomProviderDirectorId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputProviderDirectorId = randomProviderDirectorId;

            var invalidProvidersDirectorAttachmentException =
              new InvalidProvidersDirectorAttachmentException(
                  message: "Invalid ProvidersDirectorAttachment. Please fix the errors and try again.");

            invalidProvidersDirectorAttachmentException.AddData(
                key: nameof(ProvidersDirectorAttachment.ProviderDirectorId),
                values: "Id is required");

            var expectedProvidersDirectorAttachmentValidationException =
                new ProvidersDirectorAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidProvidersDirectorAttachmentException);

            // when
            ValueTask<ProvidersDirectorAttachment> removeProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.RemoveProvidersDirectorAttachmentByIdAsync(inputProviderDirectorId, inputAttachmentId);

            ProvidersDirectorAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProvidersDirectorAttachmentValidationException>(
                  removeProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()),
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
            Guid randomProviderDirectorId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputProviderDirectorId = randomProviderDirectorId;

            var invalidProvidersDirectorAttachmentException =
              new InvalidProvidersDirectorAttachmentException(
                  message: "Invalid ProvidersDirectorAttachment. Please fix the errors and try again.");

            invalidProvidersDirectorAttachmentException.AddData(
              key: nameof(ProvidersDirectorAttachment.AttachmentId),
              values: "Id is required");

            var expectedProvidersDirectorAttachmentValidationException =
                new ProvidersDirectorAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidProvidersDirectorAttachmentException);

            // when
            ValueTask<ProvidersDirectorAttachment> removeProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.RemoveProvidersDirectorAttachmentByIdAsync(inputProviderDirectorId, inputAttachmentId);

            ProvidersDirectorAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProvidersDirectorAttachmentValidationException>(
                  removeProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageProvidersDirectorAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ProvidersDirectorAttachment randomProvidersDirectorAttachment = CreateRandomProvidersDirectorAttachment(randomDateTime);
            Guid inputAttachmentId = randomProvidersDirectorAttachment.AttachmentId;
            Guid inputProviderDirectorId = randomProvidersDirectorAttachment.ProviderDirectorId;
            ProvidersDirectorAttachment nullStorageProvidersDirectorAttachment = null;

            var notFoundProvidersDirectorAttachmentException =
               new NotFoundProvidersDirectorAttachmentException(
                   message: $"Couldn't find attachment with ProviderDirectors id: {inputProviderDirectorId} " +
                        $"and attachment id: {inputAttachmentId}.");

            var expectedProviderDirectorsValidationException =
                new ProvidersDirectorAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    notFoundProvidersDirectorAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectProvidersDirectorAttachmentByIdAsync(inputProviderDirectorId, inputAttachmentId))
                    .ReturnsAsync(nullStorageProvidersDirectorAttachment);

            // when
            ValueTask<ProvidersDirectorAttachment> removeProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.RemoveProvidersDirectorAttachmentByIdAsync(inputProviderDirectorId, inputAttachmentId);

            ProvidersDirectorAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProvidersDirectorAttachmentValidationException>(
                  removeProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderDirectorsValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderDirectorsValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}