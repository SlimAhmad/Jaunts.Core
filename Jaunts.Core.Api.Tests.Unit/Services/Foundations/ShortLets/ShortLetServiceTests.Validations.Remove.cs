// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLets
{
    public partial class ShortLetServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
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

            // when
            ValueTask<ShortLet> actualShortLetTask =
                this.shortLetService.RemoveShortLetByIdAsync(inputShortLetId);

            ShortLetValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ShortLetValidationException>(
                 actualShortLetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShortLetAsync(It.IsAny<ShortLet>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageShortLetIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ShortLet randomShortLet = CreateRandomShortLet(dateTime);
            Guid inputShortLetId = randomShortLet.Id;
            ShortLet inputShortLet = randomShortLet;
            ShortLet nullStorageShortLet = null;

            var notFoundShortLetException = new NotFoundShortLetException(inputShortLetId);

            var expectedShortLetValidationException =
                new ShortLetValidationException(
                    message: "ShortLet validation error occurred, please try again.",
                    innerException: notFoundShortLetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetByIdAsync(inputShortLetId))
                    .ReturnsAsync(nullStorageShortLet);

            // when
            ValueTask<ShortLet> actualShortLetTask =
                this.shortLetService.RemoveShortLetByIdAsync(inputShortLetId);

            ShortLetValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ShortLetValidationException>(
                 actualShortLetTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(inputShortLetId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShortLetAsync(It.IsAny<ShortLet>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
