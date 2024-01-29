// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDealAttachments
{
    public partial class FlightDealAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllFlightDealAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var failedFlightDealAttachmentStorageException =
                new FailedFlightDealAttachmentStorageException(
                    message: "Failed FlightDealAttachment storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedFlightDealAttachmentDependencyException =
                new FlightDealAttachmentDependencyException
                (message: "FlightDealAttachment dependency error occurred, contact support.",
                innerException: failedFlightDealAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFlightDealAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllFlightDealAttachmentAction = () =>
                this.flightDealAttachmentService.RetrieveAllFlightDealAttachments();

            FlightDealAttachmentDependencyException actualAttachmentDependencyException =
                   Assert.Throws<FlightDealAttachmentDependencyException>(
                     retrieveAllFlightDealAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFlightDealAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllFlightDealAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedFlightDealAttachmentServiceException =
                new FailedFlightDealAttachmentServiceException(
                    message: "Failed FlightDealAttachment service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedFlightDealAttachmentServiceException =
                new FlightDealAttachmentServiceException(
                    message: "FlightDealAttachment service error occurred, contact support.",
                    innerException: failedFlightDealAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFlightDealAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllFlightDealAttachmentAction = () =>
                this.flightDealAttachmentService.RetrieveAllFlightDealAttachments();

            FlightDealAttachmentServiceException actualAttachmentDependencyException =
                   Assert.Throws<FlightDealAttachmentServiceException>(
                     retrieveAllFlightDealAttachmentAction);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFlightDealAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
