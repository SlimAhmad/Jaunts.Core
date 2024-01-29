// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderAttachments
{
    public partial class ProviderAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenProviderIdIsInvalidAndLogItAsync()
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
            ValueTask<ProviderAttachment> actualProviderAttachmentTask =
                this.providerAttachmentService.RetrieveProviderAttachmentByIdAsync(inputProviderId, inputAttachmentId);

            ProviderAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProviderAttachmentValidationException>(
                  actualProviderAttachmentTask.AsTask);

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

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenAttachmentIdIsInvalidAndLogItAsync()
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
            ValueTask<ProviderAttachment> actualProviderAttachmentTask =
                this.providerAttachmentService.RetrieveProviderAttachmentByIdAsync(inputProviderId, inputAttachmentId);

            ProviderAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProviderAttachmentValidationException>(
                  actualProviderAttachmentTask.AsTask);

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

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageProviderAttachmentIsInvalidAndLogItAsync()
        {
            // given
            ProviderAttachment randomProviderAttachment = CreateRandomProviderAttachment();
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
            ValueTask<ProviderAttachment> actualProviderAttachmentRetrieveTask =
                this.providerAttachmentService.RetrieveProviderAttachmentByIdAsync(inputProviderId, inputAttachmentId);

            ProviderAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProviderAttachmentValidationException>(
                  actualProviderAttachmentRetrieveTask.AsTask);

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

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
