// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Adverts
{
    public partial class AdvertServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
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

            // when
            ValueTask<Advert> actualAdvertTask =
                this.advertService.RemoveAdvertByIdAsync(inputAdvertId);

            AdvertValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<AdvertValidationException>(
                 actualAdvertTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAdvertAsync(It.IsAny<Advert>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageAdvertIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Advert randomAdvert = CreateRandomAdvert(dateTime);
            Guid inputAdvertId = randomAdvert.Id;
            Advert inputAdvert = randomAdvert;
            Advert nullStorageAdvert = null;

            var notFoundAdvertException = new NotFoundAdvertException(inputAdvertId);

            var expectedAdvertValidationException =
                new AdvertValidationException(
                    message: "Advert validation error occurred, please try again.",
                    innerException: notFoundAdvertException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertByIdAsync(inputAdvertId))
                    .ReturnsAsync(nullStorageAdvert);

            // when
            ValueTask<Advert> actualAdvertTask =
                this.advertService.RemoveAdvertByIdAsync(inputAdvertId);

            AdvertValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<AdvertValidationException>(
                 actualAdvertTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(inputAdvertId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAdvertAsync(It.IsAny<Advert>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
