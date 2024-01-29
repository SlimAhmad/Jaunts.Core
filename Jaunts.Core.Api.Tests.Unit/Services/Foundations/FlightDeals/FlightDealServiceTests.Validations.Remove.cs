// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDeals
{
    public partial class FlightDealServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomFlightDealId = default;
            Guid inputFlightDealId = randomFlightDealId;

            var invalidFlightDealException = new InvalidFlightDealException(
                message: "Invalid flightDeal. Please fix the errors and try again.");

            invalidFlightDealException.AddData(
                key: nameof(FlightDeal.Id),
                values: "Id is required");

            var expectedFlightDealValidationException =
                new FlightDealValidationException(
                    message: "FlightDeal validation error occurred, please try again.",
                    innerException: invalidFlightDealException);

            // when
            ValueTask<FlightDeal> actualFlightDealTask =
                this.flightDealService.RemoveFlightDealByIdAsync(inputFlightDealId);

            FlightDealValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<FlightDealValidationException>(
                 actualFlightDealTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFlightDealAsync(It.IsAny<FlightDeal>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageFlightDealIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            FlightDeal randomFlightDeal = CreateRandomFlightDeal(dateTime);
            Guid inputFlightDealId = randomFlightDeal.Id;
            FlightDeal inputFlightDeal = randomFlightDeal;
            FlightDeal nullStorageFlightDeal = null;

            var notFoundFlightDealException = new NotFoundFlightDealException(inputFlightDealId);

            var expectedFlightDealValidationException =
                new FlightDealValidationException(
                    message: "FlightDeal validation error occurred, please try again.",
                    innerException: notFoundFlightDealException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealByIdAsync(inputFlightDealId))
                    .ReturnsAsync(nullStorageFlightDeal);

            // when
            ValueTask<FlightDeal> actualFlightDealTask =
                this.flightDealService.RemoveFlightDealByIdAsync(inputFlightDealId);

            FlightDealValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<FlightDealValidationException>(
                 actualFlightDealTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(inputFlightDealId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFlightDealAsync(It.IsAny<FlightDeal>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
