// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectors
{
    public partial class ProvidersDirectorServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenProvidersDirectorIsNullAndLogItAsync()
        {
            // given
            ProvidersDirector randomProvidersDirector = null;
            ProvidersDirector nullProvidersDirector = randomProvidersDirector;

            var nullProvidersDirectorException = new NullProvidersDirectorException(
                message: "The ProvidersDirector is null.");

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorValidationException(
                    message: "ProvidersDirector validation error occurred, Please try again.",
                    innerException: nullProvidersDirectorException);

            // when
            ValueTask<ProvidersDirector> createProvidersDirectorTask =
                this.providersDirectorService.CreateProvidersDirectorAsync(nullProvidersDirector);

             ProvidersDirectorValidationException actualProvidersDirectorDependencyValidationException =
             await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                 createProvidersDirectorTask.AsTask);

            // then
            actualProvidersDirectorDependencyValidationException.Should().BeEquivalentTo(
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenProvidersDirectorIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidProvidersDirector = new ProvidersDirector
            {
                Address = invalidText,
                Description = invalidText,
                Position = invalidText,
                FirstName = invalidText,
                LastName = invalidText,
                MiddleName = invalidText,
                ContactNumber = invalidText,

            };

            var invalidProvidersDirectorException = new InvalidProvidersDirectorException();

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.Id),
                values: "Id is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.ProviderId),
                values: "Id is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.Address),
                values: "Text is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.Description),
                values: "Text is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.Position),
                values: "Text is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.FirstName),
                values: "Text is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.LastName),
                values: "Text is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.MiddleName),
                values: "Text is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.ContactNumber),
                values: "Text is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.CreatedBy),
                values: "Id is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.UpdatedBy),
                values: "Id is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.CreatedDate),
                values: "Date is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.UpdatedDate),
                values: "Date is required");

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorValidationException(
                    message: "ProvidersDirector validation error occurred, Please try again.",
                    innerException: invalidProvidersDirectorException);

            // when
            ValueTask<ProvidersDirector> createProvidersDirectorTask =
                this.providersDirectorService.CreateProvidersDirectorAsync(invalidProvidersDirector);

             ProvidersDirectorValidationException actualProvidersDirectorDependencyValidationException =
             await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                 createProvidersDirectorTask.AsTask);

            // then
            actualProvidersDirectorDependencyValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
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
            ProvidersDirector randomProvidersDirector = CreateRandomProvidersDirector(dateTime);
            ProvidersDirector inputProvidersDirector = randomProvidersDirector;
            inputProvidersDirector.UpdatedBy = Guid.NewGuid();

            var invalidProvidersDirectorException = new InvalidProvidersDirectorException();

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.UpdatedBy),
                values: $"Id is not the same as {nameof(ProvidersDirector.CreatedBy)}");

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorValidationException(
                    message: "ProvidersDirector validation error occurred, Please try again.",
                    innerException: invalidProvidersDirectorException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ProvidersDirector> createProvidersDirectorTask =
                this.providersDirectorService.CreateProvidersDirectorAsync(inputProvidersDirector);

             ProvidersDirectorValidationException actualProvidersDirectorDependencyValidationException =
             await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                 createProvidersDirectorTask.AsTask);

            // then
            actualProvidersDirectorDependencyValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
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
            ProvidersDirector randomProvidersDirector = CreateRandomProvidersDirector(dateTime);
            ProvidersDirector inputProvidersDirector = randomProvidersDirector;
            inputProvidersDirector.UpdatedBy = randomProvidersDirector.CreatedBy;
            inputProvidersDirector.UpdatedDate = GetRandomDateTime();

            var invalidProvidersDirectorException = new InvalidProvidersDirectorException();

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.UpdatedDate),
                values: $"Date is not the same as {nameof(ProvidersDirector.CreatedDate)}");

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorValidationException(
                    message: "ProvidersDirector validation error occurred, Please try again.",
                    innerException: invalidProvidersDirectorException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<ProvidersDirector> createProvidersDirectorTask =
                this.providersDirectorService.CreateProvidersDirectorAsync(inputProvidersDirector);

             ProvidersDirectorValidationException actualProvidersDirectorDependencyValidationException =
             await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                 createProvidersDirectorTask.AsTask);

            // then
            actualProvidersDirectorDependencyValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
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
            ProvidersDirector randomProvidersDirector = CreateRandomProvidersDirector(dateTime);
            ProvidersDirector inputProvidersDirector = randomProvidersDirector;
            inputProvidersDirector.UpdatedBy = inputProvidersDirector.CreatedBy;
            inputProvidersDirector.CreatedDate = dateTime.AddMinutes(minutes);
            inputProvidersDirector.UpdatedDate = inputProvidersDirector.CreatedDate;

            var invalidProvidersDirectorException = new InvalidProvidersDirectorException();

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.CreatedDate),
                values: $"Date is not recent");

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorValidationException(
                    message: "ProvidersDirector validation error occurred, Please try again.",
                    innerException: invalidProvidersDirectorException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ProvidersDirector> createProvidersDirectorTask =
                this.providersDirectorService.CreateProvidersDirectorAsync(inputProvidersDirector);

             ProvidersDirectorValidationException actualProvidersDirectorDependencyValidationException =
             await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                 createProvidersDirectorTask.AsTask);

            // then
            actualProvidersDirectorDependencyValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenProvidersDirectorAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProvidersDirector randomProvidersDirector = CreateRandomProvidersDirector(dateTime);
            ProvidersDirector alreadyExistsProvidersDirector = randomProvidersDirector;
            alreadyExistsProvidersDirector.UpdatedBy = alreadyExistsProvidersDirector.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsProvidersDirectorException =
                new AlreadyExistsProvidersDirectorException(
                   message: "ProvidersDirector with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorDependencyValidationException(
                    message: "ProvidersDirector dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsProvidersDirectorException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProvidersDirectorAsync(alreadyExistsProvidersDirector))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ProvidersDirector> createProvidersDirectorTask =
                this.providersDirectorService.CreateProvidersDirectorAsync(alreadyExistsProvidersDirector);

             ProvidersDirectorDependencyValidationException actualProvidersDirectorDependencyValidationException =
             await Assert.ThrowsAsync<ProvidersDirectorDependencyValidationException>(
                 createProvidersDirectorTask.AsTask);

            // then
            actualProvidersDirectorDependencyValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAsync(alreadyExistsProvidersDirector),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
