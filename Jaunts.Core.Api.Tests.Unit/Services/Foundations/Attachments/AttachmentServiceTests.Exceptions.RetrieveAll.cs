// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var failedAttachmentStorageException =
               new FailedAttachmentStorageException(
                   message: "Failed Attachment storage error occurred, contact support.",
                   innerException: sqlException);

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(
                    message: "Attachment dependency error occurred, contact support.",
                    innerException: failedAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllAttachmentsAction = () =>
                this.attachmentService.RetrieveAllAttachments();

            AttachmentDependencyException actualAttachmentDependencyException =
               Assert.Throws<AttachmentDependencyException>(
                 retrieveAllAttachmentsAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAttachmentDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAttachmentDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedAttachmentServiceException =
                new FailedAttachmentServiceException(
                    message: "Failed Attachment Service Exception occurred,contact support.",
                    innerException: serviceException);

            var expectedAttachmentServiceException =
                new AttachmentServiceException(
                    message: "Attachment Service error occurred, contact support.",
                    innerException: failedAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllAttachmentsAction = () =>
                this.attachmentService.RetrieveAllAttachments();

            AttachmentServiceException actualAttachmentDependencyException =
                   Assert.Throws<AttachmentServiceException>(
                     retrieveAllAttachmentsAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAttachmentServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
