// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Packages;
using Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Package somePackage = CreateRandomPackage(dateTime);
            somePackage.UpdatedBy = somePackage.CreatedBy;
            var sqlException = GetSqlException();

            var expectedFailedPackageStorageException =
                new FailedPackageStorageException(
                    message: "Failed Package storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedPackageDependencyException =
                new PackageDependencyException(
                    message: "Package dependency error occurred, contact support.",
                    innerException: expectedFailedPackageStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Package> createPackageTask =
                this.packageService.CreatePackageAsync(somePackage);

            PackageDependencyException actualDependencyException =
             await Assert.ThrowsAsync<PackageDependencyException>(
                 createPackageTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPackageDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPackageDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Package somePackage = CreateRandomPackage(dateTime);
            somePackage.UpdatedBy = somePackage.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedPackageStorageException =
                new FailedPackageStorageException(
                    message: "Failed Package storage error occurred, please contact support.",
                    databaseUpdateException);

            var expectedPackageDependencyException =
                new PackageDependencyException(
                    message: "Package dependency error occurred, contact support.",
                    expectedFailedPackageStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Package> createPackageTask =
                this.packageService.CreatePackageAsync(somePackage);

            PackageDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<PackageDependencyException>(
                     createPackageTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPackageDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Package somePackage = CreateRandomPackage(dateTime);
            somePackage.UpdatedBy = somePackage.CreatedBy;
            var serviceException = new Exception();

            var failedPackageServiceException =
                new FailedPackageServiceException(
                    message: "Failed Package service error occurred, contact support.",
                    innerException: serviceException);

            var expectedPackageServiceException =
                new PackageServiceException(
                    message: "Package service error occurred, contact support.",
                    innerException: failedPackageServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<Package> createPackageTask =
                 this.packageService.CreatePackageAsync(somePackage);

            PackageServiceException actualDependencyException =
                 await Assert.ThrowsAsync<PackageServiceException>(
                     createPackageTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPackageServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
