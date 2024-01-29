// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderServices
{
    public partial class ProviderServiceServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenProviderServiceIsNullAndLogItAsync()
        {
            //given
            ProviderService invalidProviderService = null;
            var nullProviderServiceException = new NullProviderServiceException();

            var expectedProviderServiceValidationException =
                new ProviderServiceValidationException(
                    message: "ProviderService validation error occurred, Please try again.",
                    nullProviderServiceException);

            //when
            ValueTask<ProviderService> modifyProviderServiceTask =
                this.providerServiceService.ModifyProviderServiceAsync(invalidProviderService);

            ProviderServiceValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<ProviderServiceValidationException>(
                     modifyProviderServiceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderServiceAsync(It.IsAny<ProviderService>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfProviderServiceIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidProviderService = new ProviderService
            {
                Description = invalidText,
                ServiceName = invalidText,
                
            };

            var invalidProviderServiceException = new InvalidProviderServiceException();

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.Id),
                values: "Id is required");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.ServiceName),
                values: "Text is required");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.Description),
                values: "Text is required");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.CreatedDate),
                values: "Date is required");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(ProviderService.CreatedDate)}");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.CreatedBy),
                values: "Id is required");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.UpdatedBy),
                values: "Id is required");

            var expectedProviderServiceValidationException =
                new ProviderServiceValidationException(invalidProviderServiceException);

            // when
            ValueTask<ProviderService> createProviderServiceTask =
                this.providerServiceService.ModifyProviderServiceAsync(invalidProviderService);

            // then
            await Assert.ThrowsAsync<ProviderServiceValidationException>(() =>
                createProviderServiceTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderServiceAsync(It.IsAny<ProviderService>()),
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
            ProviderService randomProviderService = CreateRandomProviderService(dateTime);
            ProviderService inputProviderService = randomProviderService;

            var invalidProviderServiceException = new InvalidProviderServiceException(
                message: "Invalid ProviderService. Please fix the errors and try again.");

            invalidProviderServiceException.AddData(
               key: nameof(ProviderService.UpdatedDate),
               values: $"Date is the same as {nameof(inputProviderService.CreatedDate)}");

            var expectedProviderServiceValidationException =
                new ProviderServiceValidationException(
                    message: "ProviderService validation error occurred, Please try again.",
                    innerException: invalidProviderServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<ProviderService> modifyProviderServiceTask =
                this.providerServiceService.ModifyProviderServiceAsync(inputProviderService);

            ProviderServiceValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderServiceValidationException>(
                modifyProviderServiceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderServiceAsync(It.IsAny<ProviderService>()),
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
            ProviderService randomProviderService = CreateRandomModifyProviderService(dateTime);
            ProviderService inputProviderService = randomProviderService;
            inputProviderService.UpdatedBy = inputProviderService.CreatedBy;
            inputProviderService.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidProviderServiceException = new InvalidProviderServiceException(
                message: "Invalid ProviderService. Please fix the errors and try again.");

            invalidProviderServiceException.AddData(
                   key: nameof(ProviderService.UpdatedDate),
                   values: "Date is not recent");

            var expectedProviderServiceValidationException =
                new ProviderServiceValidationException(
                    message: "ProviderService validation error occurred, Please try again.",
                    innerException: invalidProviderServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ProviderService> modifyProviderServiceTask =
                this.providerServiceService.ModifyProviderServiceAsync(inputProviderService);

            ProviderServiceValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderServiceValidationException>(
                modifyProviderServiceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderServiceAsync(It.IsAny<ProviderService>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfProviderServiceDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderService randomProviderService = CreateRandomProviderService(dateTime);
            ProviderService nonExistentProviderService = randomProviderService;
            nonExistentProviderService.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            ProviderService noProviderService = null;
            var notFoundProviderServiceException = new NotFoundProviderServiceException(nonExistentProviderService.Id);

            var expectedProviderServiceValidationException =
                new ProviderServiceValidationException(
                    message: "ProviderService validation error occurred, Please try again.",
                    innerException: notFoundProviderServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderServiceByIdAsync(nonExistentProviderService.Id))
                    .ReturnsAsync(noProviderService);

            // when
            ValueTask<ProviderService> modifyProviderServiceTask =
                this.providerServiceService.ModifyProviderServiceAsync(nonExistentProviderService);

            ProviderServiceValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderServiceValidationException>(
                modifyProviderServiceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(nonExistentProviderService.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderServiceAsync(It.IsAny<ProviderService>()),
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
            ProviderService randomProviderService = CreateRandomModifyProviderService(randomDateTimeOffset);
            ProviderService invalidProviderService = randomProviderService.DeepClone();
            ProviderService storageProviderService = invalidProviderService.DeepClone();
            storageProviderService.CreatedDate = storageProviderService.CreatedDate.AddMinutes(randomMinutes);
            storageProviderService.UpdatedDate = storageProviderService.UpdatedDate.AddMinutes(randomMinutes);
            Guid ProviderServiceId = invalidProviderService.Id;
          

            var invalidProviderServiceException = new InvalidProviderServiceException(
               message: "Invalid ProviderService. Please fix the errors and try again.");

            invalidProviderServiceException.AddData(
                 key: nameof(ProviderService.CreatedDate),
                 values: $"Date is not the same as {nameof(ProviderService.CreatedDate)}");

            var expectedProviderServiceValidationException =
              new ProviderServiceValidationException(
                  message: "ProviderService validation error occurred, Please try again.",
                  innerException: invalidProviderServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderServiceByIdAsync(ProviderServiceId))
                    .ReturnsAsync(storageProviderService);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ProviderService> modifyProviderServiceTask =
                this.providerServiceService.ModifyProviderServiceAsync(invalidProviderService);

            ProviderServiceValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderServiceValidationException>(
                modifyProviderServiceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(invalidProviderService.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderServiceAsync(It.IsAny<ProviderService>()),
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
            ProviderService randomProviderService = CreateRandomModifyProviderService(randomDateTimeOffset);
            ProviderService invalidProviderService = randomProviderService.DeepClone();
            ProviderService storageProviderService = invalidProviderService.DeepClone();
            storageProviderService.UpdatedDate = storageProviderService.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid ProviderServiceId = invalidProviderService.Id;
            invalidProviderService.CreatedBy = invalidCreatedBy;

            var invalidProviderServiceException = new InvalidProviderServiceException(
                message: "Invalid ProviderService. Please fix the errors and try again.");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.CreatedBy),
                values: $"Id is not the same as {nameof(ProviderService.CreatedBy)}");

            var expectedProviderServiceValidationException =
              new ProviderServiceValidationException(
                  message: "ProviderService validation error occurred, Please try again.",
                  innerException: invalidProviderServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderServiceByIdAsync(ProviderServiceId))
                    .ReturnsAsync(storageProviderService);

            // when
            ValueTask<ProviderService> modifyProviderServiceTask =
                this.providerServiceService.ModifyProviderServiceAsync(invalidProviderService);

            ProviderServiceValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderServiceValidationException>(
                modifyProviderServiceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(invalidProviderService.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderServiceAsync(It.IsAny<ProviderService>()),
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
            ProviderService randomProviderService = CreateRandomModifyProviderService(randomDate);
            ProviderService invalidProviderService = randomProviderService;
            invalidProviderService.UpdatedDate = randomDate;
            ProviderService storageProviderService = randomProviderService.DeepClone();
            Guid ProviderServiceId = invalidProviderService.Id;

            var invalidProviderServiceException = new InvalidProviderServiceException(
               message: "Invalid ProviderService. Please fix the errors and try again.");

            invalidProviderServiceException.AddData(
               key: nameof(ProviderService.UpdatedDate),
               values: $"Date is the same as {nameof(invalidProviderService.UpdatedDate)}");

            var expectedProviderServiceValidationException =
              new ProviderServiceValidationException(
                  message: "ProviderService validation error occurred, Please try again.",
                  innerException: invalidProviderServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderServiceByIdAsync(ProviderServiceId))
                    .ReturnsAsync(storageProviderService);

            // when
            ValueTask<ProviderService> modifyProviderServiceTask =
                this.providerServiceService.ModifyProviderServiceAsync(invalidProviderService);

            ProviderServiceValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderServiceValidationException>(
                modifyProviderServiceTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(invalidProviderService.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderServiceAsync(It.IsAny<ProviderService>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
