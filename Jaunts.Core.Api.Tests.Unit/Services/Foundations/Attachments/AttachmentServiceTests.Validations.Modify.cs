// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenAttachmentIsNullAndLogItAsync()
        {
            //given
            Attachment invalidAttachment = null;
            var nullAttachmentException = new NullAttachmentException();

            var expectedAttachmentValidationException =
                new AttachmentValidationException(
                    message: "Attachment validation error occurred, please try again.",
                    innerException: nullAttachmentException);

            //when
            ValueTask<Attachment> modifyAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(invalidAttachment);


            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  modifyAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfAttachmentIsInvalidAndLogItAsync(
                 string invalidText)
        {
            // given
            var invalidAttachment = new Attachment
            {
                Description = invalidText,
                Extension = invalidText,
                ExternalUrl = invalidText,
                Status = AttachmentStatus.Draft,
                Label = invalidText,
                ContentType = invalidText,
               
            };

            var invalidAttachmentException = new InvalidAttachmentException();

            invalidAttachmentException.AddData(
                key: nameof(Attachment.Id),
                values: "Id is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.Description),
                values: "Text is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.ExternalUrl),
                values: "Text is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.Extension),
                values: "Text is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.Status),
                values: "Text is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.Label),
                values: "Text is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.ContentType),
                values: "Text is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.CreatedDate),
                values: "Date is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(Attachment.CreatedDate)}");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.CreatedBy),
                values: "Id is required");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.UpdatedBy),
                values: "Id is required");

            var expectedAttachmentValidationException =
                new AttachmentValidationException(invalidAttachmentException);

            // when
            ValueTask<Attachment> createAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(invalidAttachment);

            // then
            await Assert.ThrowsAsync<AttachmentValidationException>(() =>
                createAttachmentTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttachmentAsync(It.IsAny<Attachment>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomAttachment(dateTime);
            Attachment inputAttachment = randomAttachment;

            var invalidAttachmentException =
             new InvalidAttachmentException(
                 message: "Invalid attachment. Please correct the errors and try again.");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.UpdatedDate),
                values: $"Date is the same as {nameof(Attachment.CreatedDate)}");

            var expectedAttachmentValidationException =
                new AttachmentValidationException(
                    message: "Attachment validation error occurred, please try again.", 
                    innerException: invalidAttachmentException);

            this.dateTimeBrokerMock.Setup(broker =>
                 broker.GetCurrentDateTime()).
                     Returns(dateTime);


            // when
            ValueTask<Attachment> modifyAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(inputAttachment);

            
            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  modifyAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomModifyAttachment(dateTime);
            Attachment inputAttachment = randomAttachment;
            inputAttachment.UpdatedBy = inputAttachment.CreatedBy;
            inputAttachment.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidAttachmentException =
                 new InvalidAttachmentException(
                     message: "Invalid attachment. Please correct the errors and try again.");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.UpdatedDate),
                values: "Date is not recent");

            var expectedAttachmentValidationException =
                new AttachmentValidationException(
                    message: "Attachment validation error occurred, please try again.", 
                    innerException: invalidAttachmentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Attachment> modifyAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(inputAttachment);

            
            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  modifyAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAttachmentDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomAttachment(dateTime);
            Attachment nonExistentAttachment = randomAttachment;
            nonExistentAttachment.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Attachment noAttachment = null;

            var notFoundAttachmentException =
                new NotFoundAttachmentException(
                     message: $"Couldn't find attachment with id: {nonExistentAttachment.Id}.");

            var expectedAttachmentValidationException =
                new AttachmentValidationException(
                    message: "Attachment validation error occurred, please try again.",
                    innerException: notFoundAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(nonExistentAttachment.Id))
                    .ReturnsAsync(noAttachment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Attachment> modifyAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(nonExistentAttachment);

            
            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  modifyAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(nonExistentAttachment.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetNegativeRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomModifyAttachment(randomDateTimeOffset);
            Attachment invalidAttachment = randomAttachment.DeepClone();
            Attachment storageAttachment = invalidAttachment.DeepClone();
            storageAttachment.CreatedDate = storageAttachment.CreatedDate.AddMinutes(randomMinutes);
            storageAttachment.UpdatedDate = storageAttachment.UpdatedDate.AddMinutes(randomMinutes);
            Guid attachmentId = invalidAttachment.Id;

            var invalidAttachmentException =
                 new InvalidAttachmentException(
                     message: "Invalid attachment. Please correct the errors and try again.");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.CreatedDate),
                values: $"Date is not the same as {nameof(Attachment.CreatedDate)}");

            var expectedAttachmentValidationException =
              new AttachmentValidationException(
                  message: "Attachment validation error occurred, please try again.",
                  innerException: invalidAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(attachmentId))
                    .ReturnsAsync(storageAttachment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Attachment> modifyAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(invalidAttachment);

            
            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  modifyAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(invalidAttachment.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetCurrentDateTime();
            Attachment randomAttachment = CreateRandomModifyAttachment(randomDate);
            Attachment invalidAttachment = randomAttachment;
            invalidAttachment.UpdatedDate = randomDate;
            Attachment storageAttachment = randomAttachment.DeepClone();
            Guid attachmentId = invalidAttachment.Id;

            var invalidAttachmentException =
             new InvalidAttachmentException(
                 message: "Invalid attachment. Please correct the errors and try again.");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.UpdatedDate),
                values: $"Date is the same as {nameof(invalidAttachment.UpdatedDate)}");

            var expectedAttachmentValidationException =
              new AttachmentValidationException(invalidAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(attachmentId))
                    .ReturnsAsync(storageAttachment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Attachment> modifyAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(invalidAttachment);

            
            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  modifyAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(invalidAttachment.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int randomPositiveMinutes = GetRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomModifyAttachment(randomDateTimeOffset);
            Attachment invalidAttachment = randomAttachment.DeepClone();
            Attachment storageAttachment = invalidAttachment.DeepClone();
            storageAttachment.UpdatedDate = storageAttachment.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid attachmentId = invalidAttachment.Id;
            invalidAttachment.CreatedBy = invalidCreatedBy;

            var invalidAttachmentException =
                 new InvalidAttachmentException(
                     message: "Invalid attachment. Please correct the errors and try again.");

            invalidAttachmentException.AddData(
                key: nameof(Attachment.CreatedBy),
                values: $"Id is not the same as {nameof(Attachment.CreatedBy)}");

            var expectedAttachmentValidationException =
              new AttachmentValidationException(invalidAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(attachmentId))
                    .ReturnsAsync(storageAttachment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Attachment> modifyAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(invalidAttachment);

            
            AttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AttachmentValidationException>(
                  modifyAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAttachmentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(invalidAttachment.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
