// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PackageAttachments
{
    public partial class PackageAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid somePackageId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedPackageAttachmentStorageException =
                new FailedPackageAttachmentStorageException(
                    message: "Failed PackageAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedPackageAttachmentDependencyException =
                new PackageAttachmentDependencyException
                (message: "PackageAttachment dependency error occurred, contact support.", 
                innerException: failedPackageAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<PackageAttachment> removePackageAttachmentTask =
                this.packageAttachmentService.RemovePackageAttachmentByIdAsync(
                    somePackageId,
                    someAttachmentId);

            PackageAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<PackageAttachmentDependencyException>(
                removePackageAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPackageAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPackageAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePackageAttachmentAsync(It.IsAny<PackageAttachment>()),
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
            Guid somePackageId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedPackageAttachmentStorageException =
                new FailedPackageAttachmentStorageException(
                    message: "Failed PackageAttachment storage error occurred, Please contact support.",
                    innerException: databaseUpdateException);

            var expectedPackageAttachmentDependencyException =
                new PackageAttachmentDependencyException(
                    message: "PackageAttachment dependency error occurred, contact support.",
                    innerException: failedPackageAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<PackageAttachment> removePackageAttachmentTask =
                this.packageAttachmentService.RemovePackageAttachmentByIdAsync
                (somePackageId, someAttachmentId);

            PackageAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<PackageAttachmentDependencyException>(
                removePackageAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPackageAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePackageAttachmentAsync(It.IsAny<PackageAttachment>()),
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
            Guid somePackageId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedPackageAttachmentException(
                    message: "Locked PackageAttachment record exception, Please try again later.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedPackageAttachmentException =
                new PackageAttachmentDependencyException(
                    message: "PackageAttachment dependency error occurred, contact support.",
                    innerException: lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<PackageAttachment> removePackageAttachmentTask =
                this.packageAttachmentService.RemovePackageAttachmentByIdAsync(somePackageId, someAttachmentId);

            PackageAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<PackageAttachmentDependencyException>(
                removePackageAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPackageAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePackageAttachmentAsync(It.IsAny<PackageAttachment>()),
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
            var randomPackageId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid somePackageId = randomPackageId;
            var serviceException = new Exception();

            var failedPackageAttachmentServiceException =
                new FailedPackageAttachmentServiceException(
                    message: "Failed PackageAttachment service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedPackageAttachmentException =
                new PackageAttachmentServiceException(
                    message: "PackageAttachment service error occurred, contact support.",
                    innerException: failedPackageAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<PackageAttachment> removePackageAttachmentTask =
                this.packageAttachmentService.RemovePackageAttachmentByIdAsync(
                    somePackageId,
                    someAttachmentId);

            PackageAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<PackageAttachmentServiceException>(
                removePackageAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPackageAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePackageAttachmentAsync(It.IsAny<PackageAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}