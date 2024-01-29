// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderAttachments
{
    public partial class ProviderAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllProviderAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var failedProviderAttachmentStorageException =
                new FailedProviderAttachmentStorageException(
                    message: "Failed ProviderAttachment storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedProviderAttachmentDependencyException =
                new ProviderAttachmentDependencyException
                (message: "ProviderAttachment dependency error occurred, contact support.",
                innerException: failedProviderAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProviderAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllProviderAttachmentAction = () =>
                this.providerAttachmentService.RetrieveAllProviderAttachments();

            ProviderAttachmentDependencyException actualAttachmentDependencyException =
                   Assert.Throws<ProviderAttachmentDependencyException>(
                     retrieveAllProviderAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProviderAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProviderAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProviderAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllProviderAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedProviderAttachmentServiceException =
                new FailedProviderAttachmentServiceException(
                    message: "Failed ProviderAttachment service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedProviderAttachmentServiceException =
                new ProviderAttachmentServiceException(
                    message: "ProviderAttachment service error occurred, contact support.",
                    innerException: failedProviderAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProviderAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllProviderAttachmentAction = () =>
                this.providerAttachmentService.RetrieveAllProviderAttachments();

            ProviderAttachmentServiceException actualAttachmentDependencyException =
                   Assert.Throws<ProviderAttachmentServiceException>(
                     retrieveAllProviderAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProviderAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProviderAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
