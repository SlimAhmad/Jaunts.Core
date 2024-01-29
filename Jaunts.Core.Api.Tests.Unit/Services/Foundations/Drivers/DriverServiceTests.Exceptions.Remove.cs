﻿// ---------------------------------------------------------------
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
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someDriverId = Guid.NewGuid();
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
                broker.SelectDriverByIdAsync(someDriverId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Driver> deleteDriverTask =
                this.driverService.RemoveDriverByIdAsync(someDriverId);

            DriverDependencyException actualDependencyException =
                await Assert.ThrowsAsync<DriverDependencyException>(
                    deleteDriverTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedDriverDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(someDriverId),
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
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someDriverId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedDriverStorageException =
              new FailedDriverStorageException(
                  message: "Failed driver storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedDriverDependencyException =
                new DriverDependencyException(
                    message: "Driver dependency error occurred, contact support.",
                    expectedFailedDriverStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverByIdAsync(someDriverId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Driver> deleteDriverTask =
                this.driverService.RemoveDriverByIdAsync(someDriverId);

            DriverDependencyException actualDependencyException =
                await Assert.ThrowsAsync<DriverDependencyException>(
                    deleteDriverTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedDriverDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(someDriverId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someDriverId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedDriverException = new LockedDriverException(
                message: "Locked driver record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedDriverDependencyException =
                new DriverDependencyException(
                    message: "Driver dependency error occurred, contact support.",
                    innerException: lockedDriverException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverByIdAsync(someDriverId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Driver> deleteDriverTask =
                this.driverService.RemoveDriverByIdAsync(someDriverId);

            DriverDependencyException actualDependencyException =
                await Assert.ThrowsAsync<DriverDependencyException>(
                    deleteDriverTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedDriverDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(someDriverId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someDriverId = Guid.NewGuid();
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
                broker.SelectDriverByIdAsync(someDriverId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Driver> deleteDriverTask =
                this.driverService.RemoveDriverByIdAsync(someDriverId);

            DriverServiceException actualServiceException =
             await Assert.ThrowsAsync<DriverServiceException>(
                 deleteDriverTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedDriverServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverByIdAsync(someDriverId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
