// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Providers
{
    public partial class ProviderServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenProviderIsNullAndLogItAsync()
        {
            //given
            Provider invalidProvider = null;
            var nullProviderException = new NullProviderException();

            var expectedProviderValidationException =
                new ProviderValidationException(
                    message: "Provider validation error occurred, please try again.",
                    nullProviderException);

            //when
            ValueTask<Provider> modifyProviderTask =
                this.providerService.ModifyProviderAsync(invalidProvider);

            ProviderValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<ProviderValidationException>(
                     modifyProviderTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderAsync(It.IsAny<Provider>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfProviderIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidProvider = new Provider
            {
                RcNumber = invalidText,
                CompanyName = invalidText,
                Address = invalidText
            };

            var invalidProviderException = new InvalidProviderException();

            invalidProviderException.AddData(
                key: nameof(Provider.Id),
                values: "Id is required");

            invalidProviderException.AddData(
                key: nameof(Provider.RcNumber),
                values: "Text is required");

            invalidProviderException.AddData(
                key: nameof(Provider.CompanyName),
                values: "Text is required");

            invalidProviderException.AddData(
                key: nameof(Provider.Address),
                values: "Text is required");
 
            invalidProviderException.AddData(
                key: nameof(Provider.CreatedDate),
                values: "Date is required");

            invalidProviderException.AddData(
                key: nameof(Provider.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(Provider.CreatedDate)}");

            invalidProviderException.AddData(
                key: nameof(Provider.CreatedBy),
                values: "Id is required");

            invalidProviderException.AddData(
                key: nameof(Provider.UpdatedBy),
                values: "Id is required");

            var expectedProviderValidationException =
                new ProviderValidationException(invalidProviderException);

            // when
            ValueTask<Provider> createProviderTask =
                this.providerService.ModifyProviderAsync(invalidProvider);

            // then
            await Assert.ThrowsAsync<ProviderValidationException>(() =>
                createProviderTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAsync(It.IsAny<Provider>()),
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
            Provider randomProvider = CreateRandomProvider(dateTime);
            Provider inputProvider = randomProvider;

            var invalidProviderException = new InvalidProviderException(
                message: "Invalid provider. Please fix the errors and try again.");

            invalidProviderException.AddData(
               key: nameof(Provider.UpdatedDate),
               values: $"Date is the same as {nameof(inputProvider.CreatedDate)}");

            var expectedProviderValidationException =
                new ProviderValidationException(
                    message: "Provider validation error occurred, please try again.",
                    innerException: invalidProviderException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Provider> modifyProviderTask =
                this.providerService.ModifyProviderAsync(inputProvider);

            ProviderValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderValidationException>(
                modifyProviderTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderAsync(It.IsAny<Provider>()),
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
            Provider randomProvider = CreateRandomModifyProvider(dateTime);
            Provider inputProvider = randomProvider;
            inputProvider.UpdatedBy = inputProvider.CreatedBy;
            inputProvider.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidProviderException = new InvalidProviderException(
                message: "Invalid provider. Please fix the errors and try again.");

            invalidProviderException.AddData(
                   key: nameof(Provider.UpdatedDate),
                   values: "Date is not recent");

            var expectedProviderValidationException =
                new ProviderValidationException(
                    message: "Provider validation error occurred, please try again.",
                    innerException: invalidProviderException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Provider> modifyProviderTask =
                this.providerService.ModifyProviderAsync(inputProvider);

            ProviderValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderValidationException>(
                modifyProviderTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderAsync(It.IsAny<Provider>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfProviderDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Provider randomProvider = CreateRandomProvider(dateTime);
            Provider nonExistentProvider = randomProvider;
            nonExistentProvider.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Provider noProvider = null;
            var notFoundProviderException = new NotFoundProviderException(nonExistentProvider.Id);

            var expectedProviderValidationException =
                new ProviderValidationException(
                    message: "Provider validation error occurred, please try again.",
                    innerException: notFoundProviderException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderByIdAsync(nonExistentProvider.Id))
                    .ReturnsAsync(noProvider);

            // when
            ValueTask<Provider> modifyProviderTask =
                this.providerService.ModifyProviderAsync(nonExistentProvider);

            ProviderValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderValidationException>(
                modifyProviderTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(nonExistentProvider.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderAsync(It.IsAny<Provider>()),
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
            Provider randomProvider = CreateRandomModifyProvider(randomDateTimeOffset);
            Provider invalidProvider = randomProvider.DeepClone();
            Provider storageProvider = invalidProvider.DeepClone();
            storageProvider.CreatedDate = storageProvider.CreatedDate.AddMinutes(randomMinutes);
            storageProvider.UpdatedDate = storageProvider.UpdatedDate.AddMinutes(randomMinutes);
            Guid ProviderId = invalidProvider.Id;
          

            var invalidProviderException = new InvalidProviderException(
               message: "Invalid provider. Please fix the errors and try again.");

            invalidProviderException.AddData(
                 key: nameof(Provider.CreatedDate),
                 values: $"Date is not the same as {nameof(Provider.CreatedDate)}");

            var expectedProviderValidationException =
              new ProviderValidationException(
                  message: "Provider validation error occurred, please try again.",
                  innerException: invalidProviderException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderByIdAsync(ProviderId))
                    .ReturnsAsync(storageProvider);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Provider> modifyProviderTask =
                this.providerService.ModifyProviderAsync(invalidProvider);

            ProviderValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderValidationException>(
                modifyProviderTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(invalidProvider.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderAsync(It.IsAny<Provider>()),
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
            Provider randomProvider = CreateRandomModifyProvider(randomDateTimeOffset);
            Provider invalidProvider = randomProvider.DeepClone();
            Provider storageProvider = invalidProvider.DeepClone();
            storageProvider.UpdatedDate = storageProvider.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid ProviderId = invalidProvider.Id;
            invalidProvider.CreatedBy = invalidCreatedBy;

            var invalidProviderException = new InvalidProviderException(
                message: "Invalid provider. Please fix the errors and try again.");

            invalidProviderException.AddData(
                key: nameof(Provider.CreatedBy),
                values: $"Id is not the same as {nameof(Provider.CreatedBy)}");

            var expectedProviderValidationException =
              new ProviderValidationException(
                  message: "Provider validation error occurred, please try again.",
                  innerException: invalidProviderException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderByIdAsync(ProviderId))
                    .ReturnsAsync(storageProvider);

            // when
            ValueTask<Provider> modifyProviderTask =
                this.providerService.ModifyProviderAsync(invalidProvider);

            ProviderValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderValidationException>(
                modifyProviderTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(invalidProvider.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderAsync(It.IsAny<Provider>()),
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
            Provider randomProvider = CreateRandomModifyProvider(randomDate);
            Provider invalidProvider = randomProvider;
            invalidProvider.UpdatedDate = randomDate;
            Provider storageProvider = randomProvider.DeepClone();
            Guid ProviderId = invalidProvider.Id;

            var invalidProviderException = new InvalidProviderException(
               message: "Invalid provider. Please fix the errors and try again.");

            invalidProviderException.AddData(
               key: nameof(Provider.UpdatedDate),
               values: $"Date is the same as {nameof(invalidProvider.UpdatedDate)}");

            var expectedProviderValidationException =
              new ProviderValidationException(
                  message: "Provider validation error occurred, please try again.",
                  innerException: invalidProviderException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderByIdAsync(ProviderId))
                    .ReturnsAsync(storageProvider);

            // when
            ValueTask<Provider> modifyProviderTask =
                this.providerService.ModifyProviderAsync(invalidProvider);

            ProviderValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProviderValidationException>(
                modifyProviderTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(invalidProvider.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderAsync(It.IsAny<Provider>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
