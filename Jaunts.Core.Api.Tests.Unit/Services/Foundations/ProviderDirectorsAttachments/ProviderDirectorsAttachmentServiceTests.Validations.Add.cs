// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectorAttachments
{
    public partial class ProvidersDirectorAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenProvidersDirectorAttachmentIsNullAndLogItAsync()
        {
            // given
            ProvidersDirectorAttachment invalidProvidersDirectorAttachment = null;

            var nullProvidersDirectorAttachmentException = new NullProvidersDirectorAttachmentException(
                message: "The ProvidersDirectorAttachment is null.");

            var expectedProvidersDirectorAttachmentValidationException =
                new ProvidersDirectorAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: nullProvidersDirectorAttachmentException);

            // when
            ValueTask<ProvidersDirectorAttachment> addProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.AddProvidersDirectorAttachmentAsync(invalidProvidersDirectorAttachment);

            ProvidersDirectorAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProvidersDirectorAttachmentValidationException>(
                  addProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenProviderDirectorIdIsInvalidAndLogItAsync()
        {
            // given
            ProvidersDirectorAttachment randomProvidersDirectorAttachment = CreateRandomProvidersDirectorAttachment();
            ProvidersDirectorAttachment inputProvidersDirectorAttachment = randomProvidersDirectorAttachment;
            inputProvidersDirectorAttachment.ProviderDirectorId = default;

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
            ValueTask<ProvidersDirectorAttachment> addProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.AddProvidersDirectorAttachmentAsync(inputProvidersDirectorAttachment);

            ProvidersDirectorAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProvidersDirectorAttachmentValidationException>(
                  addProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            ProvidersDirectorAttachment randomProvidersDirectorAttachment = CreateRandomProvidersDirectorAttachment();
            ProvidersDirectorAttachment inputProvidersDirectorAttachment = randomProvidersDirectorAttachment;
            inputProvidersDirectorAttachment.AttachmentId = default;

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
            ValueTask<ProvidersDirectorAttachment> addProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.AddProvidersDirectorAttachmentAsync(inputProvidersDirectorAttachment);

            ProvidersDirectorAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProvidersDirectorAttachmentValidationException>(
                  addProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenProvidersDirectorAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            ProvidersDirectorAttachment randomProvidersDirectorAttachment = CreateRandomProvidersDirectorAttachment();
            ProvidersDirectorAttachment alreadyExistsProvidersDirectorAttachment = randomProvidersDirectorAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsProvidersDirectorAttachmentException =
                new AlreadyExistsProvidersDirectorAttachmentException(
                    message: "ProvidersDirectorAttachment  with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedProvidersDirectorAttachmentValidationException =
                new ProvidersDirectorAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: alreadyExistsProvidersDirectorAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProvidersDirectorAttachmentAsync(alreadyExistsProvidersDirectorAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ProvidersDirectorAttachment> addProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.AddProvidersDirectorAttachmentAsync(alreadyExistsProvidersDirectorAttachment);

            ProvidersDirectorAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProvidersDirectorAttachmentValidationException>(
                  addProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedProvidersDirectorAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAttachmentAsync(alreadyExistsProvidersDirectorAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferenceExceptionAndLogItAsync()
        {
            // given
            ProvidersDirectorAttachment randomProvidersDirectorAttachment = CreateRandomProvidersDirectorAttachment();
            ProvidersDirectorAttachment invalidProvidersDirectorAttachment = randomProvidersDirectorAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidProvidersDirectorAttachmentReferenceException =
                new InvalidProvidersDirectorAttachmentReferenceException(
                    message: "Invalid guardian attachment reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedProvidersDirectorAttachmentValidationException =
                new ProvidersDirectorAttachmentValidationException(
                    message: "Invalid input, contact support.", 
                    innerException: invalidProvidersDirectorAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProvidersDirectorAttachmentAsync(invalidProvidersDirectorAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<ProvidersDirectorAttachment> addProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.AddProvidersDirectorAttachmentAsync(invalidProvidersDirectorAttachment);

            ProvidersDirectorAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<ProvidersDirectorAttachmentValidationException>(
                  addProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedProvidersDirectorAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAttachmentAsync(invalidProvidersDirectorAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
