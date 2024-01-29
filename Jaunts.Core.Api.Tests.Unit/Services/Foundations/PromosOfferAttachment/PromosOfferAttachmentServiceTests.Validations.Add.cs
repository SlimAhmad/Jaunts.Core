// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOfferAttachments
{
    public partial class PromosOfferAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenPromosOfferAttachmentIsNullAndLogItAsync()
        {
            // given
            PromosOfferAttachment invalidPromosOfferAttachment = null;

            var nullPromosOfferAttachmentException = new NullPromosOfferAttachmentException(
                message: "The PromosOfferAttachment is null.");

            var expectedPromosOfferAttachmentValidationException =
                new PromosOfferAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: nullPromosOfferAttachmentException);

            // when
            ValueTask<PromosOfferAttachment> addPromosOfferAttachmentTask =
                this.promosOfferAttachmentService.AddPromosOfferAttachmentAsync(invalidPromosOfferAttachment);

            PromosOfferAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PromosOfferAttachmentValidationException>(
                  addPromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAttachmentAsync(It.IsAny<PromosOfferAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenPromosOfferIdIsInvalidAndLogItAsync()
        {
            // given
            PromosOfferAttachment randomPromosOfferAttachment = CreateRandomPromosOfferAttachment();
            PromosOfferAttachment inputPromosOfferAttachment = randomPromosOfferAttachment;
            inputPromosOfferAttachment.PromosOfferId = default;

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
            ValueTask<PromosOfferAttachment> addPromosOfferAttachmentTask =
                this.promosOfferAttachmentService.AddPromosOfferAttachmentAsync(inputPromosOfferAttachment);

            PromosOfferAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PromosOfferAttachmentValidationException>(
                  addPromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAttachmentAsync(It.IsAny<PromosOfferAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            PromosOfferAttachment randomPromosOfferAttachment = CreateRandomPromosOfferAttachment();
            PromosOfferAttachment inputPromosOfferAttachment = randomPromosOfferAttachment;
            inputPromosOfferAttachment.AttachmentId = default;

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
            ValueTask<PromosOfferAttachment> addPromosOfferAttachmentTask =
                this.promosOfferAttachmentService.AddPromosOfferAttachmentAsync(inputPromosOfferAttachment);

            PromosOfferAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PromosOfferAttachmentValidationException>(
                  addPromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAttachmentAsync(It.IsAny<PromosOfferAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenPromosOfferAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            PromosOfferAttachment randomPromosOfferAttachment = CreateRandomPromosOfferAttachment();
            PromosOfferAttachment alreadyExistsPromosOfferAttachment = randomPromosOfferAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsPromosOfferAttachmentException =
                new AlreadyExistsPromosOfferAttachmentException(
                    message: "PromosOfferAttachment  with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedPromosOfferAttachmentValidationException =
                new PromosOfferAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: alreadyExistsPromosOfferAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPromosOfferAttachmentAsync(alreadyExistsPromosOfferAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<PromosOfferAttachment> addPromosOfferAttachmentTask =
                this.promosOfferAttachmentService.AddPromosOfferAttachmentAsync(alreadyExistsPromosOfferAttachment);

            PromosOfferAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PromosOfferAttachmentValidationException>(
                  addPromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedPromosOfferAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAttachmentAsync(alreadyExistsPromosOfferAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferenceExceptionAndLogItAsync()
        {
            // given
            PromosOfferAttachment randomPromosOfferAttachment = CreateRandomPromosOfferAttachment();
            PromosOfferAttachment invalidPromosOfferAttachment = randomPromosOfferAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidPromosOfferAttachmentReferenceException =
                new InvalidPromosOfferAttachmentReferenceException(
                    message: "Invalid guardian attachment reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedPromosOfferAttachmentValidationException =
                new PromosOfferAttachmentValidationException(
                    message: "Invalid input, contact support.", 
                    innerException: invalidPromosOfferAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPromosOfferAttachmentAsync(invalidPromosOfferAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<PromosOfferAttachment> addPromosOfferAttachmentTask =
                this.promosOfferAttachmentService.AddPromosOfferAttachmentAsync(invalidPromosOfferAttachment);

            PromosOfferAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PromosOfferAttachmentValidationException>(
                  addPromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedPromosOfferAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAttachmentAsync(invalidPromosOfferAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
