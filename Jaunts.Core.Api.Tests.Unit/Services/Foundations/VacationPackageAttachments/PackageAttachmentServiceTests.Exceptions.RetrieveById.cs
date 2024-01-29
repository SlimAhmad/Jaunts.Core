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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
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
                new PackageAttachmentDependencyException(
                    message: "PackageAttachment dependency error occurred, contact support.",
                    innerException: failedPackageAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<PackageAttachment> retrievePackageAttachmentTask =
                this.packageAttachmentService.RetrievePackageAttachmentByIdAsync(somePackageId, someAttachmentId);

            PackageAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<PackageAttachmentDependencyException>(
                retrievePackageAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
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
            ValueTask<PackageAttachment> retrievePackageAttachmentTask =
                this.packageAttachmentService.RetrievePackageAttachmentByIdAsync(somePackageId, someAttachmentId);

            PackageAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<PackageAttachmentDependencyException>(
                retrievePackageAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
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
                    lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<PackageAttachment> retrievePackageAttachmentTask =
                this.packageAttachmentService.RetrievePackageAttachmentByIdAsync(somePackageId, someAttachmentId);

            PackageAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<PackageAttachmentDependencyException>(
                retrievePackageAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid somePackageId = Guid.NewGuid();
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
                broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<PackageAttachment> retrievePackageAttachmentTask =
                this.packageAttachmentService.RetrievePackageAttachmentByIdAsync(somePackageId, someAttachmentId);

            PackageAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<PackageAttachmentServiceException>(
                retrievePackageAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPackageAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
