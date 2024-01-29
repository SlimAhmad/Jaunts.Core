// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Packages
{
    public partial class PackageServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllPackage())
                    .Throws(sqlException);

            // when
            Action retrieveAllPackageAction = () =>
                this.packageService.RetrieveAllPackage();

            PackageDependencyException actualDependencyException =
              Assert.Throws<PackageDependencyException>(
                 retrieveAllPackageAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPackageDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPackage(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllPackage())
                    .Throws(serviceException);

            // when
            Action retrieveAllPackageAction = () =>
                this.packageService.RetrieveAllPackage();

            PackageServiceException actualServiceException =
              Assert.Throws<PackageServiceException>(
                 retrieveAllPackageAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedPackageServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPackage(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
