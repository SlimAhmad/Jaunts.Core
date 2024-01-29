// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Hosting;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDeals
{
    public partial class FlightDealServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
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

            //when
            ValueTask<FlightDeal> retrieveFlightDealByIdTask =
                this.flightDealService.RetrieveFlightDealByIdAsync(inputFlightDealId);

            FlightDealValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<FlightDealValidationException>(
                 retrieveFlightDealByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageFlightDealIsNullAndLogItAsync()
        {
            //given
            Guid randomFlightDealId = Guid.NewGuid();
            Guid someFlightDealId = randomFlightDealId;
            FlightDeal invalidStorageFlightDeal = null;
            var notFoundFlightDealException = new NotFoundFlightDealException(
                message: $"Couldn't find FlightDeal with id: {someFlightDealId}.");

            var expectedFlightDealValidationException =
                new FlightDealValidationException(
                    message: "FlightDeal validation error occurred, please try again.",
                    innerException: notFoundFlightDealException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageFlightDeal);

            //when
            ValueTask<FlightDeal> retrieveFlightDealByIdTask =
                this.flightDealService.RetrieveFlightDealByIdAsync(someFlightDealId);

            FlightDealValidationException actualAttachmentValidationException =
             await Assert.ThrowsAsync<FlightDealValidationException>(
                 retrieveFlightDealByIdTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedFlightDealValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}