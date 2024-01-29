// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDeals
{
    public partial class FlightDealServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedFailedFlightDealStorageException =
              new FailedFlightDealStorageException(
                  message: "Failed flightDeal storage error occurred, please contact support.",
                  sqlException);

            var expectedFlightDealDependencyException =
                new FlightDealDependencyException(
                    message: "FlightDeal dependency error occurred, contact support.",
                    expectedFailedFlightDealStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFlightDeals())
                    .Throws(sqlException);

            // when
            Action retrieveAllFlightDealsAction = () =>
                this.flightDealService.RetrieveAllFlightDeals();

            FlightDealDependencyException actualDependencyException =
              Assert.Throws<FlightDealDependencyException>(
                 retrieveAllFlightDealsAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFlightDealDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFlightDeals(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedFlightDealDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedFlightDealServiceException =
              new FailedFlightDealServiceException(
                  message: "Failed flightDeal service error occurred, contact support.",
                  innerException: serviceException);

            var expectedFlightDealServiceException =
                new FlightDealServiceException(
                    message: "FlightDeal service error occurred, contact support.",
                    innerException: failedFlightDealServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFlightDeals())
                    .Throws(serviceException);

            // when
            Action retrieveAllFlightDealsAction = () =>
                this.flightDealService.RetrieveAllFlightDeals();

            FlightDealServiceException actualServiceException =
              Assert.Throws<FlightDealServiceException>(
                 retrieveAllFlightDealsAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedFlightDealServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFlightDeals(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
