// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectorAttachments
{
    public partial class ProvidersDirectorAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllProvidersDirectorAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var failedProvidersDirectorAttachmentStorageException =
                new FailedProvidersDirectorAttachmentStorageException(
                    message: "Failed ProvidersDirectorAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedProvidersDirectorAttachmentDependencyException =
                new ProvidersDirectorAttachmentDependencyException
                (message: "ProvidersDirectorAttachment dependency error occurred, contact support.",
                innerException: failedProvidersDirectorAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProvidersDirectorAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllProvidersDirectorAttachmentAction = () =>
                this.providersDirectorAttachmentService.RetrieveAllProvidersDirectorAttachments();

            ProvidersDirectorAttachmentDependencyException actualAttachmentDependencyException =
                   Assert.Throws<ProvidersDirectorAttachmentDependencyException>(
                     retrieveAllProvidersDirectorAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProvidersDirectorAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllProvidersDirectorAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedProvidersDirectorAttachmentServiceException =
                new FailedProvidersDirectorAttachmentServiceException(
                    message: "Failed ProvidersDirectorAttachment service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedProvidersDirectorAttachmentServiceException =
                new ProvidersDirectorAttachmentServiceException(
                    message: "ProvidersDirectorAttachment service error occurred, contact support.",
                    innerException: failedProvidersDirectorAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProvidersDirectorAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllProvidersDirectorAttachmentAction = () =>
                this.providersDirectorAttachmentService.RetrieveAllProvidersDirectorAttachments();

            ProvidersDirectorAttachmentServiceException actualAttachmentDependencyException =
                   Assert.Throws<ProvidersDirectorAttachmentServiceException>(
                     retrieveAllProvidersDirectorAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProvidersDirectorAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
