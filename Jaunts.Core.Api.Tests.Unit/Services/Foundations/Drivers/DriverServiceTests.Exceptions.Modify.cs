// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions;
using Microsoft.Data.SqlClient;
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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Driver someDriver = CreateRandomDriver(randomDateTime);
            someDriver.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

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
            ValueTask<Driver> modifyDriverTask =
                this.driverService.ModifyDriverAsync(someDriver);

                DriverDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<DriverDependencyException>(
                     modifyDriverTask.AsTask);

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
                broker.SelectDriverByIdAsync(It.IsAny<Guid>()),
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
            Driver someDriver = CreateRandomDriver(randomDateTime);
            someDriver.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Driver> modifyDriverTask =
                this.driverService.ModifyDriverAsync(someDriver);

            DriverDependencyException actualDependencyException =
                await Assert.ThrowsAsync<DriverDependencyException>(
                    modifyDriverTask.AsTask);

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
                broker.SelectDriverByIdAsync(It.IsAny<Guid>()),
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
            Driver randomDriver = CreateRandomDriver(randomDateTime);
            Driver someDriver = randomDriver;
            someDriver.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedDriverException = new LockedDriverException(
                message: "Locked driver record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedDriverDependencyException =
                new DriverDependencyException(
                    message: "Driver dependency error occurred, contact support.",
                    innerException: lockedDriverException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Driver> modifyDriverTask =
                this.driverService.ModifyDriverAsync(someDriver);

            DriverDependencyException actualDependencyException =
             await Assert.ThrowsAsync<DriverDependencyException>(
                 modifyDriverTask.AsTask);

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
                broker.SelectDriverByIdAsync(It.IsAny<Guid>()),
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
            Driver randomDriver = CreateRandomDriver(randomDateTime);
            Driver someDriver = randomDriver;
            someDriver.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Driver> modifyDriverTask =
                this.driverService.ModifyDriverAsync(someDriver);

            DriverServiceException actualServiceException =
             await Assert.ThrowsAsync<DriverServiceException>(
                 modifyDriverTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedDriverServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
