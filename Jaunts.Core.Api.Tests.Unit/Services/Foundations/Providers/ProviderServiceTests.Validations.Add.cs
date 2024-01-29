// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Providers
{
    public partial class ProviderServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenProviderIsNullAndLogItAsync()
        {
            // given
            Provider randomProvider = null;
            Provider nullProvider = randomProvider;

            var nullProviderException = new NullProviderException(
                message: "The Provider is null.");

            var expectedProviderValidationException =
                new ProviderValidationException(
                    message: "Provider validation error occurred, please try again.",
                    innerException: nullProviderException);

            // when
            ValueTask<Provider> createProviderTask =
                this.providerService.CreateProviderAsync(nullProvider);

             ProviderValidationException actualProviderDependencyValidationException =
             await Assert.ThrowsAsync<ProviderValidationException>(
                 createProviderTask.AsTask);

            // then
            actualProviderDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateIfProviderStatusIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Provider randomProvider = CreateRandomProvider(randomDateTime);
            Provider invalidProvider = randomProvider;
            invalidProvider.UpdatedBy = randomProvider.CreatedBy;
            invalidProvider.Status = GetInvalidEnum<ProviderStatus>();

            var invalidProviderException = new InvalidProviderException();

            invalidProviderException.AddData(
                key: nameof(Provider.Status),
                values: "Value is not recognized");

            var expectedProviderValidationException = new ProviderValidationException(
                message: "Provider validation error occurred, please try again.",
                innerException: invalidProviderException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<Provider> createProviderTask =
                this.providerService.CreateProviderAsync(invalidProvider);

            ProviderValidationException actualProviderDependencyValidationException =
            await Assert.ThrowsAsync<ProviderValidationException>(
                createProviderTask.AsTask);

            // then
            actualProviderDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAsync(It.IsAny<Provider>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenProviderIsInvalidAndLogItAsync(
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
                key: nameof(Provider.UserId),
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
                key: nameof(Provider.CreatedBy),
                values: "Id is required");

            invalidProviderException.AddData(
                key: nameof(Provider.UpdatedBy),
                values: "Id is required");

            invalidProviderException.AddData(
                key: nameof(Provider.Incorporation),
                values: "Date is required");

            invalidProviderException.AddData(
                key: nameof(Provider.CreatedDate),
                values: "Date is required");

            invalidProviderException.AddData(
                key: nameof(Provider.UpdatedDate),
                values: "Date is required");

            var expectedProviderValidationException =
                new ProviderValidationException(
                    message: "Provider validation error occurred, please try again.",
                    innerException: invalidProviderException);

            // when
            ValueTask<Provider> createProviderTask =
                this.providerService.CreateProviderAsync(invalidProvider);

             ProviderValidationException actualProviderDependencyValidationException =
             await Assert.ThrowsAsync<ProviderValidationException>(
                 createProviderTask.AsTask);

            // then
            actualProviderDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
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
            Provider randomProvider = CreateRandomProvider(dateTime);
            Provider inputProvider = randomProvider;
            inputProvider.UpdatedBy = Guid.NewGuid();

            var invalidProviderException = new InvalidProviderException();

            invalidProviderException.AddData(
                key: nameof(Provider.UpdatedBy),
                values: $"Id is not the same as {nameof(Provider.CreatedBy)}");

            var expectedProviderValidationException =
                new ProviderValidationException(
                    message: "Provider validation error occurred, please try again.",
                    innerException: invalidProviderException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Provider> createProviderTask =
                this.providerService.CreateProviderAsync(inputProvider);

             ProviderValidationException actualProviderDependencyValidationException =
             await Assert.ThrowsAsync<ProviderValidationException>(
                 createProviderTask.AsTask);

            // then
            actualProviderDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
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
            Provider randomProvider = CreateRandomProvider(dateTime);
            Provider inputProvider = randomProvider;
            inputProvider.UpdatedBy = randomProvider.CreatedBy;
            inputProvider.UpdatedDate = GetRandomDateTime();

            var invalidProviderException = new InvalidProviderException();

            invalidProviderException.AddData(
                key: nameof(Provider.UpdatedDate),
                values: $"Date is not the same as {nameof(Provider.CreatedDate)}");

            var expectedProviderValidationException =
                new ProviderValidationException(
                    message: "Provider validation error occurred, please try again.",
                    innerException: invalidProviderException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Provider> createProviderTask =
                this.providerService.CreateProviderAsync(inputProvider);

             ProviderValidationException actualProviderDependencyValidationException =
             await Assert.ThrowsAsync<ProviderValidationException>(
                 createProviderTask.AsTask);

            // then
            actualProviderDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
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
            Provider randomProvider = CreateRandomProvider(dateTime);
            Provider inputProvider = randomProvider;
            inputProvider.UpdatedBy = inputProvider.CreatedBy;
            inputProvider.CreatedDate = dateTime.AddMinutes(minutes);
            inputProvider.UpdatedDate = inputProvider.CreatedDate;

            var invalidProviderException = new InvalidProviderException();

            invalidProviderException.AddData(
                key: nameof(Provider.CreatedDate),
                values: $"Date is not recent");

            var expectedProviderValidationException =
                new ProviderValidationException(
                    message: "Provider validation error occurred, please try again.",
                    innerException: invalidProviderException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Provider> createProviderTask =
                this.providerService.CreateProviderAsync(inputProvider);

             ProviderValidationException actualProviderDependencyValidationException =
             await Assert.ThrowsAsync<ProviderValidationException>(
                 createProviderTask.AsTask);

            // then
            actualProviderDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenProviderAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Provider randomProvider = CreateRandomProvider(dateTime);
            Provider alreadyExistsProvider = randomProvider;
            alreadyExistsProvider.UpdatedBy = alreadyExistsProvider.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsProviderException =
                new AlreadyExistsProviderException(
                   message: "Provider with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedProviderValidationException =
                new ProviderDependencyValidationException(
                    message: "Provider dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsProviderException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProviderAsync(alreadyExistsProvider))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Provider> createProviderTask =
                this.providerService.CreateProviderAsync(alreadyExistsProvider);

             ProviderDependencyValidationException actualProviderDependencyValidationException =
             await Assert.ThrowsAsync<ProviderDependencyValidationException>(
                 createProviderTask.AsTask);

            // then
            actualProviderDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAsync(alreadyExistsProvider),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
