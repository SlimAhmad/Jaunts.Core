// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PackageAttachments
{
    public partial class PackageAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllPackageAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var failedPackageAttachmentStorageException =
                new FailedPackageAttachmentStorageException(
                    message: "Failed PackageAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedPackageAttachmentDependencyException =
                new PackageAttachmentDependencyException
                (message: "PackageAttachment dependency error occurred, contact support.",
                innerException: failedPackageAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPackageAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllPackageAttachmentAction = () =>
                this.packageAttachmentService.RetrieveAllPackageAttachments();

            PackageAttachmentDependencyException actualAttachmentDependencyException =
                   Assert.Throws<PackageAttachmentDependencyException>(
                     retrieveAllPackageAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPackageAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPackageAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPackageAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllPackageAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedPackageAttachmentServiceException =
                new FailedPackageAttachmentServiceException(
                    message: "Failed PackageAttachment service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedPackageAttachmentServiceException =
                new PackageAttachmentServiceException(
                    message: "PackageAttachment service error occurred, contact support.",
                    innerException: failedPackageAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPackageAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllPackageAttachmentAction = () =>
                this.packageAttachmentService.RetrieveAllPackageAttachments();

            PackageAttachmentServiceException actualAttachmentDependencyException =
                   Assert.Throws<PackageAttachmentServiceException>(
                     retrieveAllPackageAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPackageAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPackageAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
