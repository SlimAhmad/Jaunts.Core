// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderService someProviderService = CreateRandomProviderService(dateTime);
            someProviderService.UpdatedBy = someProviderService.CreatedBy;
            var sqlException = GetSqlException();

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
            ValueTask<ProviderService> createProviderServiceTask =
                this.providerServiceService.CreateProviderServiceAsync(someProviderService);

            ProviderServiceDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderServiceDependencyException>(
                 createProviderServiceTask.AsTask);

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
                broker.InsertProviderServiceAsync(It.IsAny<ProviderService>()),
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
            ProviderService someProviderService = CreateRandomProviderService(dateTime);
            someProviderService.UpdatedBy = someProviderService.CreatedBy;
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
            ValueTask<ProviderService> createProviderServiceTask =
                this.providerServiceService.CreateProviderServiceAsync(someProviderService);

            ProviderServiceDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<ProviderServiceDependencyException>(
                     createProviderServiceTask.AsTask);

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
                broker.InsertProviderServiceAsync(It.IsAny<ProviderService>()),
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
            ProviderService someProviderService = CreateRandomProviderService(dateTime);
            someProviderService.UpdatedBy = someProviderService.CreatedBy;
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
            ValueTask<ProviderService> createProviderServiceTask =
                 this.providerServiceService.CreateProviderServiceAsync(someProviderService);

            ProviderServiceServiceException actualDependencyException =
                 await Assert.ThrowsAsync<ProviderServiceServiceException>(
                     createProviderServiceTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderServiceServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderServiceAsync(It.IsAny<ProviderService>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
