// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomShortLetId = default;
            Guid inputShortLetId = randomShortLetId;

            var invalidShortLetException = new InvalidShortLetException(
                message: "Invalid shortLet. Please fix the errors and try again.");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.Id),
                values: "Id is required");

            var expectedShortLetValidationException =
                new ShortLetValidationException(
                    message: "ShortLet validation error occurred, please try again.", 
                    innerException: invalidShortLetException);

            //when
            ValueTask<ShortLet> retrieveShortLetByIdTask =
                this.shortLetService.RetrieveShortLetByIdAsync(inputShortLetId);

            ShortLetValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ShortLetValidationException>(
                 retrieveShortLetByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageShortLetIsNullAndLogItAsync()
        {
            //given
            Guid randomShortLetId = Guid.NewGuid();
            Guid someShortLetId = randomShortLetId;
            ShortLet invalidStorageShortLet = null;
            var notFoundShortLetException = new NotFoundShortLetException(
                message: $"Couldn't find ShortLet with id: {someShortLetId}.");

            var expectedShortLetValidationException =
                new ShortLetValidationException(
                    message: "ShortLet validation error occurred, please try again.",
                    innerException: notFoundShortLetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageShortLet);

            //when
            ValueTask<ShortLet> retrieveShortLetByIdTask =
                this.shortLetService.RetrieveShortLetByIdAsync(someShortLetId);

            ShortLetValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ShortLetValidationException>(
                 retrieveShortLetByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}