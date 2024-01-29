// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.CustomerAttachments
{
    public partial class CustomerAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllCustomerAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var failedCustomerAttachmentStorageException =
                new FailedCustomerAttachmentStorageException(
                    message: "Failed CustomerAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedCustomerAttachmentDependencyException =
                new CustomerAttachmentDependencyException
                (message: "CustomerAttachment dependency error occurred, contact support.",
                innerException: failedCustomerAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCustomerAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllCustomerAttachmentAction = () =>
                this.customerAttachmentService.RetrieveAllCustomerAttachments();

            CustomerAttachmentDependencyException actualAttachmentDependencyException =
                   Assert.Throws<CustomerAttachmentDependencyException>(
                     retrieveAllCustomerAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedCustomerAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCustomerAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllCustomerAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedCustomerAttachmentServiceException =
                new FailedCustomerAttachmentServiceException(
                    message: "Failed CustomerAttachment service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedCustomerAttachmentServiceException =
                new CustomerAttachmentServiceException(
                    message: "CustomerAttachment service error occurred, contact support.",
                    innerException: failedCustomerAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCustomerAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllCustomerAttachmentAction = () =>
                this.customerAttachmentService.RetrieveAllCustomerAttachments();

            CustomerAttachmentServiceException actualAttachmentDependencyException =
                   Assert.Throws<CustomerAttachmentServiceException>(
                     retrieveAllCustomerAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedCustomerAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCustomerAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
