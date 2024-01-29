// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOffers
{
    public partial class PromosOfferServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenPromosOfferIsNullAndLogItAsync()
        {
            // given
            PromosOffer randomPromosOffer = null;
            PromosOffer nullPromosOffer = randomPromosOffer;

            var nullPromosOfferException = new NullPromosOffersException(
                message: "The PromosOffer is null.");

            var expectedPromosOffersValidationException =
                new PromosOffersValidationException(
                    message: "PromosOffer validation error occurred, please try again.",
                    innerException: nullPromosOfferException);

            // when
            ValueTask<PromosOffer> createPromosOfferTask =
                this.promosOfferService.CreatePromosOfferAsync(nullPromosOffer);

             PromosOffersValidationException actualPromosOfferDependencyValidationException =
             await Assert.ThrowsAsync<PromosOffersValidationException>(
                 createPromosOfferTask.AsTask);

            // then
            actualPromosOfferDependencyValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateIfPromosOfferStatusIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            PromosOffer randomPromosOffer = CreateRandomPromosOffer(randomDateTime);
            PromosOffer invalidPromosOffer = randomPromosOffer;
            invalidPromosOffer.UpdatedBy = randomPromosOffer.CreatedBy;
            invalidPromosOffer.Status = GetInvalidEnum<PromosOffersStatus>();

            var invalidPromosOfferException = new InvalidPromosOffersException();

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.Status),
                values: "Value is not recognized");

            var expectedPromosOffersValidationException = new PromosOffersValidationException(
                message: "PromosOffer validation error occurred, please try again.",
                innerException: invalidPromosOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<PromosOffer> createPromosOfferTask =
                this.promosOfferService.CreatePromosOfferAsync(invalidPromosOffer);

            PromosOffersValidationException actualPromosOfferDependencyValidationException =
            await Assert.ThrowsAsync<PromosOffersValidationException>(
                createPromosOfferTask.AsTask);

            // then
            actualPromosOfferDependencyValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAsync(It.IsAny<PromosOffer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenPromosOfferIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidPromosOffer = new PromosOffer
            {
                Description = invalidText,
                CodeOrName = invalidText

            };

            var invalidPromosOfferException = new InvalidPromosOffersException();

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.Id),
                values: "Id is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.ProviderId),
                values: "Id is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.CodeOrName),
                values: "Text is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.Description),
                values: "Text is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.StartDate),
                values: "Date is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.EndDate),
                values: "Date is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.CreatedBy),
                values: "Id is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.UpdatedBy),
                values: "Id is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.CreatedDate),
                values: "Date is required");

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.UpdatedDate),
                values: "Date is required");

            var expectedPromosOffersValidationException =
                new PromosOffersValidationException(
                    message: "PromosOffer validation error occurred, please try again.",
                    innerException: invalidPromosOfferException);

            // when
            ValueTask<PromosOffer> createPromosOfferTask =
                this.promosOfferService.CreatePromosOfferAsync(invalidPromosOffer);

             PromosOffersValidationException actualPromosOfferDependencyValidationException =
             await Assert.ThrowsAsync<PromosOffersValidationException>(
                 createPromosOfferTask.AsTask);

            // then
            actualPromosOfferDependencyValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(It.IsAny<Guid>()),
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
            PromosOffer randomPromosOffer = CreateRandomPromosOffer(dateTime);
            PromosOffer inputPromosOffer = randomPromosOffer;
            inputPromosOffer.UpdatedBy = Guid.NewGuid();

            var invalidPromosOfferException = new InvalidPromosOffersException();

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.UpdatedBy),
                values: $"Id is not the same as {nameof(PromosOffer.CreatedBy)}");

            var expectedPromosOffersValidationException =
                new PromosOffersValidationException(
                    message: "PromosOffer validation error occurred, please try again.",
                    innerException: invalidPromosOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<PromosOffer> createPromosOfferTask =
                this.promosOfferService.CreatePromosOfferAsync(inputPromosOffer);

             PromosOffersValidationException actualPromosOfferDependencyValidationException =
             await Assert.ThrowsAsync<PromosOffersValidationException>(
                 createPromosOfferTask.AsTask);

            // then
            actualPromosOfferDependencyValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(It.IsAny<Guid>()),
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
            PromosOffer randomPromosOffer = CreateRandomPromosOffer(dateTime);
            PromosOffer inputPromosOffer = randomPromosOffer;
            inputPromosOffer.UpdatedBy = randomPromosOffer.CreatedBy;
            inputPromosOffer.UpdatedDate = GetRandomDateTime();

            var invalidPromosOfferException = new InvalidPromosOffersException();

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.UpdatedDate),
                values: $"Date is not the same as {nameof(PromosOffer.CreatedDate)}");

            var expectedPromosOffersValidationException =
                new PromosOffersValidationException(
                    message: "PromosOffer validation error occurred, please try again.",
                    innerException: invalidPromosOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<PromosOffer> createPromosOfferTask =
                this.promosOfferService.CreatePromosOfferAsync(inputPromosOffer);

             PromosOffersValidationException actualPromosOfferDependencyValidationException =
             await Assert.ThrowsAsync<PromosOffersValidationException>(
                 createPromosOfferTask.AsTask);

            // then
            actualPromosOfferDependencyValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(It.IsAny<Guid>()),
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
            PromosOffer randomPromosOffer = CreateRandomPromosOffer(dateTime);
            PromosOffer inputPromosOffer = randomPromosOffer;
            inputPromosOffer.UpdatedBy = inputPromosOffer.CreatedBy;
            inputPromosOffer.CreatedDate = dateTime.AddMinutes(minutes);
            inputPromosOffer.UpdatedDate = inputPromosOffer.CreatedDate;

            var invalidPromosOfferException = new InvalidPromosOffersException();

            invalidPromosOfferException.AddData(
                key: nameof(PromosOffer.CreatedDate),
                values: $"Date is not recent");

            var expectedPromosOffersValidationException =
                new PromosOffersValidationException(
                    message: "PromosOffer validation error occurred, please try again.",
                    innerException: invalidPromosOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<PromosOffer> createPromosOfferTask =
                this.promosOfferService.CreatePromosOfferAsync(inputPromosOffer);

             PromosOffersValidationException actualPromosOfferDependencyValidationException =
             await Assert.ThrowsAsync<PromosOffersValidationException>(
                 createPromosOfferTask.AsTask);

            // then
            actualPromosOfferDependencyValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenPromosOfferAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            PromosOffer randomPromosOffer = CreateRandomPromosOffer(dateTime);
            PromosOffer alreadyExistsPromosOffer = randomPromosOffer;
            alreadyExistsPromosOffer.UpdatedBy = alreadyExistsPromosOffer.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsPromosOfferException =
                new AlreadyExistsPromosOffersException(
                   message: "PromosOffer with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedPromosOffersValidationException =
                new PromosOffersDependencyValidationException(
                    message: "PromosOffer dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsPromosOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPromosOfferAsync(alreadyExistsPromosOffer))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<PromosOffer> createPromosOfferTask =
                this.promosOfferService.CreatePromosOfferAsync(alreadyExistsPromosOffer);

             PromosOffersDependencyValidationException actualPromosOfferDependencyValidationException =
             await Assert.ThrowsAsync<PromosOffersDependencyValidationException>(
                 createPromosOfferTask.AsTask);

            // then
            actualPromosOfferDependencyValidationException.Should().BeEquivalentTo(
                expectedPromosOffersValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedPromosOffersValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAsync(alreadyExistsPromosOffer),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
