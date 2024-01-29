// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            PackageAttachment randomPackageAttachment = CreateRandomPackageAttachment();
            PackageAttachment inputPackageAttachment = randomPackageAttachment;
            var sqlException = GetSqlException();

            var failedPackageAttachmentStorageException =
                new FailedPackageAttachmentStorageException(
                    message: "Failed PackageAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedPackageAttachmentDependencyException =
                new PackageAttachmentDependencyException(
                    message: "PackageAttachment dependency error occurred, contact support.",
                    innerException: failedPackageAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPackageAttachmentAsync(It.IsAny<PackageAttachment>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<PackageAttachment> addPackageAttachmentTask =
                this.packageAttachmentService.AddPackageAttachmentAsync(inputPackageAttachment);

            PackageAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<PackageAttachmentDependencyException>(
                     addPackageAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPackageAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPackageAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAttachmentAsync(It.IsAny<PackageAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            PackageAttachment randomPackageAttachment = CreateRandomPackageAttachment();
            PackageAttachment inputPackageAttachment = randomPackageAttachment;
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
                broker.InsertPackageAttachmentAsync(It.IsAny<PackageAttachment>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<PackageAttachment> addPackageAttachmentTask =
                this.packageAttachmentService.AddPackageAttachmentAsync(inputPackageAttachment);

            PackageAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<PackageAttachmentDependencyException>(
                     addPackageAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPackageAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAttachmentAsync(It.IsAny<PackageAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            PackageAttachment randomPackageAttachment = CreateRandomPackageAttachment();
            PackageAttachment inputPackageAttachment = randomPackageAttachment;
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
                broker.InsertPackageAttachmentAsync(It.IsAny<PackageAttachment>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<PackageAttachment> addPackageAttachmentTask =
                 this.packageAttachmentService.AddPackageAttachmentAsync(inputPackageAttachment);

            PackageAttachmentServiceException actualAttachmentDependencyException =
             await Assert.ThrowsAsync<PackageAttachmentServiceException>(
                 addPackageAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPackageAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAttachmentAsync(It.IsAny<PackageAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
