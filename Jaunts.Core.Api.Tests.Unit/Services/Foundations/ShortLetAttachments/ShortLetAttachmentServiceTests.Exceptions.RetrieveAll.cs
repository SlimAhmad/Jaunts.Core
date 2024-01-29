// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLetAttachments
{
    public partial class ShortLetAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllShortLetAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var failedShortLetAttachmentStorageException =
                new FailedShortLetAttachmentStorageException(
                    message: "Failed ShortLetAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedShortLetAttachmentDependencyException =
                new ShortLetAttachmentDependencyException
                (message: "ShortLetAttachment dependency error occurred, contact support.",
                innerException: failedShortLetAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllShortLetAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllShortLetAttachmentAction = () =>
                this.shortLetAttachmentService.RetrieveAllShortLetAttachments();

            ShortLetAttachmentDependencyException actualAttachmentDependencyException =
                   Assert.Throws<ShortLetAttachmentDependencyException>(
                     retrieveAllShortLetAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedShortLetAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllShortLetAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllShortLetAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedShortLetAttachmentServiceException =
                new FailedShortLetAttachmentServiceException(
                    message: "Failed ShortLetAttachment service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedShortLetAttachmentServiceException =
                new ShortLetAttachmentServiceException(
                    message: "ShortLetAttachment service error occurred, contact support.",
                    innerException: failedShortLetAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllShortLetAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllShortLetAttachmentAction = () =>
                this.shortLetAttachmentService.RetrieveAllShortLetAttachments();

            ShortLetAttachmentServiceException actualAttachmentDependencyException =
                   Assert.Throws<ShortLetAttachmentServiceException>(
                     retrieveAllShortLetAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedShortLetAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllShortLetAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
