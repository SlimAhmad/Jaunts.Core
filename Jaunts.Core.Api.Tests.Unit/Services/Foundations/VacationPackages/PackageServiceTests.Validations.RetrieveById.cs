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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
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

            //when
            ValueTask<Package> retrievePackageByIdTask =
                this.packageService.RetrievePackageByIdAsync(inputPackageId);

            PackageValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<PackageValidationException>(
                 retrievePackageByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStoragePackageIsNullAndLogItAsync()
        {
            //given
            Guid randomPackageId = Guid.NewGuid();
            Guid somePackageId = randomPackageId;
            Package invalidStoragePackage = null;
            var notFoundPackageException = new NotFoundPackageException(
                message: $"Couldn't find Package with id: {somePackageId}.");

            var expectedPackageValidationException =
                new PackageValidationException(
                    message: "Package validation error occurred, please try again.",
                    innerException: notFoundPackageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStoragePackage);

            //when
            ValueTask<Package> retrievePackageByIdTask =
                this.packageService.RetrievePackageByIdAsync(somePackageId);

            PackageValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<PackageValidationException>(
                 retrievePackageByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}