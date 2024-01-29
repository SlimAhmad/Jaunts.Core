// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attachment someAttachment = CreateRandomAttachment(dateTime);
            someAttachment.UpdatedBy = someAttachment.CreatedBy;
            someAttachment.UpdatedDate = someAttachment.CreatedDate;
            var sqlException = GetSqlException();

            var failedAttachmentStorageException =
                new FailedAttachmentStorageException(
                    message: "Failed Attachment storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(
                    message: "Attachment dependency error occurred, contact support.",
                    innerException: failedAttachmentStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAttachmentAsync(It.IsAny<Attachment>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Attachment> createAttachmentTask =
                this.attachmentService.AddAttachmentAsync(someAttachment);

            AttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<AttachmentDependencyException>(
                     createAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAttachmentDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttachmentAsync(It.IsAny<Attachment>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAttachmentDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attachment someAttachment = CreateRandomAttachment(dateTime);
            someAttachment.UpdatedBy = someAttachment.CreatedBy;
            someAttachment.UpdatedDate = someAttachment.CreatedDate;
            var databaseUpdateException = new DbUpdateException();

            var failedAttachmentStorageException =
                 new FailedAttachmentStorageException(
                     message: "Failed Attachment storage error occurred, contact support.",
                     innerException: databaseUpdateException);

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(
                    message: "Attachment dependency error occurred, contact support.",
                    innerException: failedAttachmentStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAttachmentAsync(It.IsAny<Attachment>()))
                     .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Attachment> createAttachmentTask =
                this.attachmentService.AddAttachmentAsync(someAttachment);


            AttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<AttachmentDependencyException>(
                     createAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAttachmentDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttachmentAsync(It.IsAny<Attachment>()),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attachment someAttachment = CreateRandomAttachment(dateTime);
            someAttachment.UpdatedBy = someAttachment.CreatedBy;
            someAttachment.UpdatedDate = someAttachment.CreatedDate;
            var serviceException = new Exception();

            var failedAttachmentServiceException =
                  new FailedAttachmentServiceException(
                      message: "Failed Attachment Service Exception occurred,contact support.",
                      innerException: serviceException);

            var expectedAttachmentServiceException =
                new AttachmentServiceException(
                    message: "Attachment Service error occurred, contact support.",
                    innerException: failedAttachmentServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAttachmentAsync(It.IsAny<Attachment>()))
                     .ThrowsAsync(serviceException);

            // when
            ValueTask<Attachment> createAttachmentTask =
                 this.attachmentService.AddAttachmentAsync(someAttachment);

            AttachmentServiceException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<AttachmentServiceException>(
                     createAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAttachmentServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttachmentAsync(It.IsAny<Attachment>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
