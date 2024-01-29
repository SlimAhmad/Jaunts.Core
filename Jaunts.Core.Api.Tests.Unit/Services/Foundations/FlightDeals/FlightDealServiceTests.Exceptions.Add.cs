// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDeals
{
    public partial class FlightDealServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            FlightDeal someFlightDeal = CreateRandomFlightDeal(dateTime);
            someFlightDeal.UpdatedBy = someFlightDeal.CreatedBy;
            var sqlException = GetSqlException();

            var expectedFailedFlightDealStorageException =
                new FailedFlightDealStorageException(
                    message: "Failed flightDeal storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedFlightDealDependencyException =
                new FlightDealDependencyException(
                    message: "FlightDeal dependency error occurred, contact support.",
                    innerException: expectedFailedFlightDealStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<FlightDeal> createFlightDealTask =
                this.flightDealService.CreateFlightDealAsync(someFlightDeal);

            FlightDealDependencyException actualDependencyException =
             await Assert.ThrowsAsync<FlightDealDependencyException>(
                 createFlightDealTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFlightDealDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedFlightDealDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAsync(It.IsAny<FlightDeal>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            FlightDeal someFlightDeal = CreateRandomFlightDeal(dateTime);
            someFlightDeal.UpdatedBy = someFlightDeal.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedFlightDealStorageException =
                new FailedFlightDealStorageException(
                    message: "Failed flightDeal storage error occurred, please contact support.",
                    databaseUpdateException);

            var expectedFlightDealDependencyException =
                new FlightDealDependencyException(
                    message: "FlightDeal dependency error occurred, contact support.",
                    expectedFailedFlightDealStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<FlightDeal> createFlightDealTask =
                this.flightDealService.CreateFlightDealAsync(someFlightDeal);

            FlightDealDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<FlightDealDependencyException>(
                     createFlightDealTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFlightDealDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAsync(It.IsAny<FlightDeal>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            FlightDeal someFlightDeal = CreateRandomFlightDeal(dateTime);
            someFlightDeal.UpdatedBy = someFlightDeal.CreatedBy;
            var serviceException = new Exception();

            var failedFlightDealServiceException =
                new FailedFlightDealServiceException(
                    message: "Failed flightDeal service error occurred, contact support.",
                    innerException: serviceException);

            var expectedFlightDealServiceException =
                new FlightDealServiceException(
                    message: "FlightDeal service error occurred, contact support.",
                    innerException: failedFlightDealServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<FlightDeal> createFlightDealTask =
                 this.flightDealService.CreateFlightDealAsync(someFlightDeal);

            FlightDealServiceException actualDependencyException =
                 await Assert.ThrowsAsync<FlightDealServiceException>(
                     createFlightDealTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFlightDealServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAsync(It.IsAny<FlightDeal>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
