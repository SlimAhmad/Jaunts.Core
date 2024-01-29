// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Hosting;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Adverts
{
    public partial class AdvertServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomAdvertId = default;
            Guid inputAdvertId = randomAdvertId;

            var invalidAdvertException = new InvalidAdvertException(
                message: "Invalid advert. Please fix the errors and try again.");

            invalidAdvertException.AddData(
                key: nameof(Advert.Id),
                values: "Id is required");

            var expectedAdvertValidationException =
                new AdvertValidationException(
                    message: "Advert validation error occurred, please try again.", 
                    innerException: invalidAdvertException);

            //when
            ValueTask<Advert> retrieveAdvertByIdTask =
                this.advertService.RetrieveAdvertByIdAsync(inputAdvertId);

            AdvertValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<AdvertValidationException>(
                 retrieveAdvertByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageAdvertIsNullAndLogItAsync()
        {
            //given
            Guid randomAdvertId = Guid.NewGuid();
            Guid someAdvertId = randomAdvertId;
            Advert invalidStorageAdvert = null;
            var notFoundAdvertException = new NotFoundAdvertException(
                message: $"Couldn't find advert with id: {someAdvertId}.");

            var expectedAdvertValidationException =
                new AdvertValidationException(
                    message: "Advert validation error occurred, please try again.",
                    innerException: notFoundAdvertException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageAdvert);

            //when
            ValueTask<Advert> retrieveAdvertByIdTask =
                this.advertService.RetrieveAdvertByIdAsync(someAdvertId);

            AdvertValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<AdvertValidationException>(
                 retrieveAdvertByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}