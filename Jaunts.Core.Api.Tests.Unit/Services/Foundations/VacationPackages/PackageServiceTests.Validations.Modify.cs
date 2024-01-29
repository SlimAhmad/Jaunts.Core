// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Packages;
using Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Packages
{
    public partial class PackageServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenPackageIsNullAndLogItAsync()
        {
            //given
            Package invalidPackage = null;
            var nullPackageException = new NullPackageException();

            var expectedPackageValidationException =
                new PackageValidationException(
                    message: "Package validation error occurred, please try again.",
                    nullPackageException);

            //when
            ValueTask<Package> modifyPackageTask =
                this.packageService.ModifyPackageAsync(invalidPackage);

            PackageValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<PackageValidationException>(
                     modifyPackageTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfPackageIsInvalidAndLogItAsync(
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
               key: nameof(Package.StartDate),
               values: "Date is required");

            invalidPackageException.AddData(
                key: nameof(Package.EndDate),
                values: "Date is required");

            invalidPackageException.AddData(
                key: nameof(Package.CreatedDate),
                values: "Date is required");

            invalidPackageException.AddData(
                key: nameof(Package.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(Package.CreatedDate)}");

            invalidPackageException.AddData(
                key: nameof(Package.CreatedBy),
                values: "Id is required");

            invalidPackageException.AddData(
                key: nameof(Package.UpdatedBy),
                values: "Id is required");

            var expectedPackageValidationException =
                new PackageValidationException(invalidPackageException);

            // when
            ValueTask<Package> createPackageTask =
                this.packageService.ModifyPackageAsync(invalidPackage);

            // then
            await Assert.ThrowsAsync<PackageValidationException>(() =>
                createPackageTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            Package randomPackage = CreateRandomPackage(dateTime);
            Package inputPackage = randomPackage;

            var invalidPackageException = new InvalidPackageException(
                message: "Invalid Package. Please fix the errors and try again.");

            invalidPackageException.AddData(
               key: nameof(Package.UpdatedDate),
               values: $"Date is the same as {nameof(inputPackage.CreatedDate)}");

            var expectedPackageValidationException =
                new PackageValidationException(
                    message: "Package validation error occurred, please try again.",
                    innerException: invalidPackageException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Package> modifyPackageTask =
                this.packageService.ModifyPackageAsync(inputPackage);

            PackageValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<PackageValidationException>(
                modifyPackageTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Package randomPackage = CreateRandomModifyPackage(dateTime);
            Package inputPackage = randomPackage;
            inputPackage.UpdatedBy = inputPackage.CreatedBy;
            inputPackage.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidPackageException = new InvalidPackageException(
                message: "Invalid Package. Please fix the errors and try again.");

            invalidPackageException.AddData(
                   key: nameof(Package.UpdatedDate),
                   values: "Date is not recent");

            var expectedPackageValidationException =
                new PackageValidationException(
                    message: "Package validation error occurred, please try again.",
                    innerException: invalidPackageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Package> modifyPackageTask =
                this.packageService.ModifyPackageAsync(inputPackage);

            PackageValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<PackageValidationException>(
                modifyPackageTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfPackageDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Package randomPackage = CreateRandomPackage(dateTime);
            Package nonExistentPackage = randomPackage;
            nonExistentPackage.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Package noPackage = null;
            var notFoundPackageException = new NotFoundPackageException(nonExistentPackage.Id);

            var expectedPackageValidationException =
                new PackageValidationException(
                    message: "Package validation error occurred, please try again.",
                    innerException: notFoundPackageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageByIdAsync(nonExistentPackage.Id))
                    .ReturnsAsync(noPackage);

            // when
            ValueTask<Package> modifyPackageTask =
                this.packageService.ModifyPackageAsync(nonExistentPackage);

            PackageValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<PackageValidationException>(
                modifyPackageTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(nonExistentPackage.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetNegativeRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            Package randomPackage = CreateRandomModifyPackage(randomDateTimeOffset);
            Package invalidPackage = randomPackage.DeepClone();
            Package storagePackage = invalidPackage.DeepClone();
            storagePackage.CreatedDate = storagePackage.CreatedDate.AddMinutes(randomMinutes);
            storagePackage.UpdatedDate = storagePackage.UpdatedDate.AddMinutes(randomMinutes);
            Guid PackageId = invalidPackage.Id;
          

            var invalidPackageException = new InvalidPackageException(
               message: "Invalid Package. Please fix the errors and try again.");

            invalidPackageException.AddData(
                 key: nameof(Package.CreatedDate),
                 values: $"Date is not the same as {nameof(Package.CreatedDate)}");

            var expectedPackageValidationException =
              new PackageValidationException(
                  message: "Package validation error occurred, please try again.",
                  innerException: invalidPackageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageByIdAsync(PackageId))
                    .ReturnsAsync(storagePackage);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Package> modifyPackageTask =
                this.packageService.ModifyPackageAsync(invalidPackage);

            PackageValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<PackageValidationException>(
                modifyPackageTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(invalidPackage.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
           public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int randomPositiveMinutes = GetRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            Package randomPackage = CreateRandomModifyPackage(randomDateTimeOffset);
            Package invalidPackage = randomPackage.DeepClone();
            Package storagePackage = invalidPackage.DeepClone();
            storagePackage.UpdatedDate = storagePackage.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid PackageId = invalidPackage.Id;
            invalidPackage.CreatedBy = invalidCreatedBy;

            var invalidPackageException = new InvalidPackageException(
                message: "Invalid Package. Please fix the errors and try again.");

            invalidPackageException.AddData(
                key: nameof(Package.CreatedBy),
                values: $"Id is not the same as {nameof(Package.CreatedBy)}");

            var expectedPackageValidationException =
              new PackageValidationException(
                  message: "Package validation error occurred, please try again.",
                  innerException: invalidPackageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageByIdAsync(PackageId))
                    .ReturnsAsync(storagePackage);

            // when
            ValueTask<Package> modifyPackageTask =
                this.packageService.ModifyPackageAsync(invalidPackage);

            PackageValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<PackageValidationException>(
                modifyPackageTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(invalidPackage.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetCurrentDateTime();
            Package randomPackage = CreateRandomModifyPackage(randomDate);
            Package invalidPackage = randomPackage;
            invalidPackage.UpdatedDate = randomDate;
            Package storagePackage = randomPackage.DeepClone();
            Guid PackageId = invalidPackage.Id;

            var invalidPackageException = new InvalidPackageException(
               message: "Invalid Package. Please fix the errors and try again.");

            invalidPackageException.AddData(
               key: nameof(Package.UpdatedDate),
               values: $"Date is the same as {nameof(invalidPackage.UpdatedDate)}");

            var expectedPackageValidationException =
              new PackageValidationException(
                  message: "Package validation error occurred, please try again.",
                  innerException: invalidPackageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageByIdAsync(PackageId))
                    .ReturnsAsync(storagePackage);

            // when
            ValueTask<Package> modifyPackageTask =
                this.packageService.ModifyPackageAsync(invalidPackage);

            PackageValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<PackageValidationException>(
                modifyPackageTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(invalidPackage.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePackageAsync(It.IsAny<Package>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
