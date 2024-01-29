// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Hosting;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectors
{
    public partial class ProvidersDirectorServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
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

            //when
            ValueTask<ProvidersDirector> retrieveProvidersDirectorByIdTask =
                this.providersDirectorService.RetrieveProvidersDirectorByIdAsync(inputProvidersDirectorId);

            ProvidersDirectorValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                 retrieveProvidersDirectorByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageProvidersDirectorIsNullAndLogItAsync()
        {
            //given
            Guid randomProvidersDirectorId = Guid.NewGuid();
            Guid someProvidersDirectorId = randomProvidersDirectorId;
            ProvidersDirector invalidStorageProvidersDirector = null;
            var notFoundProvidersDirectorException = new NotFoundProvidersDirectorException(
                message: $"Couldn't find ProvidersDirector with id: {someProvidersDirectorId}.");

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorValidationException(
                    message: "ProvidersDirector validation error occurred, Please try again.",
                    innerException: notFoundProvidersDirectorException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageProvidersDirector);

            //when
            ValueTask<ProvidersDirector> retrieveProvidersDirectorByIdTask =
                this.providersDirectorService.RetrieveProvidersDirectorByIdAsync(someProvidersDirectorId);

            ProvidersDirectorValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                 retrieveProvidersDirectorByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}