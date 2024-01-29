// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOfferAttachments
{
    public partial class PromosOfferAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenPromosOfferIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomPromosOfferId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputPromosOfferId = randomPromosOfferId;

            var invalidPromosOfferAttachmentException =
                 new InvalidPromosOfferAttachmentException(
                     message: "Invalid PromosOfferAttachment. Please correct the errors and try again.");

            invalidPromosOfferAttachmentException.AddData(
              key: nameof(PromosOfferAttachment.PromosOfferId),
              values: "Id is required");

            var expectedPromosOfferAttachmentValidationException =
                new PromosOfferAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidPromosOfferAttachmentException);

            // when
            ValueTask<PromosOfferAttachment> actualPromosOfferAttachmentTask =
                this.promosOfferAttachmentService.RetrievePromosOfferAttachmentByIdAsync(inputPromosOfferId, inputAttachmentId);

            PromosOfferAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PromosOfferAttachmentValidationException>(
                  actualPromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            Guid randomPromosOfferId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputPromosOfferId = randomPromosOfferId;

            var invalidPromosOfferAttachmentException =
                 new InvalidPromosOfferAttachmentException(
                     message: "Invalid PromosOfferAttachment. Please correct the errors and try again.");

            invalidPromosOfferAttachmentException.AddData(
              key: nameof(PromosOfferAttachment.AttachmentId),
              values: "Id is required");

            var expectedPromosOfferAttachmentValidationException =
                new PromosOfferAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidPromosOfferAttachmentException);

            // when
            ValueTask<PromosOfferAttachment> actualPromosOfferAttachmentTask =
                this.promosOfferAttachmentService.RetrievePromosOfferAttachmentByIdAsync(inputPromosOfferId, inputAttachmentId);

            PromosOfferAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PromosOfferAttachmentValidationException>(
                  actualPromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenStoragePromosOfferAttachmentIsInvalidAndLogItAsync()
        {
            // given
            PromosOfferAttachment randomPromosOfferAttachment = CreateRandomPromosOfferAttachment();
            Guid inputAttachmentId = randomPromosOfferAttachment.AttachmentId;
            Guid inputPromosOfferId = randomPromosOfferAttachment.PromosOfferId;
            PromosOfferAttachment nullStoragePromosOfferAttachment = null;

            var notFoundPromosOfferAttachmentException =
               new NotFoundPromosOfferAttachmentException(
                   message: $"Couldn't find attachment with PromosOffer id: {inputPromosOfferId} " +
                        $"and attachment id: {inputAttachmentId}.");

            var expectedPromosOfferValidationException =
                new PromosOfferAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    notFoundPromosOfferAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectPromosOfferAttachmentByIdAsync(inputPromosOfferId, inputAttachmentId))
                    .ReturnsAsync(nullStoragePromosOfferAttachment);

            // when
            ValueTask<PromosOfferAttachment> actualPromosOfferAttachmentRetrieveTask =
                this.promosOfferAttachmentService.RetrievePromosOfferAttachmentByIdAsync(inputPromosOfferId, inputAttachmentId);

            PromosOfferAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PromosOfferAttachmentValidationException>(
                  actualPromosOfferAttachmentRetrieveTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOfferValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
