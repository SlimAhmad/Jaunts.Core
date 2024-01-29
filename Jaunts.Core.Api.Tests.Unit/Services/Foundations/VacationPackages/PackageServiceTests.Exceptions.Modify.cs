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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Package somePackage = CreateRandomPackage(randomDateTime);
            somePackage.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

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
            ValueTask<Package> modifyPackageTask =
                this.packageService.ModifyPackageAsync(somePackage);

                PackageDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<PackageDependencyException>(
                     modifyPackageTask.AsTask);

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
                broker.SelectPackageByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Package somePackage = CreateRandomPackage(randomDateTime);
            somePackage.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Package> modifyPackageTask =
                this.packageService.ModifyPackageAsync(somePackage);

            PackageDependencyException actualDependencyException =
                await Assert.ThrowsAsync<PackageDependencyException>(
                    modifyPackageTask.AsTask);

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
                broker.SelectPackageByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Package randomPackage = CreateRandomPackage(randomDateTime);
            Package somePackage = randomPackage;
            somePackage.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedPackageException = new LockedPackageException(
                message: "Locked Package record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedPackageDependencyException =
                new PackageDependencyException(
                    message: "Package dependency error occurred, contact support.",
                    innerException: lockedPackageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Package> modifyPackageTask =
                this.packageService.ModifyPackageAsync(somePackage);

            PackageDependencyException actualDependencyException =
             await Assert.ThrowsAsync<PackageDependencyException>(
                 modifyPackageTask.AsTask);

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
                broker.SelectPackageByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Package randomPackage = CreateRandomPackage(randomDateTime);
            Package somePackage = randomPackage;
            somePackage.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Package> modifyPackageTask =
                this.packageService.ModifyPackageAsync(somePackage);

            PackageServiceException actualServiceException =
             await Assert.ThrowsAsync<PackageServiceException>(
                 modifyPackageTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedPackageServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
