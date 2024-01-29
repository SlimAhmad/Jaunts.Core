// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions;
using Microsoft.Data.SqlClient;
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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            FlightDeal someFlightDeal = CreateRandomFlightDeal(randomDateTime);
            someFlightDeal.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

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
            ValueTask<FlightDeal> modifyFlightDealTask =
                this.flightDealService.ModifyFlightDealAsync(someFlightDeal);

                FlightDealDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<FlightDealDependencyException>(
                     modifyFlightDealTask.AsTask);

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
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            FlightDeal someFlightDeal = CreateRandomFlightDeal(randomDateTime);
            someFlightDeal.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<FlightDeal> modifyFlightDealTask =
                this.flightDealService.ModifyFlightDealAsync(someFlightDeal);

            FlightDealDependencyException actualDependencyException =
                await Assert.ThrowsAsync<FlightDealDependencyException>(
                    modifyFlightDealTask.AsTask);

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
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            FlightDeal randomFlightDeal = CreateRandomFlightDeal(randomDateTime);
            FlightDeal someFlightDeal = randomFlightDeal;
            someFlightDeal.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedFlightDealException = new LockedFlightDealException(
                message: "Locked flightDeal record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedFlightDealDependencyException =
                new FlightDealDependencyException(
                    message: "FlightDeal dependency error occurred, contact support.",
                    innerException: lockedFlightDealException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<FlightDeal> modifyFlightDealTask =
                this.flightDealService.ModifyFlightDealAsync(someFlightDeal);

            FlightDealDependencyException actualDependencyException =
             await Assert.ThrowsAsync<FlightDealDependencyException>(
                 modifyFlightDealTask.AsTask);

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
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            FlightDeal randomFlightDeal = CreateRandomFlightDeal(randomDateTime);
            FlightDeal someFlightDeal = randomFlightDeal;
            someFlightDeal.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<FlightDeal> modifyFlightDealTask =
                this.flightDealService.ModifyFlightDealAsync(someFlightDeal);

            FlightDealServiceException actualServiceException =
             await Assert.ThrowsAsync<FlightDealServiceException>(
                 modifyFlightDealTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedFlightDealServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
