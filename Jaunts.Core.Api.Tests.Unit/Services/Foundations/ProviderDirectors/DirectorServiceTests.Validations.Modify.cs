// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectors
{
    public partial class ProvidersDirectorServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenProvidersDirectorIsNullAndLogItAsync()
        {
            //given
            ProvidersDirector invalidProvidersDirector = null;
            var nullProvidersDirectorException = new NullProvidersDirectorException();

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorValidationException(
                    message: "ProvidersDirector validation error occurred, Please try again.",
                    nullProvidersDirectorException);

            //when
            ValueTask<ProvidersDirector> modifyProvidersDirectorTask =
                this.providersDirectorService.ModifyProvidersDirectorAsync(invalidProvidersDirector);

            ProvidersDirectorValidationException actualAttachmentValidationException =
                 await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                     modifyProvidersDirectorTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProvidersDirectorAsync(It.IsAny<ProvidersDirector>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ShouldThrowValidationExceptionOnModifyIfProvidersDirectorIsInvalidAndLogItAsync(
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
                key: nameof(ProvidersDirector.ContactNumber),
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
                key: nameof(ProvidersDirector.Position),
                values: "Text is required");

            invalidProvidersDirectorException.AddData(
               key: nameof(ProvidersDirector.Address),
               values: "Text is required");

            invalidProvidersDirectorException.AddData(
               key: nameof(ProvidersDirector.Description),
               values: "Text is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.CreatedDate),
                values: "Date is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.UpdatedDate),
            "Date is required",
                $"Date is the same as {nameof(ProvidersDirector.CreatedDate)}");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.CreatedBy),
                values: "Id is required");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.UpdatedBy),
                values: "Id is required");

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorValidationException(invalidProvidersDirectorException);

            // when
            ValueTask<ProvidersDirector> createProvidersDirectorTask =
                this.providersDirectorService.ModifyProvidersDirectorAsync(invalidProvidersDirector);

            // then
            await Assert.ThrowsAsync<ProvidersDirectorValidationException>(() =>
                createProvidersDirectorTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAsync(It.IsAny<ProvidersDirector>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            ProvidersDirector randomProvidersDirector = CreateRandomProvidersDirector(dateTime);
            ProvidersDirector inputProvidersDirector = randomProvidersDirector;

            var invalidProvidersDirectorException = new InvalidProvidersDirectorException(
                message: "Invalid ProvidersDirector. Please fix the errors and try again.");

            invalidProvidersDirectorException.AddData(
               key: nameof(ProvidersDirector.UpdatedDate),
               values: $"Date is the same as {nameof(inputProvidersDirector.CreatedDate)}");

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorValidationException(
                    message: "ProvidersDirector validation error occurred, Please try again.",
                    innerException: invalidProvidersDirectorException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<ProvidersDirector> modifyProvidersDirectorTask =
                this.providersDirectorService.ModifyProvidersDirectorAsync(inputProvidersDirector);

            ProvidersDirectorValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                modifyProvidersDirectorTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProvidersDirectorAsync(It.IsAny<ProvidersDirector>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProvidersDirector randomProvidersDirector = CreateRandomModifyProvidersDirector(dateTime);
            ProvidersDirector inputProvidersDirector = randomProvidersDirector;
            inputProvidersDirector.UpdatedBy = inputProvidersDirector.CreatedBy;
            inputProvidersDirector.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidProvidersDirectorException = new InvalidProvidersDirectorException(
                message: "Invalid ProvidersDirector. Please fix the errors and try again.");

            invalidProvidersDirectorException.AddData(
                   key: nameof(ProvidersDirector.UpdatedDate),
                   values: "Date is not recent");

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorValidationException(
                    message: "ProvidersDirector validation error occurred, Please try again.",
                    innerException: invalidProvidersDirectorException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ProvidersDirector> modifyProvidersDirectorTask =
                this.providersDirectorService.ModifyProvidersDirectorAsync(inputProvidersDirector);

            ProvidersDirectorValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                modifyProvidersDirectorTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProvidersDirectorAsync(It.IsAny<ProvidersDirector>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfProvidersDirectorDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            ProvidersDirector randomProvidersDirector = CreateRandomProvidersDirector(dateTime);
            ProvidersDirector nonExistentProvidersDirector = randomProvidersDirector;
            nonExistentProvidersDirector.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            ProvidersDirector noProvidersDirector = null;
            var notFoundProvidersDirectorException = new NotFoundProvidersDirectorException(nonExistentProvidersDirector.Id);

            var expectedProvidersDirectorValidationException =
                new ProvidersDirectorValidationException(
                    message: "ProvidersDirector validation error occurred, Please try again.",
                    innerException: notFoundProvidersDirectorException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorByIdAsync(nonExistentProvidersDirector.Id))
                    .ReturnsAsync(noProvidersDirector);

            // when
            ValueTask<ProvidersDirector> modifyProvidersDirectorTask =
                this.providersDirectorService.ModifyProvidersDirectorAsync(nonExistentProvidersDirector);

            ProvidersDirectorValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                modifyProvidersDirectorTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(nonExistentProvidersDirector.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProvidersDirectorAsync(It.IsAny<ProvidersDirector>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetNegativeRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            ProvidersDirector randomProvidersDirector = CreateRandomModifyProvidersDirector(randomDateTimeOffset);
            ProvidersDirector invalidProvidersDirector = randomProvidersDirector.DeepClone();
            ProvidersDirector storageProvidersDirector = invalidProvidersDirector.DeepClone();
            storageProvidersDirector.CreatedDate = storageProvidersDirector.CreatedDate.AddMinutes(randomMinutes);
            storageProvidersDirector.UpdatedDate = storageProvidersDirector.UpdatedDate.AddMinutes(randomMinutes);
            Guid ProvidersDirectorId = invalidProvidersDirector.Id;
          

            var invalidProvidersDirectorException = new InvalidProvidersDirectorException(
               message: "Invalid ProvidersDirector. Please fix the errors and try again.");

            invalidProvidersDirectorException.AddData(
                 key: nameof(ProvidersDirector.CreatedDate),
                 values: $"Date is not the same as {nameof(ProvidersDirector.CreatedDate)}");

            var expectedProvidersDirectorValidationException =
              new ProvidersDirectorValidationException(
                  message: "ProvidersDirector validation error occurred, Please try again.",
                  innerException: invalidProvidersDirectorException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorByIdAsync(ProvidersDirectorId))
                    .ReturnsAsync(storageProvidersDirector);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ProvidersDirector> modifyProvidersDirectorTask =
                this.providersDirectorService.ModifyProvidersDirectorAsync(invalidProvidersDirector);

            ProvidersDirectorValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                modifyProvidersDirectorTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(invalidProvidersDirector.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProvidersDirectorAsync(It.IsAny<ProvidersDirector>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int randomPositiveMinutes = GetRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();
            ProvidersDirector randomProvidersDirector = CreateRandomModifyProvidersDirector(randomDateTimeOffset);
            ProvidersDirector invalidProvidersDirector = randomProvidersDirector.DeepClone();
            ProvidersDirector storageProvidersDirector = invalidProvidersDirector.DeepClone();
            storageProvidersDirector.UpdatedDate = storageProvidersDirector.UpdatedDate.AddMinutes(randomPositiveMinutes);
            Guid ProvidersDirectorId = invalidProvidersDirector.Id;
            invalidProvidersDirector.CreatedBy = invalidCreatedBy;

            var invalidProvidersDirectorException = new InvalidProvidersDirectorException(
                message: "Invalid ProvidersDirector. Please fix the errors and try again.");

            invalidProvidersDirectorException.AddData(
                key: nameof(ProvidersDirector.CreatedBy),
                values: $"Id is not the same as {nameof(ProvidersDirector.CreatedBy)}");

            var expectedProvidersDirectorValidationException =
              new ProvidersDirectorValidationException(
                  message: "ProvidersDirector validation error occurred, Please try again.",
                  innerException: invalidProvidersDirectorException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorByIdAsync(ProvidersDirectorId))
                    .ReturnsAsync(storageProvidersDirector);

            // when
            ValueTask<ProvidersDirector> modifyProvidersDirectorTask =
                this.providersDirectorService.ModifyProvidersDirectorAsync(invalidProvidersDirector);

            ProvidersDirectorValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                modifyProvidersDirectorTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(invalidProvidersDirector.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProvidersDirectorAsync(It.IsAny<ProvidersDirector>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetCurrentDateTime();
            ProvidersDirector randomProvidersDirector = CreateRandomModifyProvidersDirector(randomDate);
            ProvidersDirector invalidProvidersDirector = randomProvidersDirector;
            invalidProvidersDirector.UpdatedDate = randomDate;
            ProvidersDirector storageProvidersDirector = randomProvidersDirector.DeepClone();
            Guid ProvidersDirectorId = invalidProvidersDirector.Id;

            var invalidProvidersDirectorException = new InvalidProvidersDirectorException(
               message: "Invalid ProvidersDirector. Please fix the errors and try again.");

            invalidProvidersDirectorException.AddData(
               key: nameof(ProvidersDirector.UpdatedDate),
               values: $"Date is the same as {nameof(invalidProvidersDirector.UpdatedDate)}");

            var expectedProvidersDirectorValidationException =
              new ProvidersDirectorValidationException(
                  message: "ProvidersDirector validation error occurred, Please try again.",
                  innerException: invalidProvidersDirectorException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorByIdAsync(ProvidersDirectorId))
                    .ReturnsAsync(storageProvidersDirector);

            // when
            ValueTask<ProvidersDirector> modifyProvidersDirectorTask =
                this.providersDirectorService.ModifyProvidersDirectorAsync(invalidProvidersDirector);

            ProvidersDirectorValidationException actualAttachmentValidationException =
            await Assert.ThrowsAsync<ProvidersDirectorValidationException>(
                modifyProvidersDirectorTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedProvidersDirectorValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(invalidProvidersDirector.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProvidersDirectorAsync(It.IsAny<ProvidersDirector>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
