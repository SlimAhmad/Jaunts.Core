// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDealAttachments
{
    public partial class FlightDealAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            FlightDealAttachment randomFlightDealAttachment = CreateRandomFlightDealAttachment();
            FlightDealAttachment inputFlightDealAttachment = randomFlightDealAttachment;
            var sqlException = GetSqlException();

            var failedFlightDealAttachmentStorageException =
                new FailedFlightDealAttachmentStorageException(
                    message: "Failed FlightDealAttachment storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedFlightDealAttachmentDependencyException =
                new FlightDealAttachmentDependencyException(
                    message: "FlightDealAttachment dependency error occurred, contact support.",
                    innerException: failedFlightDealAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<FlightDealAttachment> addFlightDealAttachmentTask =
                this.flightDealAttachmentService.AddFlightDealAttachmentAsync(inputFlightDealAttachment);

            FlightDealAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<FlightDealAttachmentDependencyException>(
                     addFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            FlightDealAttachment randomFlightDealAttachment = CreateRandomFlightDealAttachment();
            FlightDealAttachment inputFlightDealAttachment = randomFlightDealAttachment;
            var databaseUpdateException = new DbUpdateException();

            var failedFlightDealAttachmentStorageException =
              new FailedFlightDealAttachmentStorageException(
                  message: "Failed FlightDealAttachment storage error occurred, please contact support.",
                  innerException: databaseUpdateException);

            var expectedFlightDealAttachmentDependencyException =
                new FlightDealAttachmentDependencyException(
                    message: "FlightDealAttachment dependency error occurred, contact support.",
                    innerException: failedFlightDealAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<FlightDealAttachment> addFlightDealAttachmentTask =
                this.flightDealAttachmentService.AddFlightDealAttachmentAsync(inputFlightDealAttachment);

            FlightDealAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<FlightDealAttachmentDependencyException>(
                     addFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            FlightDealAttachment randomFlightDealAttachment = CreateRandomFlightDealAttachment();
            FlightDealAttachment inputFlightDealAttachment = randomFlightDealAttachment;
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
                broker.InsertFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<FlightDealAttachment> addFlightDealAttachmentTask =
                 this.flightDealAttachmentService.AddFlightDealAttachmentAsync(inputFlightDealAttachment);

            FlightDealAttachmentServiceException actualAttachmentDependencyException =
             await Assert.ThrowsAsync<FlightDealAttachmentServiceException>(
                 addFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
