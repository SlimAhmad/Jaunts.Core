// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLets
{
    public partial class ShortLetServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenShortLetIsNullAndLogItAsync()
        {
            //given
            ShortLet invalidShortLet = null;
            var nullShortLetException = new NullShortLetException();

            var expectedShortLetValidationException =
                new ShortLetValidationException(
                    message: "ShortLet validation error occurred, please try again.",
                    nullShortLetException);

            //when
            ValueTask<ShortLet> modifyShortLetTask =
                this.shortLetService.ModifyShortLetAsync(invalidShortLet);

            ShortLetValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<ShortLetValidationException>(
                     modifyShortLetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShortLetAsync(It.IsAny<ShortLet>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfShortLetIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidShortLet = new ShortLet
            {
                Description = invalidText,
                Name = invalidText,
                Location = invalidText,
            };

            var invalidShortLetException = new InvalidShortLetException();

            invalidShortLetException.AddData(
                key: nameof(ShortLet.Id),
                values: "Id is required");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.Description),
                values: "Text is required");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.Name),
                values: "Text is required");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.Location),
                values: "Text is required");
 
            invalidShortLetException.AddData(
                key: nameof(ShortLet.CreatedDate),
                values: "Date is required");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(ShortLet.CreatedDate)}");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.CreatedBy),
                values: "Id is required");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.UpdatedBy),
                values: "Id is required");

            var expectedShortLetValidationException =
                new ShortLetValidationException(invalidShortLetException);

            // when
            ValueTask<ShortLet> createShortLetTask =
                this.shortLetService.ModifyShortLetAsync(invalidShortLet);

            // then
            await Assert.ThrowsAsync<ShortLetValidationException>(() =>
                createShortLetTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAsync(It.IsAny<ShortLet>()),
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
            ShortLet randomShortLet = CreateRandomShortLet(dateTime);
            ShortLet inputShortLet = randomShortLet;

            var invalidShortLetException = new InvalidShortLetException(
                message: "Invalid shortLet. Please fix the errors and try again.");

            invalidShortLetException.AddData(
               key: nameof(ShortLet.UpdatedDate),
               values: $"Date is the same as {nameof(inputShortLet.CreatedDate)}");

            var expectedShortLetValidationException =
                new ShortLetValidationException(
                    message: "ShortLet validation error occurred, please try again.",
                    innerException: invalidShortLetException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<ShortLet> modifyShortLetTask =
                this.shortLetService.ModifyShortLetAsync(inputShortLet);

            ShortLetValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ShortLetValidationException>(
                modifyShortLetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShortLetAsync(It.IsAny<ShortLet>()),
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
            ShortLet randomShortLet = CreateRandomModifyShortLet(dateTime);
            ShortLet inputShortLet = randomShortLet;
            inputShortLet.UpdatedBy = inputShortLet.CreatedBy;
            inputShortLet.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidShortLetException = new InvalidShortLetException(
                message: "Invalid shortLet. Please fix the errors and try again.");

            invalidShortLetException.AddData(
                   key: nameof(ShortLet.UpdatedDate),
                   values: "Date is not recent");

            var expectedShortLetValidationException =
                new ShortLetValidationException(
                    message: "ShortLet validation error occurred, please try again.",
                    innerException: invalidShortLetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ShortLet> modifyShortLetTask =
                this.shortLetService.ModifyShortLetAsync(inputShortLet);

            ShortLetValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ShortLetValidationException>(
                modifyShortLetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShortLetAsync(It.IsAny<ShortLet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfShortLetDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            ShortLet randomShortLet = CreateRandomShortLet(dateTime);
            ShortLet nonExistentShortLet = randomShortLet;
            nonExistentShortLet.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            ShortLet noShortLet = null;
            var notFoundShortLetException = new NotFoundShortLetException(nonExistentShortLet.Id);

            var expectedShortLetValidationException =
                new ShortLetValidationException(
                    message: "ShortLet validation error occurred, please try again.",
                    innerException: notFoundShortLetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetByIdAsync(nonExistentShortLet.Id))
                    .ReturnsAsync(noShortLet);

            // when
            ValueTask<ShortLet> modifyShortLetTask =
                this.shortLetService.ModifyShortLetAsync(nonExistentShortLet);

            ShortLetValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ShortLetValidationException>(
                modifyShortLetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(nonExistentShortLet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShortLetAsync(It.IsAny<ShortLet>()),
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
            ShortLet randomShortLet = CreateRandomModifyShortLet(randomDateTimeOffset);
            ShortLet invalidShortLet = randomShortLet.DeepClone();
            ShortLet storageShortLet = invalidShortLet.DeepClone();
            storageShortLet.CreatedDate = storageShortLet.CreatedDate.AddMinutes(randomMinutes);
            storageShortLet.UpdatedDate = storageShortLet.UpdatedDate.AddMinutes(randomMinutes);
            Guid ShortLetId = invalidShortLet.Id;
          

            var invalidShortLetException = new InvalidShortLetException(
               message: "Invalid shortLet. Please fix the errors and try again.");

            invalidShortLetException.AddData(
                 key: nameof(ShortLet.CreatedDate),
                 values: $"Date is not the same as {nameof(ShortLet.CreatedDate)}");

            var expectedShortLetValidationException =
              new ShortLetValidationException(
                  message: "ShortLet validation error occurred, please try again.",
                  innerException: invalidShortLetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetByIdAsync(ShortLetId))
                    .ReturnsAsync(storageShortLet);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShortLet> modifyShortLetTask =
                this.shortLetService.ModifyShortLetAsync(invalidShortLet);

            ShortLetValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ShortLetValidationException>(
                modifyShortLetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(invalidShortLet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShortLetAsync(It.IsAny<ShortLet>()),
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
            ShortLet randomShortLet = CreateRandomModifyShortLet(randomDateTimeOffset);
            ShortLet invalidShortLet = randomShortLet.DeepClone();
            ShortLet storageShortLet = invalidShortLet.DeepClone();
            storageShortLet.UpdatedDate = storageShortLet.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid ShortLetId = invalidShortLet.Id;
            invalidShortLet.CreatedBy = invalidCreatedBy;

            var invalidShortLetException = new InvalidShortLetException(
                message: "Invalid shortLet. Please fix the errors and try again.");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.CreatedBy),
                values: $"Id is not the same as {nameof(ShortLet.CreatedBy)}");

            var expectedShortLetValidationException =
              new ShortLetValidationException(
                  message: "ShortLet validation error occurred, please try again.",
                  innerException: invalidShortLetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetByIdAsync(ShortLetId))
                    .ReturnsAsync(storageShortLet);

            // when
            ValueTask<ShortLet> modifyShortLetTask =
                this.shortLetService.ModifyShortLetAsync(invalidShortLet);

            ShortLetValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ShortLetValidationException>(
                modifyShortLetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(invalidShortLet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShortLetAsync(It.IsAny<ShortLet>()),
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
            ShortLet randomShortLet = CreateRandomModifyShortLet(randomDate);
            ShortLet invalidShortLet = randomShortLet;
            invalidShortLet.UpdatedDate = randomDate;
            ShortLet storageShortLet = randomShortLet.DeepClone();
            Guid ShortLetId = invalidShortLet.Id;

            var invalidShortLetException = new InvalidShortLetException(
               message: "Invalid shortLet. Please fix the errors and try again.");

            invalidShortLetException.AddData(
               key: nameof(ShortLet.UpdatedDate),
               values: $"Date is the same as {nameof(invalidShortLet.UpdatedDate)}");

            var expectedShortLetValidationException =
              new ShortLetValidationException(
                  message: "ShortLet validation error occurred, please try again.",
                  innerException: invalidShortLetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetByIdAsync(ShortLetId))
                    .ReturnsAsync(storageShortLet);

            // when
            ValueTask<ShortLet> modifyShortLetTask =
                this.shortLetService.ModifyShortLetAsync(invalidShortLet);

            ShortLetValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ShortLetValidationException>(
                modifyShortLetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(invalidShortLet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShortLetAsync(It.IsAny<ShortLet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
