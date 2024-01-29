// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Packages;
using Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Packages
{
    public partial class PackageServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenPackageIsNullAndLogItAsync()
        {
            // given
            Package randomPackage = null;
            Package nullPackage = randomPackage;

            var nullPackageException = new NullPackageException(
                message: "The Package is null.");

            var expectedPackageValidationException =
                new PackageValidationException(
                    message: "Package validation error occurred, please try again.",
                    innerException: nullPackageException);

            // when
            ValueTask<Package> createPackageTask =
                this.packageService.CreatePackageAsync(nullPackage);

             PackageValidationException actualPackageDependencyValidationException =
             await Assert.ThrowsAsync<PackageValidationException>(
                 createPackageTask.AsTask);

            // then
            actualPackageDependencyValidationException.Should().BeEquivalentTo(
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
        public async void ShouldThrowValidationExceptionOnCreateIfPackageStatusIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Package randomPackage = CreateRandomPackage(randomDateTime);
            Package invalidPackage = randomPackage;
            invalidPackage.UpdatedBy = randomPackage.CreatedBy;
            invalidPackage.Status = GetInvalidEnum<PackageStatus>();

            var invalidPackageException = new InvalidPackageException();

            invalidPackageException.AddData(
                key: nameof(Package.Status),
                values: "Value is not recognized");

            var expectedPackageValidationException = new PackageValidationException(
                message: "Package validation error occurred, please try again.",
                innerException: invalidPackageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<Package> createPackageTask =
                this.packageService.CreatePackageAsync(invalidPackage);

            PackageValidationException actualPackageDependencyValidationException =
            await Assert.ThrowsAsync<PackageValidationException>(
                createPackageTask.AsTask);

            // then
            actualPackageDependencyValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenPackageIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidPackage = new Package
            {
                Description = invalidText,
                Destination = invalidText,
                Name = invalidText

            };

            var invalidPackageException = new InvalidPackageException();

            invalidPackageException.AddData(
                key: nameof(Package.Id),
                values: "Id is required");

            invalidPackageException.AddData(
                key: nameof(Package.Destination),
                values: "Text is required");

            invalidPackageException.AddData(
                key: nameof(Package.Description),
                values: "Text is required");

            invalidPackageException.AddData(
                key: nameof(Package.Name),
                values: "Text is required");

            invalidPackageException.AddData(
                key: nameof(Package.CreatedBy),
                values: "Id is required");

            invalidPackageException.AddData(
                key: nameof(Package.UpdatedBy),
                values: "Id is required");

            invalidPackageException.AddData(
                key: nameof(Package.CreatedDate),
                values: "Date is required");

            invalidPackageException.AddData(
                key: nameof(Package.UpdatedDate),
                values: "Date is required");

            invalidPackageException.AddData(
               key: nameof(Package.StartDate),
               values: "Date is required");

            invalidPackageException.AddData(
               key: nameof(Package.EndDate),
               values: "Date is required");

            var expectedPackageValidationException =
                new PackageValidationException(
                    message: "Package validation error occurred, please try again.",
                    innerException: invalidPackageException);

            // when
            ValueTask<Package> createPackageTask =
                this.packageService.CreatePackageAsync(invalidPackage);

             PackageValidationException actualPackageDependencyValidationException =
             await Assert.ThrowsAsync<PackageValidationException>(
                 createPackageTask.AsTask);

            // then
            actualPackageDependencyValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            Package randomPackage = CreateRandomPackage(dateTime);
            Package inputPackage = randomPackage;
            inputPackage.UpdatedBy = Guid.NewGuid();

            var invalidPackageException = new InvalidPackageException();

            invalidPackageException.AddData(
                key: nameof(Package.UpdatedBy),
                values: $"Id is not the same as {nameof(Package.CreatedBy)}");

            var expectedPackageValidationException =
                new PackageValidationException(
                    message: "Package validation error occurred, please try again.",
                    innerException: invalidPackageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Package> createPackageTask =
                this.packageService.CreatePackageAsync(inputPackage);

             PackageValidationException actualPackageDependencyValidationException =
             await Assert.ThrowsAsync<PackageValidationException>(
                 createPackageTask.AsTask);

            // then
            actualPackageDependencyValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Package randomPackage = CreateRandomPackage(dateTime);
            Package inputPackage = randomPackage;
            inputPackage.UpdatedBy = randomPackage.CreatedBy;
            inputPackage.UpdatedDate = GetRandomDateTime();

            var invalidPackageException = new InvalidPackageException();

            invalidPackageException.AddData(
                key: nameof(Package.UpdatedDate),
                values: $"Date is not the same as {nameof(Package.CreatedDate)}");

            var expectedPackageValidationException =
                new PackageValidationException(
                    message: "Package validation error occurred, please try again.",
                    innerException: invalidPackageException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Package> createPackageTask =
                this.packageService.CreatePackageAsync(inputPackage);

             PackageValidationException actualPackageDependencyValidationException =
             await Assert.ThrowsAsync<PackageValidationException>(
                 createPackageTask.AsTask);

            // then
            actualPackageDependencyValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Package randomPackage = CreateRandomPackage(dateTime);
            Package inputPackage = randomPackage;
            inputPackage.UpdatedBy = inputPackage.CreatedBy;
            inputPackage.CreatedDate = dateTime.AddMinutes(minutes);
            inputPackage.UpdatedDate = inputPackage.CreatedDate;

            var invalidPackageException = new InvalidPackageException();

            invalidPackageException.AddData(
                key: nameof(Package.CreatedDate),
                values: $"Date is not recent");

            var expectedPackageValidationException =
                new PackageValidationException(
                    message: "Package validation error occurred, please try again.",
                    innerException: invalidPackageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Package> createPackageTask =
                this.packageService.CreatePackageAsync(inputPackage);

             PackageValidationException actualPackageDependencyValidationException =
             await Assert.ThrowsAsync<PackageValidationException>(
                 createPackageTask.AsTask);

            // then
            actualPackageDependencyValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenPackageAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Package randomPackage = CreateRandomPackage(dateTime);
            Package alreadyExistsPackage = randomPackage;
            alreadyExistsPackage.UpdatedBy = alreadyExistsPackage.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsPackageException =
                new AlreadyExistsPackageException(
                   message: "Package with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedPackageValidationException =
                new PackageDependencyValidationException(
                    message: "Package dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsPackageException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPackageAsync(alreadyExistsPackage))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Package> createPackageTask =
                this.packageService.CreatePackageAsync(alreadyExistsPackage);

             PackageDependencyValidationException actualPackageDependencyValidationException =
             await Assert.ThrowsAsync<PackageDependencyValidationException>(
                 createPackageTask.AsTask);

            // then
            actualPackageDependencyValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAsync(alreadyExistsPackage),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
