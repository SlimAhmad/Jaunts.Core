// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.TransactionFees
{
    public partial class TransactionFeeServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenTransactionFeeIsNullAndLogItAsync()
        {
            // given
            TransactionFee randomTransactionFee = null;
            TransactionFee nullTransactionFee = randomTransactionFee;

            var nullTransactionFeeException = new NullTransactionFeeException(
                message: "The TransactionFee is null.");

            var expectedTransactionFeeValidationException =
                new TransactionFeeValidationException(
                    message: "TransactionFee validation error occurred, Please try again.",
                    innerException: nullTransactionFeeException);

            // when
            ValueTask<TransactionFee> createTransactionFeeTask =
                this.transactionFeeService.CreateTransactionFeeAsync(nullTransactionFee);

             TransactionFeeValidationException actualTransactionFeeDependencyValidationException =
             await Assert.ThrowsAsync<TransactionFeeValidationException>(
                 createTransactionFeeTask.AsTask);

            // then
            actualTransactionFeeDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateIfTransactionFeeStatusIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(randomDateTime);
            TransactionFee invalidTransactionFee = randomTransactionFee;
            invalidTransactionFee.UpdatedBy = randomTransactionFee.CreatedBy;
            invalidTransactionFee.Status = GetInvalidEnum<TransactionFeesStatus>();

            var invalidTransactionFeeException = new InvalidTransactionFeeException();

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.Status),
                values: "Value is not recognized");

            var expectedTransactionFeeValidationException = new TransactionFeeValidationException(
                message: "TransactionFee validation error occurred, Please try again.",
                innerException: invalidTransactionFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<TransactionFee> createTransactionFeeTask =
                this.transactionFeeService.CreateTransactionFeeAsync(invalidTransactionFee);

            TransactionFeeValidationException actualTransactionFeeDependencyValidationException =
            await Assert.ThrowsAsync<TransactionFeeValidationException>(
                createTransactionFeeTask.AsTask);

            // then
            actualTransactionFeeDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTransactionFeeAsync(It.IsAny<TransactionFee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenTransactionFeeIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidTransactionFee = new TransactionFee
            {
                Description = invalidText,
                Name = invalidText

            };

            var invalidTransactionFeeException = new InvalidTransactionFeeException();

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.Id),
                values: "Id is required");

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.Name),
                values: "Text is required");

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.Description),
                values: "Text is required");

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.CreatedBy),
                values: "Id is required");

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.UpdatedBy),
                values: "Id is required");

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.CreatedDate),
                values: "Date is required");

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.UpdatedDate),
                values: "Date is required");

            var expectedTransactionFeeValidationException =
                new TransactionFeeValidationException(
                    message: "TransactionFee validation error occurred, Please try again.",
                    innerException: invalidTransactionFeeException);

            // when
            ValueTask<TransactionFee> createTransactionFeeTask =
                this.transactionFeeService.CreateTransactionFeeAsync(invalidTransactionFee);

             TransactionFeeValidationException actualTransactionFeeDependencyValidationException =
             await Assert.ThrowsAsync<TransactionFeeValidationException>(
                 createTransactionFeeTask.AsTask);

            // then
            actualTransactionFeeDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
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
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(dateTime);
            TransactionFee inputTransactionFee = randomTransactionFee;
            inputTransactionFee.UpdatedBy = Guid.NewGuid();

            var invalidTransactionFeeException = new InvalidTransactionFeeException();

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.UpdatedBy),
                values: $"Id is not the same as {nameof(TransactionFee.CreatedBy)}");

            var expectedTransactionFeeValidationException =
                new TransactionFeeValidationException(
                    message: "TransactionFee validation error occurred, Please try again.",
                    innerException: invalidTransactionFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<TransactionFee> createTransactionFeeTask =
                this.transactionFeeService.CreateTransactionFeeAsync(inputTransactionFee);

             TransactionFeeValidationException actualTransactionFeeDependencyValidationException =
             await Assert.ThrowsAsync<TransactionFeeValidationException>(
                 createTransactionFeeTask.AsTask);

            // then
            actualTransactionFeeDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
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
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(dateTime);
            TransactionFee inputTransactionFee = randomTransactionFee;
            inputTransactionFee.UpdatedBy = randomTransactionFee.CreatedBy;
            inputTransactionFee.UpdatedDate = GetRandomDateTime();

            var invalidTransactionFeeException = new InvalidTransactionFeeException();

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.UpdatedDate),
                values: $"Date is not the same as {nameof(TransactionFee.CreatedDate)}");

            var expectedTransactionFeeValidationException =
                new TransactionFeeValidationException(
                    message: "TransactionFee validation error occurred, Please try again.",
                    innerException: invalidTransactionFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<TransactionFee> createTransactionFeeTask =
                this.transactionFeeService.CreateTransactionFeeAsync(inputTransactionFee);

             TransactionFeeValidationException actualTransactionFeeDependencyValidationException =
             await Assert.ThrowsAsync<TransactionFeeValidationException>(
                 createTransactionFeeTask.AsTask);

            // then
            actualTransactionFeeDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
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
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(dateTime);
            TransactionFee inputTransactionFee = randomTransactionFee;
            inputTransactionFee.UpdatedBy = inputTransactionFee.CreatedBy;
            inputTransactionFee.CreatedDate = dateTime.AddMinutes(minutes);
            inputTransactionFee.UpdatedDate = inputTransactionFee.CreatedDate;

            var invalidTransactionFeeException = new InvalidTransactionFeeException();

            invalidTransactionFeeException.AddData(
                key: nameof(TransactionFee.CreatedDate),
                values: $"Date is not recent");

            var expectedTransactionFeeValidationException =
                new TransactionFeeValidationException(
                    message: "TransactionFee validation error occurred, Please try again.",
                    innerException: invalidTransactionFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<TransactionFee> createTransactionFeeTask =
                this.transactionFeeService.CreateTransactionFeeAsync(inputTransactionFee);

             TransactionFeeValidationException actualTransactionFeeDependencyValidationException =
             await Assert.ThrowsAsync<TransactionFeeValidationException>(
                 createTransactionFeeTask.AsTask);

            // then
            actualTransactionFeeDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenTransactionFeeAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(dateTime);
            TransactionFee alreadyExistsTransactionFee = randomTransactionFee;
            alreadyExistsTransactionFee.UpdatedBy = alreadyExistsTransactionFee.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsTransactionFeeException =
                new AlreadyExistsTransactionFeeException(
                   message: "TransactionFee with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedTransactionFeeValidationException =
                new TransactionFeeDependencyValidationException(
                    message: "TransactionFee dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsTransactionFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTransactionFeeAsync(alreadyExistsTransactionFee))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<TransactionFee> createTransactionFeeTask =
                this.transactionFeeService.CreateTransactionFeeAsync(alreadyExistsTransactionFee);

             TransactionFeeDependencyValidationException actualTransactionFeeDependencyValidationException =
             await Assert.ThrowsAsync<TransactionFeeDependencyValidationException>(
                 createTransactionFeeTask.AsTask);

            // then
            actualTransactionFeeDependencyValidationException.Should().BeEquivalentTo(
                expectedTransactionFeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedTransactionFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTransactionFeeAsync(alreadyExistsTransactionFee),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
