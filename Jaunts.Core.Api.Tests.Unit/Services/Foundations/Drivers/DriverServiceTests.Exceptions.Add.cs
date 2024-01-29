// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Drivers
{
    public partial class DriverServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Driver someDriver = CreateRandomDriver(dateTime);
            someDriver.UpdatedBy = someDriver.CreatedBy;
            var sqlException = GetSqlException();

            var expectedFailedDriverStorageException =
                new FailedDriverStorageException(
                    message: "Failed driver storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDriverDependencyException =
                new DriverDependencyException(
                    message: "Driver dependency error occurred, contact support.",
                    innerException: expectedFailedDriverStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Driver> createDriverTask =
                this.driverService.CreateDriverAsync(someDriver);

            DriverDependencyException actualDependencyException =
             await Assert.ThrowsAsync<DriverDependencyException>(
                 createDriverTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedDriverDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDriverDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAsync(It.IsAny<Driver>()),
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
            Driver someDriver = CreateRandomDriver(dateTime);
            someDriver.UpdatedBy = someDriver.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedDriverStorageException =
                new FailedDriverStorageException(
                    message: "Failed driver storage error occurred, please contact support.",
                    databaseUpdateException);

            var expectedDriverDependencyException =
                new DriverDependencyException(
                    message: "Driver dependency error occurred, contact support.",
                    expectedFailedDriverStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Driver> createDriverTask =
                this.driverService.CreateDriverAsync(someDriver);

            DriverDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<DriverDependencyException>(
                     createDriverTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedDriverDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAsync(It.IsAny<Driver>()),
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
            Driver someDriver = CreateRandomDriver(dateTime);
            someDriver.UpdatedBy = someDriver.CreatedBy;
            var serviceException = new Exception();

            var failedDriverServiceException =
                new FailedDriverServiceException(
                    message: "Failed driver service error occurred, contact support.",
                    innerException: serviceException);

            var expectedDriverServiceException =
                new DriverServiceException(
                    message: "Driver service error occurred, contact support.",
                    innerException: failedDriverServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<Driver> createDriverTask =
                 this.driverService.CreateDriverAsync(someDriver);

            DriverServiceException actualDependencyException =
                 await Assert.ThrowsAsync<DriverServiceException>(
                     createDriverTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedDriverServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAsync(It.IsAny<Driver>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
