// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Packages;
using Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Packages
{
    public partial class PackageServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomPackageId = default;
            Guid inputPackageId = randomPackageId;

            var invalidPackageException = new InvalidPackageException(
                message: "Invalid Package. Please fix the errors and try again.");

            invalidPackageException.AddData(
                key: nameof(Package.Id),
                values: "Id is required");

            var expectedPackageValidationException =
                new PackageValidationException(
                    message: "Package validation error occurred, please try again.",
                    innerException: invalidPackageException);

            // when
            ValueTask<Package> actualPackageTask =
                this.packageService.RemovePackageByIdAsync(inputPackageId);

            PackageValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<PackageValidationException>(
                 actualPackageTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStoragePackageIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Package randomPackage = CreateRandomPackage(dateTime);
            Guid inputPackageId = randomPackage.Id;
            Package inputPackage = randomPackage;
            Package nullStoragePackage = null;

            var notFoundPackageException = new NotFoundPackageException(inputPackageId);

            var expectedPackageValidationException =
                new PackageValidationException(
                    message: "Package validation error occurred, please try again.",
                    innerException: notFoundPackageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageByIdAsync(inputPackageId))
                    .ReturnsAsync(nullStoragePackage);

            // when
            ValueTask<Package> actualPackageTask =
                this.packageService.RemovePackageByIdAsync(inputPackageId);

            PackageValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<PackageValidationException>(
                 actualPackageTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(inputPackageId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
