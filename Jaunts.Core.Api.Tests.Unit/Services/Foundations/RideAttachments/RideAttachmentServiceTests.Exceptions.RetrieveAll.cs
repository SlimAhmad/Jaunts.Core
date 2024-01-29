// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.RideAttachments
{
    public partial class RideAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllRideAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var failedRideAttachmentStorageException =
                new FailedRideAttachmentStorageException(
                    message: "Failed RideAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedRideAttachmentDependencyException =
                new RideAttachmentDependencyException
                (message: "RideAttachment dependency error occurred, contact support.",
                innerException: failedRideAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRideAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllRideAttachmentAction = () =>
                this.rideAttachmentService.RetrieveAllRideAttachments();

            RideAttachmentDependencyException actualAttachmentDependencyException =
                   Assert.Throws<RideAttachmentDependencyException>(
                     retrieveAllRideAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedRideAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRideAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRideAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllRideAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedRideAttachmentServiceException =
                new FailedRideAttachmentServiceException(
                    message: "Failed RideAttachment service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedRideAttachmentServiceException =
                new RideAttachmentServiceException(
                    message: "RideAttachment service error occurred, contact support.",
                    innerException: failedRideAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRideAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllRideAttachmentAction = () =>
                this.rideAttachmentService.RetrieveAllRideAttachments();

            RideAttachmentServiceException actualAttachmentDependencyException =
                   Assert.Throws<RideAttachmentServiceException>(
                     retrieveAllRideAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedRideAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRideAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
