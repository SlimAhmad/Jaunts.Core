// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectors
{
    public partial class ProvidersDirectorServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomProvidersDirectorId = default;
            Guid inputProvidersDirectorId = randomProvidersDirectorId;

            var invalidProvidersDirectorException = new InvalidProvidersDirectorException(
                message: "Invalid ProvidersDirector. Please fix the errors and try again.");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.Id),
                values: "Id is required");

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorValidationException(
                    message: "ProvidersDirector validation error occurred, Please try again.",
                    innerException: invalidProvidersDirectorException);

            // when
            ValueTask<ProvidersDirector> actualProvidersDirectorTask =
                this.providersDirectorService.RemoveProvidersDirectorByIdAsync(inputProvidersDirectorId);

            ProvidersDirectorValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                 actualProvidersDirectorTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProvidersDirectorAsync(It.IsAny<ProvidersDirector>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageProvidersDirectorIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProvidersDirector randomProvidersDirector = CreateRandomProvidersDirector(dateTime);
            Guid inputProvidersDirectorId = randomProvidersDirector.Id;
            ProvidersDirector inputProvidersDirector = randomProvidersDirector;
            ProvidersDirector nullStorageProvidersDirector = null;

            var notFoundProvidersDirectorException = new NotFoundProvidersDirectorException(inputProvidersDirectorId);

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorValidationException(
                    message: "ProvidersDirector validation error occurred, Please try again.",
                    innerException: notFoundProvidersDirectorException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorByIdAsync(inputProvidersDirectorId))
                    .ReturnsAsync(nullStorageProvidersDirector);

            // when
            ValueTask<ProvidersDirector> actualProvidersDirectorTask =
                this.providersDirectorService.RemoveProvidersDirectorByIdAsync(inputProvidersDirectorId);

            ProvidersDirectorValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                 actualProvidersDirectorTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(inputProvidersDirectorId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProvidersDirectorAsync(It.IsAny<ProvidersDirector>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
