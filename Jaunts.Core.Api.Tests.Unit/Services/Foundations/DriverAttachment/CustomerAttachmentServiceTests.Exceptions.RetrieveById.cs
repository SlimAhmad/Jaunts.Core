// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.DriverAttachments
{
    public partial class DriverAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someDriverId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedDriverAttachmentStorageException =
             new FailedDriverAttachmentStorageException(
                 message: "Failed DriverAttachment storage error occurred, please contact support.",
                 innerException: sqlException);

            var expectedDriverAttachmentDependencyException =
                new DriverAttachmentDependencyException(
                    message: "DriverAttachment dependency error occurred, contact support.",
                    innerException: failedDriverAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectDriverAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<DriverAttachment> retrieveDriverAttachmentTask =
                this.driverAttachmentService.RetrieveDriverAttachmentByIdAsync(someDriverId, someAttachmentId);

            DriverAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<DriverAttachmentDependencyException>(
                retrieveDriverAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedDriverAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDriverAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            Guid someDriverId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedDriverAttachmentStorageException =
             new FailedDriverAttachmentStorageException(
                 message: "Failed DriverAttachment storage error occurred, please contact support.",
                 innerException: databaseUpdateException);

            var expectedDriverAttachmentDependencyException =
                new DriverAttachmentDependencyException(
                    message: "DriverAttachment dependency error occurred, contact support.",
                    innerException: failedDriverAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<DriverAttachment> retrieveDriverAttachmentTask =
                this.driverAttachmentService.RetrieveDriverAttachmentByIdAsync(someDriverId, someAttachmentId);

            DriverAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<DriverAttachmentDependencyException>(
                retrieveDriverAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedDriverAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            Guid someDriverId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedDriverAttachmentException(
                    message: "Locked DriverAttachment record exception, please try again later.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedDriverAttachmentException =
                new DriverAttachmentDependencyException(
                    message: "DriverAttachment dependency error occurred, contact support.",
                    lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<DriverAttachment> retrieveDriverAttachmentTask =
                this.driverAttachmentService.RetrieveDriverAttachmentByIdAsync(someDriverId, someAttachmentId);

            DriverAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<DriverAttachmentDependencyException>(
                retrieveDriverAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedDriverAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            Guid someDriverId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedDriverAttachmentServiceException =
                 new FailedDriverAttachmentServiceException(
                     message: "Failed DriverAttachment service error occurred, please contact support.",
                     innerException: serviceException);

            var expectedDriverAttachmentServiceException =
                new DriverAttachmentServiceException(
                    message: "DriverAttachment service error occurred, contact support.",
                    innerException: failedDriverAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DriverAttachment> retrieveDriverAttachmentTask =
                this.driverAttachmentService.RetrieveDriverAttachmentByIdAsync(someDriverId, someAttachmentId);

            DriverAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<DriverAttachmentServiceException>(
                retrieveDriverAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedDriverAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
