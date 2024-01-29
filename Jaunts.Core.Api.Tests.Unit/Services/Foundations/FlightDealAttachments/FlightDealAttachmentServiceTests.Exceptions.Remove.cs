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
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
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
                new FlightDealAttachmentDependencyException
                (message: "FlightDealAttachment dependency error occurred, contact support.", 
                innerException: failedFlightDealAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<FlightDealAttachment> removeFlightDealAttachmentTask =
                this.flightDealAttachmentService.RemoveFlightDealAttachmentByIdAsync(
                    someFlightDealId,
                    someAttachmentId);

            FlightDealAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<FlightDealAttachmentDependencyException>(
                removeFlightDealAttachmentTask.AsTask);

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

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
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
            ValueTask<FlightDealAttachment> removeFlightDealAttachmentTask =
                this.flightDealAttachmentService.RemoveFlightDealAttachmentByIdAsync
                (someFlightDealId, someAttachmentId);

            FlightDealAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<FlightDealAttachmentDependencyException>(
                removeFlightDealAttachmentTask.AsTask);

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

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
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
                    innerException: lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<FlightDealAttachment> removeFlightDealAttachmentTask =
                this.flightDealAttachmentService.RemoveFlightDealAttachmentByIdAsync(someFlightDealId, someAttachmentId);

            FlightDealAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<FlightDealAttachmentDependencyException>(
                removeFlightDealAttachmentTask.AsTask);

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

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            var randomAttachmentId = Guid.NewGuid();
            var randomFlightDealId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someFlightDealId = randomFlightDealId;
            var serviceException = new Exception();

            var failedFlightDealAttachmentServiceException =
                new FailedFlightDealAttachmentServiceException(
                    message: "Failed FlightDealAttachment service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedFlightDealAttachmentException =
                new FlightDealAttachmentServiceException(
                    message: "FlightDealAttachment service error occurred, contact support.",
                    innerException: failedFlightDealAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<FlightDealAttachment> removeFlightDealAttachmentTask =
                this.flightDealAttachmentService.RemoveFlightDealAttachmentByIdAsync(
                    someFlightDealId,
                    someAttachmentId);

            FlightDealAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<FlightDealAttachmentServiceException>(
                removeFlightDealAttachmentTask.AsTask);

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

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFlightDealAttachmentAsync(It.IsAny<FlightDealAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}