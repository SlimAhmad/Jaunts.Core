// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectorAttachments
{
    public partial class ProvidersDirectorAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenProviderDirectorsIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomProviderDirectorsId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputProviderDirectorsId = randomProviderDirectorsId;

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
            ValueTask<ProvidersDirectorAttachment> actualProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.RetrieveProvidersDirectorAttachmentByIdAsync(inputProviderDirectorsId, inputAttachmentId);

           ProvidersDirectorAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProvidersDirectorAttachmentValidationException>(
                  actualProvidersDirectorAttachmentTask.AsTask);

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

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = default;
            Guid randomProviderDirectorsId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputProviderDirectorsId = randomProviderDirectorsId;

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
            ValueTask<ProvidersDirectorAttachment> actualProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.RetrieveProvidersDirectorAttachmentByIdAsync(inputProviderDirectorsId, inputAttachmentId);

            ProvidersDirectorAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProvidersDirectorAttachmentValidationException>(
                  actualProvidersDirectorAttachmentTask.AsTask);

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

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageProvidersDirectorAttachmentIsInvalidAndLogItAsync()
        {
            // given
            ProvidersDirectorAttachment randomProvidersDirectorAttachment = CreateRandomProvidersDirectorAttachment();
            Guid inputAttachmentId = randomProvidersDirectorAttachment.AttachmentId;
            Guid inputProviderDirectorsId = randomProvidersDirectorAttachment.ProviderDirectorId;
            ProvidersDirectorAttachment nullStorageProvidersDirectorAttachment = null;

            var notFoundProvidersDirectorAttachmentException =
               new NotFoundProvidersDirectorAttachmentException(
                   message: $"Couldn't find attachment with ProviderDirectors id: {inputProviderDirectorsId} " +
                        $"and attachment id: {inputAttachmentId}.");

            var expectedProviderDirectorsValidationException =
                new ProvidersDirectorAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    notFoundProvidersDirectorAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectProvidersDirectorAttachmentByIdAsync(inputProviderDirectorsId, inputAttachmentId))
                    .ReturnsAsync(nullStorageProvidersDirectorAttachment);

            // when
            ValueTask<ProvidersDirectorAttachment> actualProvidersDirectorAttachmentRetrieveTask =
                this.providersDirectorAttachmentService.RetrieveProvidersDirectorAttachmentByIdAsync(inputProviderDirectorsId, inputAttachmentId);

            ProvidersDirectorAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProvidersDirectorAttachmentValidationException>(
                  actualProvidersDirectorAttachmentRetrieveTask.AsTask);

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

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
