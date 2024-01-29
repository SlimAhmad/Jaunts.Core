// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someFlightDealId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedFlightDealAttachmentStorageException =
             new FailedFlightDealAttachmentStorageException(
                 message: "Failed FlightDealAttachment storage error occurred, please contact support.",
                 innerException: sqlException);

            var expectedFlightDealAttachmentDependencyException =
                new FlightDealAttachmentDependencyException(
                    message: "FlightDealAttachment dependency error occurred, contact support.",
                    innerException: failedFlightDealAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<FlightDealAttachment> retrieveFlightDealAttachmentTask =
                this.flightDealAttachmentService.RetrieveFlightDealAttachmentByIdAsync(someFlightDealId, someAttachmentId);

            FlightDealAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<FlightDealAttachmentDependencyException>(
                retrieveFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someFlightDealId = Guid.NewGuid();
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
                broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<FlightDealAttachment> retrieveFlightDealAttachmentTask =
                this.flightDealAttachmentService.RetrieveFlightDealAttachmentByIdAsync(someFlightDealId, someAttachmentId);

            FlightDealAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<FlightDealAttachmentDependencyException>(
                retrieveFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someFlightDealId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedFlightDealAttachmentException(
                    message: "Locked FlightDealAttachment record exception, please try again later.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedFlightDealAttachmentException =
                new FlightDealAttachmentDependencyException(
                    message: "FlightDealAttachment dependency error occurred, contact support.",
                    lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<FlightDealAttachment> retrieveFlightDealAttachmentTask =
                this.flightDealAttachmentService.RetrieveFlightDealAttachmentByIdAsync(someFlightDealId, someAttachmentId);

            FlightDealAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<FlightDealAttachmentDependencyException>(
                retrieveFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someFlightDealId = Guid.NewGuid();
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
                broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<FlightDealAttachment> retrieveFlightDealAttachmentTask =
                this.flightDealAttachmentService.RetrieveFlightDealAttachmentByIdAsync(someFlightDealId, someAttachmentId);

            FlightDealAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<FlightDealAttachmentServiceException>(
                retrieveFlightDealAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedFlightDealAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
