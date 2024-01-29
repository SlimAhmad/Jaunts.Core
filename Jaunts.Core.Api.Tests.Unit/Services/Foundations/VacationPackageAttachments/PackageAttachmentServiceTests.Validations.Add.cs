// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PackageAttachments
{
    public partial class PackageAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenPackageAttachmentIsNullAndLogItAsync()
        {
            // given
            PackageAttachment invalidPackageAttachment = null;

            var nullPackageAttachmentException = new NullPackageAttachmentException(
                message: "The PackageAttachment is null.");

            var expectedPackageAttachmentValidationException =
                new PackageAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: nullPackageAttachmentException);

            // when
            ValueTask<PackageAttachment> addPackageAttachmentTask =
                this.packageAttachmentService.AddPackageAttachmentAsync(invalidPackageAttachment);

            PackageAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PackageAttachmentValidationException>(
                  addPackageAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAttachmentAsync(It.IsAny<PackageAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenPackageIdIsInvalidAndLogItAsync()
        {
            // given
            PackageAttachment randomPackageAttachment = CreateRandomPackageAttachment();
            PackageAttachment inputPackageAttachment = randomPackageAttachment;
            inputPackageAttachment.PackageId = default;

            var invalidPackageAttachmentException =
              new InvalidPackageAttachmentException(
                  message: "Invalid PackageAttachment. Please correct the errors and try again.");

            invalidPackageAttachmentException.AddData(
                key: nameof(PackageAttachment.PackageId),
                values: "Id is required");

            var expectedPackageAttachmentValidationException =
                new PackageAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidPackageAttachmentException);

            // when
            ValueTask<PackageAttachment> addPackageAttachmentTask =
                this.packageAttachmentService.AddPackageAttachmentAsync(inputPackageAttachment);

            PackageAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PackageAttachmentValidationException>(
                  addPackageAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAttachmentAsync(It.IsAny<PackageAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            PackageAttachment randomPackageAttachment = CreateRandomPackageAttachment();
            PackageAttachment inputPackageAttachment = randomPackageAttachment;
            inputPackageAttachment.AttachmentId = default;

            var invalidPackageAttachmentException =
                new InvalidPackageAttachmentException(
                    message: "Invalid PackageAttachment. Please correct the errors and try again.");

            invalidPackageAttachmentException.AddData(
                key: nameof(PackageAttachment.AttachmentId),
                values: "Id is required");

            var expectedPackageAttachmentValidationException =
                new PackageAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidPackageAttachmentException);

            // when
            ValueTask<PackageAttachment> addPackageAttachmentTask =
                this.packageAttachmentService.AddPackageAttachmentAsync(inputPackageAttachment);

            PackageAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PackageAttachmentValidationException>(
                  addPackageAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAttachmentAsync(It.IsAny<PackageAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenPackageAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            PackageAttachment randomPackageAttachment = CreateRandomPackageAttachment();
            PackageAttachment alreadyExistsPackageAttachment = randomPackageAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsPackageAttachmentException =
                new AlreadyExistsPackageAttachmentException(
                    message: "PackageAttachment  with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedPackageAttachmentValidationException =
                new PackageAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: alreadyExistsPackageAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPackageAttachmentAsync(alreadyExistsPackageAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<PackageAttachment> addPackageAttachmentTask =
                this.packageAttachmentService.AddPackageAttachmentAsync(alreadyExistsPackageAttachment);

            PackageAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PackageAttachmentValidationException>(
                  addPackageAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedPackageAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAttachmentAsync(alreadyExistsPackageAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferenceExceptionAndLogItAsync()
        {
            // given
            PackageAttachment randomPackageAttachment = CreateRandomPackageAttachment();
            PackageAttachment invalidPackageAttachment = randomPackageAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidPackageAttachmentReferenceException =
                new InvalidPackageAttachmentReferenceException(
                    message: "Invalid guardian attachment reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedPackageAttachmentValidationException =
                new PackageAttachmentValidationException(
                    message: "Invalid input, contact support.", 
                    innerException: invalidPackageAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPackageAttachmentAsync(invalidPackageAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<PackageAttachment> addPackageAttachmentTask =
                this.packageAttachmentService.AddPackageAttachmentAsync(invalidPackageAttachment);

            PackageAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PackageAttachmentValidationException>(
                  addPackageAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedPackageAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAttachmentAsync(invalidPackageAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
