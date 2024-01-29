// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Hosting;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Providers
{
    public partial class ProviderServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
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

            //when
            ValueTask<Provider> retrieveProviderByIdTask =
                this.providerService.RetrieveProviderByIdAsync(inputProviderId);

            ProviderValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProviderValidationException>(
                 retrieveProviderByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageProviderIsNullAndLogItAsync()
        {
            //given
            Guid randomProviderId = Guid.NewGuid();
            Guid someProviderId = randomProviderId;
            Provider invalidStorageProvider = null;
            var notFoundProviderException = new NotFoundProviderException(
                message: $"Couldn't find Provider with id: {someProviderId}.");

            var expectedProviderValidationException =
                new ProviderValidationException(
                    message: "Provider validation error occurred, please try again.",
                    innerException: notFoundProviderException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageProvider);

            //when
            ValueTask<Provider> retrieveProviderByIdTask =
                this.providerService.RetrieveProviderByIdAsync(someProviderId);

            ProviderValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProviderValidationException>(
                 retrieveProviderByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}