// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
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
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomProviderId = default;
            Guid inputProviderId = randomProviderId;

            var invalidProviderException = new InvalidProviderException(
                message: "Invalid provider. Please fix the errors and try again.");

            invalidProviderException.AddData(
                key: nameof(Provider.Id),
                values: "Id is required");

            var expectedProviderValidationException =
                new ProviderValidationException(
                    message: "Provider validation error occurred, please try again.",
                    innerException: invalidProviderException);

            // when
            ValueTask<Provider> actualProviderTask =
                this.providerService.RemoveProviderByIdAsync(inputProviderId);

            ProviderValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProviderValidationException>(
                 actualProviderTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderAsync(It.IsAny<Provider>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageProviderIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Provider randomProvider = CreateRandomProvider(dateTime);
            Guid inputProviderId = randomProvider.Id;
            Provider inputProvider = randomProvider;
            Provider nullStorageProvider = null;

            var notFoundProviderException = new NotFoundProviderException(inputProviderId);

            var expectedProviderValidationException =
                new ProviderValidationException(
                    message: "Provider validation error occurred, please try again.",
                    innerException: notFoundProviderException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderByIdAsync(inputProviderId))
                    .ReturnsAsync(nullStorageProvider);

            // when
            ValueTask<Provider> actualProviderTask =
                this.providerService.RemoveProviderByIdAsync(inputProviderId);

            ProviderValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProviderValidationException>(
                 actualProviderTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(inputProviderId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderAsync(It.IsAny<Provider>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
