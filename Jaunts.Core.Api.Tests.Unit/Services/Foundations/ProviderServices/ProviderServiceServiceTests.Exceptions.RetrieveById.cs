// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderServices
{
    public partial class ProviderServiceServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProviderServiceId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedProviderServiceStorageException =
               new FailedProviderServiceStorageException(
                   message: "Failed ProviderService storage error occurred, Please contact support.",
                   sqlException);

            var expectedProviderServiceDependencyException =
                new ProviderServiceDependencyException(
                    message: "ProviderService dependency error occurred, contact support.",
                    expectedFailedProviderServiceStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ProviderService> retrieveByIdProviderServiceTask =
                this.providerServiceService.RetrieveProviderServiceByIdAsync(someProviderServiceId);

            ProviderServiceDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderServiceDependencyException>(
                 retrieveByIdProviderServiceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderServiceDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProviderServiceDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProviderServiceId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedProviderServiceStorageException =
              new FailedProviderServiceStorageException(
                  message: "Failed ProviderService storage error occurred, Please contact support.",
                  databaseUpdateException);

            var expectedProviderServiceDependencyException =
                new ProviderServiceDependencyException(
                    message: "ProviderService dependency error occurred, contact support.",
                    expectedFailedProviderServiceStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ProviderService> retrieveByIdProviderServiceTask =
                this.providerServiceService.RetrieveProviderServiceByIdAsync(someProviderServiceId);

            ProviderServiceDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderServiceDependencyException>(
                 retrieveByIdProviderServiceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderServiceDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProviderServiceId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();


            var lockedProviderServiceException = new LockedProviderServiceException(
                message: "Locked ProviderService record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedProviderServiceDependencyException =
                new ProviderServiceDependencyException(
                    message: "ProviderService dependency error occurred, contact support.",
                    innerException: lockedProviderServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ProviderService> retrieveByIdProviderServiceTask =
                this.providerServiceService.RetrieveProviderServiceByIdAsync(someProviderServiceId);

            ProviderServiceDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderServiceDependencyException>(
                 retrieveByIdProviderServiceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderServiceDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProviderServiceId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedProviderServiceServiceException =
                 new FailedProviderServiceServiceException(
                     message: "Failed ProviderService service error occurred, contact support.",
                     innerException: serviceException);

            var expectedProviderServiceServiceException =
                new ProviderServiceServiceException(
                    message: "ProviderService service error occurred, contact support.",
                    innerException: failedProviderServiceServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ProviderService> retrieveByIdProviderServiceTask =
                this.providerServiceService.RetrieveProviderServiceByIdAsync(someProviderServiceId);

            ProviderServiceServiceException actualServiceException =
                 await Assert.ThrowsAsync<ProviderServiceServiceException>(
                     retrieveByIdProviderServiceTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProviderServiceServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
