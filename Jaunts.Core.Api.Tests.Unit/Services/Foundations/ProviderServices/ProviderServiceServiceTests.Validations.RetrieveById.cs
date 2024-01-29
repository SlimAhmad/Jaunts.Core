// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomProviderServiceId = default;
            Guid inputProviderServiceId = randomProviderServiceId;

            var invalidProviderServiceException = new InvalidProviderServiceException(
                message: "Invalid ProviderService. Please fix the errors and try again.");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.Id),
                values: "Id is required");

            var expectedProviderServiceValidationException =
                new ProviderServiceValidationException(
                    message: "ProviderService validation error occurred, Please try again.", 
                    innerException: invalidProviderServiceException);

            //when
            ValueTask<ProviderService> retrieveProviderServiceByIdTask =
                this.providerServiceService.RetrieveProviderServiceByIdAsync(inputProviderServiceId);

            ProviderServiceValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProviderServiceValidationException>(
                 retrieveProviderServiceByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageProviderServiceIsNullAndLogItAsync()
        {
            //given
            Guid randomProviderServiceId = Guid.NewGuid();
            Guid someProviderServiceId = randomProviderServiceId;
            ProviderService invalidStorageProviderService = null;
            var notFoundProviderServiceException = new NotFoundProviderServiceException(
                message: $"Couldn't find ProviderService with id: {someProviderServiceId}.");

            var expectedProviderServiceValidationException =
                new ProviderServiceValidationException(
                    message: "ProviderService validation error occurred, Please try again.",
                    innerException: notFoundProviderServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageProviderService);

            //when
            ValueTask<ProviderService> retrieveProviderServiceByIdTask =
                this.providerServiceService.RetrieveProviderServiceByIdAsync(someProviderServiceId);

            ProviderServiceValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProviderServiceValidationException>(
                 retrieveProviderServiceByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}