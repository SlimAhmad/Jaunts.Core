// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.AdvertAttachments
{
    public partial class AdvertAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllAdvertAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var failedAdvertAttachmentStorageException =
                new FailedAdvertAttachmentStorageException(
                    message: "Failed AdvertAttachment storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedAdvertAttachmentDependencyException =
                new AdvertAttachmentDependencyException
                (message: "AdvertAttachment dependency error occurred, contact support.",
                innerException: failedAdvertAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAdvertAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllAdvertAttachmentAction = () =>
                this.AdvertAttachmentService.RetrieveAllAdvertAttachments();

            AdvertAttachmentDependencyException actualAttachmentDependencyException =
                   Assert.Throws<AdvertAttachmentDependencyException>(
                     retrieveAllAdvertAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAdvertAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAdvertAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllAdvertAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedAdvertAttachmentServiceException =
                new FailedAdvertAttachmentServiceException(
                    message: "Failed AdvertAttachment service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAdvertAttachmentServiceException =
                new AdvertAttachmentServiceException(
                    message: "AdvertAttachment service error occurred, contact support.",
                    innerException: failedAdvertAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAdvertAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllAdvertAttachmentAction = () =>
                this.AdvertAttachmentService.RetrieveAllAdvertAttachments();

            AdvertAttachmentServiceException actualAttachmentDependencyException =
                   Assert.Throws<AdvertAttachmentServiceException>(
                     retrieveAllAdvertAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAdvertAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAdvertAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
