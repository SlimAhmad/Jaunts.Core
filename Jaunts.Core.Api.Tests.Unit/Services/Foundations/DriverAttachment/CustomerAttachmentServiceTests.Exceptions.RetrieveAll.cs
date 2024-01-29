// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.DriverAttachments
{
    public partial class DriverAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllDriverAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var failedDriverAttachmentStorageException =
                new FailedDriverAttachmentStorageException(
                    message: "Failed DriverAttachment storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDriverAttachmentDependencyException =
                new DriverAttachmentDependencyException
                (message: "DriverAttachment dependency error occurred, contact support.",
                innerException: failedDriverAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDriverAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllDriverAttachmentAction = () =>
                this.driverAttachmentService.RetrieveAllDriverAttachments();

            DriverAttachmentDependencyException actualAttachmentDependencyException =
                   Assert.Throws<DriverAttachmentDependencyException>(
                     retrieveAllDriverAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedDriverAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDriverAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDriverAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllDriverAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedDriverAttachmentServiceException =
                new FailedDriverAttachmentServiceException(
                    message: "Failed DriverAttachment service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDriverAttachmentServiceException =
                new DriverAttachmentServiceException(
                    message: "DriverAttachment service error occurred, contact support.",
                    innerException: failedDriverAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDriverAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllDriverAttachmentAction = () =>
                this.driverAttachmentService.RetrieveAllDriverAttachments();

            DriverAttachmentServiceException actualAttachmentDependencyException =
                   Assert.Throws<DriverAttachmentServiceException>(
                     retrieveAllDriverAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedDriverAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDriverAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
