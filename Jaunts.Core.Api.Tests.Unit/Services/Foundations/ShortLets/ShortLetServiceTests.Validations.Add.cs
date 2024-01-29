// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLets
{
    public partial class ShortLetServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenShortLetIsNullAndLogItAsync()
        {
            // given
            ShortLet randomShortLet = null;
            ShortLet nullShortLet = randomShortLet;

            var nullShortLetException = new NullShortLetException(
                message: "The ShortLet is null.");

            var expectedShortLetValidationException =
                new ShortLetValidationException(
                    message: "ShortLet validation error occurred, please try again.",
                    innerException: nullShortLetException);

            // when
            ValueTask<ShortLet> createShortLetTask =
                this.shortLetService.CreateShortLetAsync(nullShortLet);

             ShortLetValidationException actualShortLetDependencyValidationException =
             await Assert.ThrowsAsync<ShortLetValidationException>(
                 createShortLetTask.AsTask);

            // then
            actualShortLetDependencyValidationException.Should().BeEquivalentTo(
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
        public async void ShouldThrowValidationExceptionOnCreateIfShortLetStatusIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ShortLet randomShortLet = CreateRandomShortLet(randomDateTime);
            ShortLet invalidShortLet = randomShortLet;
            invalidShortLet.UpdatedBy = randomShortLet.CreatedBy;
            invalidShortLet.Status = GetInvalidEnum<ShortLetStatus>();

            var invalidShortLetException = new InvalidShortLetException();

            invalidShortLetException.AddData(
                key: nameof(ShortLet.Status),
                values: "Value is not recognized");

            var expectedShortLetValidationException = new ShortLetValidationException(
                message: "ShortLet validation error occurred, please try again.",
                innerException: invalidShortLetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).
                    Returns(randomDateTime);

            // when
            ValueTask<ShortLet> createShortLetTask =
                this.shortLetService.CreateShortLetAsync(invalidShortLet);

            ShortLetValidationException actualShortLetDependencyValidationException =
            await Assert.ThrowsAsync<ShortLetValidationException>(
                createShortLetTask.AsTask);

            // then
            actualShortLetDependencyValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAsync(It.IsAny<ShortLet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenShortLetIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidShortLet = new ShortLet
            {
                Name = invalidText,
                Description = invalidText,
                Location = invalidText,
            };

            var invalidShortLetException = new InvalidShortLetException();

            invalidShortLetException.AddData(
                key: nameof(ShortLet.Id),
                values: "Id is required");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.Name),
                values: "Text is required");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.Location),
                values: "Text is required");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.Description),
                values: "Text is required");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.CreatedBy),
                values: "Id is required");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.UpdatedBy),
                values: "Id is required");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.CreatedDate),
                values: "Date is required");

            invalidShortLetException.AddData(
                key: nameof(ShortLet.UpdatedDate),
                values: "Date is required");

            var expectedShortLetValidationException =
                new ShortLetValidationException(
                    message: "ShortLet validation error occurred, please try again.",
                    innerException: invalidShortLetException);

            // when
            ValueTask<ShortLet> createShortLetTask =
                this.shortLetService.CreateShortLetAsync(invalidShortLet);

             ShortLetValidationException actualShortLetDependencyValidationException =
             await Assert.ThrowsAsync<ShortLetValidationException>(
                 createShortLetTask.AsTask);

            // then
            actualShortLetDependencyValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(It.IsAny<Guid>()),
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
            ShortLet randomShortLet = CreateRandomShortLet(dateTime);
            ShortLet inputShortLet = randomShortLet;
            inputShortLet.UpdatedBy = Guid.NewGuid();

            var invalidShortLetException = new InvalidShortLetException();

            invalidShortLetException.AddData(
                key: nameof(ShortLet.UpdatedBy),
                values: $"Id is not the same as {nameof(ShortLet.CreatedBy)}");

            var expectedShortLetValidationException =
                new ShortLetValidationException(
                    message: "ShortLet validation error occurred, please try again.",
                    innerException: invalidShortLetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ShortLet> createShortLetTask =
                this.shortLetService.CreateShortLetAsync(inputShortLet);

             ShortLetValidationException actualShortLetDependencyValidationException =
             await Assert.ThrowsAsync<ShortLetValidationException>(
                 createShortLetTask.AsTask);

            // then
            actualShortLetDependencyValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(It.IsAny<Guid>()),
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
            ShortLet randomShortLet = CreateRandomShortLet(dateTime);
            ShortLet inputShortLet = randomShortLet;
            inputShortLet.UpdatedBy = randomShortLet.CreatedBy;
            inputShortLet.UpdatedDate = GetRandomDateTime();

            var invalidShortLetException = new InvalidShortLetException();

            invalidShortLetException.AddData(
                key: nameof(ShortLet.UpdatedDate),
                values: $"Date is not the same as {nameof(ShortLet.CreatedDate)}");

            var expectedShortLetValidationException =
                new ShortLetValidationException(
                    message: "ShortLet validation error occurred, please try again.",
                    innerException: invalidShortLetException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<ShortLet> createShortLetTask =
                this.shortLetService.CreateShortLetAsync(inputShortLet);

             ShortLetValidationException actualShortLetDependencyValidationException =
             await Assert.ThrowsAsync<ShortLetValidationException>(
                 createShortLetTask.AsTask);

            // then
            actualShortLetDependencyValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(It.IsAny<Guid>()),
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
            ShortLet randomShortLet = CreateRandomShortLet(dateTime);
            ShortLet inputShortLet = randomShortLet;
            inputShortLet.UpdatedBy = inputShortLet.CreatedBy;
            inputShortLet.CreatedDate = dateTime.AddMinutes(minutes);
            inputShortLet.UpdatedDate = inputShortLet.CreatedDate;

            var invalidShortLetException = new InvalidShortLetException();

            invalidShortLetException.AddData(
                key: nameof(ShortLet.CreatedDate),
                values: $"Date is not recent");

            var expectedShortLetValidationException =
                new ShortLetValidationException(
                    message: "ShortLet validation error occurred, please try again.",
                    innerException: invalidShortLetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ShortLet> createShortLetTask =
                this.shortLetService.CreateShortLetAsync(inputShortLet);

             ShortLetValidationException actualShortLetDependencyValidationException =
             await Assert.ThrowsAsync<ShortLetValidationException>(
                 createShortLetTask.AsTask);

            // then
            actualShortLetDependencyValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenShortLetAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ShortLet randomShortLet = CreateRandomShortLet(dateTime);
            ShortLet alreadyExistsShortLet = randomShortLet;
            alreadyExistsShortLet.UpdatedBy = alreadyExistsShortLet.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsShortLetException =
                new AlreadyExistsShortLetException(
                   message: "ShortLet with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedShortLetValidationException =
                new ShortLetDependencyValidationException(
                    message: "ShortLet dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsShortLetException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertShortLetAsync(alreadyExistsShortLet))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ShortLet> createShortLetTask =
                this.shortLetService.CreateShortLetAsync(alreadyExistsShortLet);

             ShortLetDependencyValidationException actualShortLetDependencyValidationException =
             await Assert.ThrowsAsync<ShortLetDependencyValidationException>(
                 createShortLetTask.AsTask);

            // then
            actualShortLetDependencyValidationException.Should().BeEquivalentTo(
                expectedShortLetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedShortLetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAsync(alreadyExistsShortLet),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
