// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Drivers
{
    public partial class DriverServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedFailedDriverStorageException =
              new FailedDriverStorageException(
                  message: "Failed driver storage error occurred, please contact support.",
                  sqlException);

            var expectedDriverDependencyException =
                new DriverDependencyException(
                    message: "Driver dependency error occurred, contact support.",
                    expectedFailedDriverStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDrivers())
                    .Throws(sqlException);

            // when
            Action retrieveAllDriversAction = () =>
                this.driverService.RetrieveAllDrivers();

            DriverDependencyException actualDependencyException =
              Assert.Throws<DriverDependencyException>(
                 retrieveAllDriversAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedDriverDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDrivers(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDriverDependencyException))),
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

            var failedDriverServiceException =
              new FailedDriverServiceException(
                  message: "Failed driver service error occurred, contact support.",
                  innerException: serviceException);

            var expectedDriverServiceException =
                new DriverServiceException(
                    message: "Driver service error occurred, contact support.",
                    innerException: failedDriverServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDrivers())
                    .Throws(serviceException);

            // when
            Action retrieveAllDriversAction = () =>
                this.driverService.RetrieveAllDrivers();

            DriverServiceException actualServiceException =
              Assert.Throws<DriverServiceException>(
                 retrieveAllDriversAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedDriverServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDrivers(),
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
