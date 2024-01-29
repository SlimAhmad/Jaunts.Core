// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Adverts
{
    public partial class AdvertServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenAdvertIsNullAndLogItAsync()
        {
            // given
            Advert randomAdvert = null;
            Advert nullAdvert = randomAdvert;

            var nullAdvertException = new NullAdvertException(
                message: "The advert is null.");

            var expectedAdvertValidationException =
                new AdvertValidationException(
                    message: "Advert validation error occurred, please try again.",
                    innerException: nullAdvertException);

            // when
            ValueTask<Advert> createAdvertTask =
                this.advertService.CreateAdvertAsync(nullAdvert);

             AdvertValidationException actualAdvertDependencyValidationException =
             await Assert.ThrowsAsync<AdvertValidationException>(
                 createAdvertTask.AsTask);

            // then
            actualAdvertDependencyValidationException.Should().BeEquivalentTo(
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
        public async void ShouldThrowValidationExceptionOnCreateIfAdvertStatusIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Advert randomAdvert = CreateRandomAdvert(randomDateTime);
            Advert invalidAdvert = randomAdvert;
            invalidAdvert.UpdatedBy = randomAdvert.CreatedBy;
            invalidAdvert.Status = GetInvalidEnum<AdvertStatus>();

            var invalidAdvertException = new InvalidAdvertException();

            invalidAdvertException.AddData(
                key: nameof(Advert.Status),
                values: "Value is not recognized");

            var expectedAdvertValidationException = new AdvertValidationException(
                message: "Advert validation error occurred, please try again.",
                innerException: invalidAdvertException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<Advert> createAdvertTask =
                this.advertService.CreateAdvertAsync(invalidAdvert);

            AdvertValidationException actualAdvertDependencyValidationException =
            await Assert.ThrowsAsync<AdvertValidationException>(
                createAdvertTask.AsTask);

            // then
            actualAdvertDependencyValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAsync(It.IsAny<Advert>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenAdvertIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidAdvert = new Advert
            {
                Description = invalidText,
            };

            var invalidAdvertException = new InvalidAdvertException();

            invalidAdvertException.AddData(
                key: nameof(Advert.Id),
                values: "Id is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.ProviderId),
                values: "Id is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.TransactionFeeId),
                values: "Id is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.Description),
                values: "Text is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.CreatedBy),
                values: "Id is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.UpdatedBy),
                values: "Id is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.StartDate),
                values: "Date is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.EndDate),
                values: "Date is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.CreatedDate),
                values: "Date is required");

            invalidAdvertException.AddData(
                key: nameof(Advert.UpdatedDate),
                values: "Date is required");

            var expectedAdvertValidationException =
                new AdvertValidationException(
                    message: "Advert validation error occurred, please try again.",
                    innerException: invalidAdvertException);

            // when
            ValueTask<Advert> createAdvertTask =
                this.advertService.CreateAdvertAsync(invalidAdvert);

             AdvertValidationException actualAdvertDependencyValidationException =
             await Assert.ThrowsAsync<AdvertValidationException>(
                 createAdvertTask.AsTask);

            // then
            actualAdvertDependencyValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
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
            Advert randomAdvert = CreateRandomAdvert(dateTime);
            Advert inputAdvert = randomAdvert;
            inputAdvert.UpdatedBy = Guid.NewGuid();

            var invalidAdvertException = new InvalidAdvertException();

            invalidAdvertException.AddData(
                key: nameof(Advert.UpdatedBy),
                values: $"Id is not the same as {nameof(Advert.CreatedBy)}");

            var expectedAdvertValidationException =
                new AdvertValidationException(
                    message: "Advert validation error occurred, please try again.",
                    innerException: invalidAdvertException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Advert> createAdvertTask =
                this.advertService.CreateAdvertAsync(inputAdvert);

             AdvertValidationException actualAdvertDependencyValidationException =
             await Assert.ThrowsAsync<AdvertValidationException>(
                 createAdvertTask.AsTask);

            // then
            actualAdvertDependencyValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
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
            Advert randomAdvert = CreateRandomAdvert(dateTime);
            Advert inputAdvert = randomAdvert;
            inputAdvert.UpdatedBy = randomAdvert.CreatedBy;
            inputAdvert.UpdatedDate = GetRandomDateTime();

            var invalidAdvertException = new InvalidAdvertException();

            invalidAdvertException.AddData(
                key: nameof(Advert.UpdatedDate),
                values: $"Date is not the same as {nameof(Advert.CreatedDate)}");

            var expectedAdvertValidationException =
                new AdvertValidationException(
                    message: "Advert validation error occurred, please try again.",
                    innerException: invalidAdvertException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<Advert> createAdvertTask =
                this.advertService.CreateAdvertAsync(inputAdvert);

             AdvertValidationException actualAdvertDependencyValidationException =
             await Assert.ThrowsAsync<AdvertValidationException>(
                 createAdvertTask.AsTask);

            // then
            actualAdvertDependencyValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
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
            Advert randomAdvert = CreateRandomAdvert(dateTime);
            Advert inputAdvert = randomAdvert;
            inputAdvert.UpdatedBy = inputAdvert.CreatedBy;
            inputAdvert.CreatedDate = dateTime.AddMinutes(minutes);
            inputAdvert.UpdatedDate = inputAdvert.CreatedDate;

            var invalidAdvertException = new InvalidAdvertException();

            invalidAdvertException.AddData(
                key: nameof(Advert.CreatedDate),
                values: $"Date is not recent");

            var expectedAdvertValidationException =
                new AdvertValidationException(
                    message: "Advert validation error occurred, please try again.",
                    innerException: invalidAdvertException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Advert> createAdvertTask =
                this.advertService.CreateAdvertAsync(inputAdvert);

             AdvertValidationException actualAdvertDependencyValidationException =
             await Assert.ThrowsAsync<AdvertValidationException>(
                 createAdvertTask.AsTask);

            // then
            actualAdvertDependencyValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenAdvertAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Advert randomAdvert = CreateRandomAdvert(dateTime);
            Advert alreadyExistsAdvert = randomAdvert;
            alreadyExistsAdvert.UpdatedBy = alreadyExistsAdvert.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsAdvertException =
                new AlreadyExistsAdvertException(
                   message: "Advert with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedAdvertValidationException =
                new AdvertDependencyValidationException(
                    message: "Advert dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsAdvertException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAdvertAsync(alreadyExistsAdvert))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Advert> createAdvertTask =
                this.advertService.CreateAdvertAsync(alreadyExistsAdvert);

             AdvertDependencyValidationException actualAdvertDependencyValidationException =
             await Assert.ThrowsAsync<AdvertDependencyValidationException>(
                 createAdvertTask.AsTask);

            // then
            actualAdvertDependencyValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAsync(alreadyExistsAdvert),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
