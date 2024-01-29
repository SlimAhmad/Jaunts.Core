// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOfferAttachments
{
    public partial class PromosOfferAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllPromosOfferAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var failedPromosOfferAttachmentStorageException =
                new FailedPromosOfferAttachmentStorageException(
                    message: "Failed PromosOfferAttachment storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedPromosOfferAttachmentDependencyException =
                new PromosOfferAttachmentDependencyException
                (message: "PromosOfferAttachment dependency error occurred, contact support.",
                innerException: failedPromosOfferAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPromosOfferAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllPromosOfferAttachmentAction = () =>
                this.promosOfferAttachmentService.RetrieveAllPromosOfferAttachments();

            PromosOfferAttachmentDependencyException actualAttachmentDependencyException =
                   Assert.Throws<PromosOfferAttachmentDependencyException>(
                     retrieveAllPromosOfferAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPromosOfferAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllPromosOfferAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedPromosOfferAttachmentServiceException =
                new FailedPromosOfferAttachmentServiceException(
                    message: "Failed PromosOfferAttachment service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedPromosOfferAttachmentServiceException =
                new PromosOfferAttachmentServiceException(
                    message: "PromosOfferAttachment service error occurred, contact support.",
                    innerException: failedPromosOfferAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPromosOfferAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllPromosOfferAttachmentAction = () =>
                this.promosOfferAttachmentService.RetrieveAllPromosOfferAttachments();

            PromosOfferAttachmentServiceException actualAttachmentDependencyException =
                   Assert.Throws<PromosOfferAttachmentServiceException>(
                     retrieveAllPromosOfferAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPromosOfferAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
