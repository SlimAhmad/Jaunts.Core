// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
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
        public async void ShouldThrowValidationExceptionOnCreateWhenAttachmentIsNullAndLogItAsync()
        {
            // given
            Attachment invalidAttachment = null;

            var nullAttachmentException = new NullAttachmentException(
                message: "The attachment is null.");

            var expectedAttachmentValidationException =
               new AttachmentValidationException(
                   message: "Attachment validation error occurred, please try again.",
                   innerException: nullAttachmentException);

            // when
            ValueTask<Attachment> createAttachmentTask =
                this.attachmentService.AddAttachmentAsync(invalidAttachment);

            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  createAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateIfAttachmentStatusIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomAttachment(randomDateTime);
            Attachment invalidAttachment = randomAttachment;
            invalidAttachment.UpdatedBy = randomAttachment.CreatedBy;
            invalidAttachment.Status = GetInvalidEnum<AttachmentStatus>();

            var invalidAttachmentException = new InvalidAttachmentException(
                message: "Invalid attachment. Please correct the errors and try again.");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.Status),
                values: "Value is not recognized");

            var expectedAttachmentValidationException = new AttachmentValidationException(
                message: "Attachment validation error occurred, please try again.",
                innerException: invalidAttachmentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<Attachment> createAttachmentTask =
                this.attachmentService.AddAttachmentAsync(invalidAttachment);

            AttachmentValidationException actualAttachmentDependencyValidationException =
            await Assert.ThrowsAsync<AttachmentValidationException>(
                createAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttachmentAsync(It.IsAny<Attachment>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenAttachmentIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidAttachment = new Attachment
            {
                Extension = invalidText,
                ContentType = invalidText,
                ExternalUrl = invalidText,
                Label = invalidText,
                Description = invalidText
            };

            var invalidAttachmentException = new InvalidAttachmentException();

            invalidAttachmentException.AddData(
                key: nameof(Attachment.Id),
                values: "Id is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.Extension),
                values: "Text is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.ContentType),
                values: "Text is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.ExternalUrl),
                values: "Text is required");

            invalidAttachmentException.AddData(
               key: nameof(Attachment.Label),
               values: "Text is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.Description),
                values: "Text is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.CreatedBy),
                values: "Id is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.UpdatedBy),
                values: "Id is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.CreatedDate),
                values: "Date is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.UpdatedDate),
                values: "Date is required");

            var expectedAttachmentValidationException =
                new AttachmentValidationException(
                    message: "Attachment validation error occurred, please try again.",
                    innerException: invalidAttachmentException);

            // when
            ValueTask<Attachment> createAttachmentTask =
                this.attachmentService.AddAttachmentAsync(invalidAttachment);

            AttachmentValidationException actualAttachmentDependencyValidationException =
            await Assert.ThrowsAsync<AttachmentValidationException>(
                createAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomAttachment(dateTime);
            Attachment inputAttachment = randomAttachment;
            inputAttachment.UpdatedBy = Guid.NewGuid();

            var invalidAttachmentException =
                 new InvalidAttachmentException(
                     message: "Invalid attachment. Please correct the errors and try again.");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.UpdatedBy),
                values: $"Id is not the same as {nameof(Attachment.CreatedBy)}");

            var expectedAttachmentValidationException =
                new AttachmentValidationException(
                    message: "Attachment validation error occurred, please try again.",
                    innerException: invalidAttachmentException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime()).
                 Returns(dateTime);

            // when
            ValueTask<Attachment> createAttachmentTask =
                this.attachmentService.AddAttachmentAsync(inputAttachment);

            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  createAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomAttachment(dateTime);
            Attachment inputAttachment = randomAttachment;
            inputAttachment.UpdatedBy = randomAttachment.CreatedBy;
            inputAttachment.UpdatedDate = GetRandomDateTime();

            var invalidAttachmentException =
                 new InvalidAttachmentException(
                     message: "Invalid attachment. Please correct the errors and try again.");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.UpdatedDate),
                values: $"Date is not the same as {nameof(Attachment.CreatedDate)}");

            var expectedAttachmentValidationException =
                new AttachmentValidationException(
                    message: "Attachment validation error occurred, please try again.",
                    innerException: invalidAttachmentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(dateTime);

            // when
            ValueTask<Attachment> createAttachmentTask =
                this.attachmentService.AddAttachmentAsync(inputAttachment);

            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  createAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomAttachment(dateTime);
            Attachment inputAttachment = randomAttachment;
            inputAttachment.UpdatedBy = inputAttachment.CreatedBy;
            inputAttachment.CreatedDate = dateTime.AddMinutes(minutes);
            inputAttachment.UpdatedDate = inputAttachment.CreatedDate;

            var invalidAttachmentException =
                  new InvalidAttachmentException(
                      message: "Invalid attachment. Please correct the errors and try again.");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.CreatedDate),
                values: "Date is not recent");

            var expectedAttachmentValidationException =
                new AttachmentValidationException(
                    message: "Attachment validation error occurred, please try again.",
                    innerException: invalidAttachmentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Attachment> createAttachmentTask =
                this.attachmentService.AddAttachmentAsync(inputAttachment);

            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  createAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomAttachment(dateTime);
            Attachment alreadyExistsAttachment = randomAttachment;
            alreadyExistsAttachment.UpdatedBy = alreadyExistsAttachment.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsAttachmentException =
                new AlreadyExistsAttachmentException(
                    message: "Attachment with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedAttachmentValidationException =
                new AttachmentValidationException(
                    message: "Attachment validation error occurred, please try again.",
                    innerException: alreadyExistsAttachmentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAttachmentAsync(It.IsAny<Attachment>()))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Attachment> createAttachmentTask =
                this.attachmentService.AddAttachmentAsync(alreadyExistsAttachment);

            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  createAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttachmentAsync(It.IsAny<Attachment>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAttachmentValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
