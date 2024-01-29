// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Fleets
{
    public partial class FleetServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedFailedFleetStorageException =
              new FailedFleetStorageException(
                  message: "Failed fleet storage error occurred, please contact support.",
                  sqlException);

            var expectedFleetDependencyException =
                new FleetDependencyException(
                    message: "Fleet dependency error occurred, contact support.",
                    expectedFailedFleetStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFleets())
                    .Throws(sqlException);

            // when
            Action retrieveAllFleetsAction = () =>
                this.fleetService.RetrieveAllFleets();

            FleetDependencyException actualDependencyException =
              Assert.Throws<FleetDependencyException>(
                 retrieveAllFleetsAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFleetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFleets(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedFleetDependencyException))),
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

            var failedFleetServiceException =
              new FailedFleetServiceException(
                  message: "Failed fleet service error occurred, contact support.",
                  innerException: serviceException);

            var expectedFleetServiceException =
                new FleetServiceException(
                    message: "Fleet service error occurred, contact support.",
                    innerException: failedFleetServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFleets())
                    .Throws(serviceException);

            // when
            Action retrieveAllFleetsAction = () =>
                this.fleetService.RetrieveAllFleets();

            FleetServiceException actualServiceException =
              Assert.Throws<FleetServiceException>(
                 retrieveAllFleetsAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedFleetServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFleetServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFleets(),
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
