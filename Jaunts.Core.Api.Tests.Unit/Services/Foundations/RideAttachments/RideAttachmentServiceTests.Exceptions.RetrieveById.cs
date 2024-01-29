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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
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
                new RideAttachmentDependencyException(
                    message: "RideAttachment dependency error occurred, contact support.",
                    innerException: failedRideAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<RideAttachment> retrieveRideAttachmentTask =
                this.rideAttachmentService.RetrieveRideAttachmentByIdAsync(someRideId, someAttachmentId);

            RideAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<RideAttachmentDependencyException>(
                retrieveRideAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
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
            ValueTask<RideAttachment> retrieveRideAttachmentTask =
                this.rideAttachmentService.RetrieveRideAttachmentByIdAsync(someRideId, someAttachmentId);

            RideAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<RideAttachmentDependencyException>(
                retrieveRideAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
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
                    lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<RideAttachment> retrieveRideAttachmentTask =
                this.rideAttachmentService.RetrieveRideAttachmentByIdAsync(someRideId, someAttachmentId);

            RideAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<RideAttachmentDependencyException>(
                retrieveRideAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someRideId = Guid.NewGuid();
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
                broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<RideAttachment> retrieveRideAttachmentTask =
                this.rideAttachmentService.RetrieveRideAttachmentByIdAsync(someRideId, someAttachmentId);

            RideAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<RideAttachmentServiceException>(
                retrieveRideAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedRideAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
