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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ProviderService someProviderService = CreateRandomProviderService(randomDateTime);
            someProviderService.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedFailedProviderServiceStorageException =
              new FailedProviderServiceStorageException(
                  message: "Failed ProviderService storage error occurred, Please contact support.",
                  innerException: sqlException);

            var expectedProviderServiceDependencyException =
                new ProviderServiceDependencyException(
                    message: "ProviderService dependency error occurred, contact support.",
                    innerException: expectedFailedProviderServiceStorageException);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<ProviderService> modifyProviderServiceTask =
                this.providerServiceService.ModifyProviderServiceAsync(someProviderService);

                ProviderServiceDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<ProviderServiceDependencyException>(
                     modifyProviderServiceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderServiceDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProviderServiceDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
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
            ProviderService someProviderService = CreateRandomProviderService(randomDateTime);
            someProviderService.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedProviderServiceStorageException =
              new FailedProviderServiceStorageException(
                  message: "Failed ProviderService storage error occurred, Please contact support.",
                  databaseUpdateException);

            var expectedProviderServiceDependencyException =
                new ProviderServiceDependencyException(
                    message: "ProviderService dependency error occurred, contact support.",
                    expectedFailedProviderServiceStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<ProviderService> modifyProviderServiceTask =
                this.providerServiceService.ModifyProviderServiceAsync(someProviderService);

            ProviderServiceDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ProviderServiceDependencyException>(
                    modifyProviderServiceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderServiceDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
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
            ProviderService randomProviderService = CreateRandomProviderService(randomDateTime);
            ProviderService someProviderService = randomProviderService;
            someProviderService.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedProviderServiceException = new LockedProviderServiceException(
                message: "Locked ProviderService record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedProviderServiceDependencyException =
                new ProviderServiceDependencyException(
                    message: "ProviderService dependency error occurred, contact support.",
                    innerException: lockedProviderServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<ProviderService> modifyProviderServiceTask =
                this.providerServiceService.ModifyProviderServiceAsync(someProviderService);

            ProviderServiceDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderServiceDependencyException>(
                 modifyProviderServiceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderServiceDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
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
            ProviderService randomProviderService = CreateRandomProviderService(randomDateTime);
            ProviderService someProviderService = randomProviderService;
            someProviderService.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var failedProviderServiceServiceException =
             new FailedProviderServiceServiceException(
                 message: "Failed ProviderService service error occurred, contact support.",
                 innerException: serviceException);

            var expectedProviderServiceServiceException =
                new ProviderServiceServiceException(
                    message: "ProviderService service error occurred, contact support.",
                    innerException: failedProviderServiceServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<ProviderService> modifyProviderServiceTask =
                this.providerServiceService.ModifyProviderServiceAsync(someProviderService);

            ProviderServiceServiceException actualServiceException =
             await Assert.ThrowsAsync<ProviderServiceServiceException>(
                 modifyProviderServiceTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProviderServiceServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
