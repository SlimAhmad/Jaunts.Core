// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Packages;
using Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Packages
{
    public partial class PackageServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid somePackageId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedPackageStorageException =
              new FailedPackageStorageException(
                  message: "Failed Package storage error occurred, please contact support.",
                  sqlException);

            var expectedPackageDependencyException =
                new PackageDependencyException(
                    message: "Package dependency error occurred, contact support.",
                    expectedFailedPackageStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageByIdAsync(somePackageId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Package> deletePackageTask =
                this.packageService.RemovePackageByIdAsync(somePackageId);

            PackageDependencyException actualDependencyException =
                await Assert.ThrowsAsync<PackageDependencyException>(
                    deletePackageTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPackageDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(somePackageId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPackageDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid somePackageId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedPackageStorageException =
              new FailedPackageStorageException(
                  message: "Failed Package storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedPackageDependencyException =
                new PackageDependencyException(
                    message: "Package dependency error occurred, contact support.",
                    expectedFailedPackageStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageByIdAsync(somePackageId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Package> deletePackageTask =
                this.packageService.RemovePackageByIdAsync(somePackageId);

            PackageDependencyException actualDependencyException =
                await Assert.ThrowsAsync<PackageDependencyException>(
                    deletePackageTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPackageDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(somePackageId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid somePackageId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedPackageException = new LockedPackageException(
                message: "Locked Package record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedPackageDependencyException =
                new PackageDependencyException(
                    message: "Package dependency error occurred, contact support.",
                    innerException: lockedPackageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageByIdAsync(somePackageId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Package> deletePackageTask =
                this.packageService.RemovePackageByIdAsync(somePackageId);

            PackageDependencyException actualDependencyException =
                await Assert.ThrowsAsync<PackageDependencyException>(
                    deletePackageTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPackageDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(somePackageId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid somePackageId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedPackageServiceException =
             new FailedPackageServiceException(
                 message: "Failed Package service error occurred, contact support.",
                 innerException: serviceException);

            var expectedPackageServiceException =
                new PackageServiceException(
                    message: "Package service error occurred, contact support.",
                    innerException: failedPackageServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageByIdAsync(somePackageId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Package> deletePackageTask =
                this.packageService.RemovePackageByIdAsync(somePackageId);

            PackageServiceException actualServiceException =
             await Assert.ThrowsAsync<PackageServiceException>(
                 deletePackageTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedPackageServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(somePackageId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
