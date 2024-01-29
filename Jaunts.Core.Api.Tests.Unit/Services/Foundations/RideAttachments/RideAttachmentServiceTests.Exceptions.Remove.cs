// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.RideAttachments
{
    public partial class RideAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someRideId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedRideAttachmentStorageException =
                new FailedRideAttachmentStorageException(
                    message: "Failed RideAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedRideAttachmentDependencyException =
                new RideAttachmentDependencyException
                (message: "RideAttachment dependency error occurred, contact support.", 
                innerException: failedRideAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<RideAttachment> removeRideAttachmentTask =
                this.rideAttachmentService.RemoveRideAttachmentByIdAsync(
                    someRideId,
                    someAttachmentId);

            RideAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<RideAttachmentDependencyException>(
                removeRideAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedRideAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRideAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRideAttachmentAsync(It.IsAny<RideAttachment>()),
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
            Guid someRideId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedRideAttachmentStorageException =
                new FailedRideAttachmentStorageException(
                    message: "Failed RideAttachment storage error occurred, Please contact support.",
                    innerException: databaseUpdateException);

            var expectedRideAttachmentDependencyException =
                new RideAttachmentDependencyException(
                    message: "RideAttachment dependency error occurred, contact support.",
                    innerException: failedRideAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<RideAttachment> removeRideAttachmentTask =
                this.rideAttachmentService.RemoveRideAttachmentByIdAsync
                (someRideId, someAttachmentId);

            RideAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<RideAttachmentDependencyException>(
                removeRideAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedRideAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRideAttachmentAsync(It.IsAny<RideAttachment>()),
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
            Guid someRideId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedRideAttachmentException(
                    message: "Locked RideAttachment record exception, Please try again later.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedRideAttachmentException =
                new RideAttachmentDependencyException(
                    message: "RideAttachment dependency error occurred, contact support.",
                    innerException: lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<RideAttachment> removeRideAttachmentTask =
                this.rideAttachmentService.RemoveRideAttachmentByIdAsync(someRideId, someAttachmentId);

            RideAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<RideAttachmentDependencyException>(
                removeRideAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedRideAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRideAttachmentAsync(It.IsAny<RideAttachment>()),
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
            var randomRideId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someRideId = randomRideId;
            var serviceException = new Exception();

            var failedRideAttachmentServiceException =
                new FailedRideAttachmentServiceException(
                    message: "Failed RideAttachment service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedRideAttachmentException =
                new RideAttachmentServiceException(
                    message: "RideAttachment service error occurred, contact support.",
                    innerException: failedRideAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<RideAttachment> removeRideAttachmentTask =
                this.rideAttachmentService.RemoveRideAttachmentByIdAsync(
                    someRideId,
                    someAttachmentId);

            RideAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<RideAttachmentServiceException>(
                removeRideAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedRideAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRideAttachmentAsync(It.IsAny<RideAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}