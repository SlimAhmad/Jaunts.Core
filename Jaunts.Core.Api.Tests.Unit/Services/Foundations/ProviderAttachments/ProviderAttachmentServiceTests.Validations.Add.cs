// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderAttachments
{
    public partial class ProviderAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenProviderAttachmentIsNullAndLogItAsync()
        {
            // given
            ProviderAttachment invalidProviderAttachment = null;

            var nullProviderAttachmentException = new NullProviderAttachmentException(
                message: "The ProviderAttachment is null.");

            var expectedProviderAttachmentValidationException =
                new ProviderAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: nullProviderAttachmentException);

            // when
            ValueTask<ProviderAttachment> addProviderAttachmentTask =
                this.providerAttachmentService.AddProviderAttachmentAsync(invalidProviderAttachment);

            ProviderAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProviderAttachmentValidationException>(
                  addProviderAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAttachmentAsync(It.IsAny<ProviderAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenProviderIdIsInvalidAndLogItAsync()
        {
            // given
            ProviderAttachment randomProviderAttachment = CreateRandomProviderAttachment();
            ProviderAttachment inputProviderAttachment = randomProviderAttachment;
            inputProviderAttachment.ProviderId = default;

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
            ValueTask<ProviderAttachment> addProviderAttachmentTask =
                this.providerAttachmentService.AddProviderAttachmentAsync(inputProviderAttachment);

            ProviderAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProviderAttachmentValidationException>(
                  addProviderAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAttachmentAsync(It.IsAny<ProviderAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            ProviderAttachment randomProviderAttachment = CreateRandomProviderAttachment();
            ProviderAttachment inputProviderAttachment = randomProviderAttachment;
            inputProviderAttachment.AttachmentId = default;

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
            ValueTask<ProviderAttachment> addProviderAttachmentTask =
                this.providerAttachmentService.AddProviderAttachmentAsync(inputProviderAttachment);

            ProviderAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProviderAttachmentValidationException>(
                  addProviderAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAttachmentAsync(It.IsAny<ProviderAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenProviderAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            ProviderAttachment randomProviderAttachment = CreateRandomProviderAttachment();
            ProviderAttachment alreadyExistsProviderAttachment = randomProviderAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsProviderAttachmentException =
                new AlreadyExistsProviderAttachmentException(
                    message: "ProviderAttachment  with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedProviderAttachmentValidationException =
                new ProviderAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: alreadyExistsProviderAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProviderAttachmentAsync(alreadyExistsProviderAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ProviderAttachment> addProviderAttachmentTask =
                this.providerAttachmentService.AddProviderAttachmentAsync(alreadyExistsProviderAttachment);

            ProviderAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProviderAttachmentValidationException>(
                  addProviderAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedProviderAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAttachmentAsync(alreadyExistsProviderAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferenceExceptionAndLogItAsync()
        {
            // given
            ProviderAttachment randomProviderAttachment = CreateRandomProviderAttachment();
            ProviderAttachment invalidProviderAttachment = randomProviderAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidProviderAttachmentReferenceException =
                new InvalidProviderAttachmentReferenceException(
                    message: "Invalid guardian attachment reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedProviderAttachmentValidationException =
                new ProviderAttachmentValidationException(
                    message: "Invalid input, contact support.", 
                    innerException: invalidProviderAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProviderAttachmentAsync(invalidProviderAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<ProviderAttachment> addProviderAttachmentTask =
                this.providerAttachmentService.AddProviderAttachmentAsync(invalidProviderAttachment);

            ProviderAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProviderAttachmentValidationException>(
                  addProviderAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedProviderAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAttachmentAsync(invalidProviderAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
