// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Providers
{
    public partial class ProviderServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Provider someProvider = CreateRandomProvider(dateTime);
            someProvider.UpdatedBy = someProvider.CreatedBy;
            var sqlException = GetSqlException();

            var expectedFailedProviderStorageException =
                new FailedProviderStorageException(
                    message: "Failed Provider storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedProviderDependencyException =
                new ProviderDependencyException(
                    message: "Provider dependency error occurred, contact support.",
                    innerException: expectedFailedProviderStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Provider> createProviderTask =
                this.providerService.CreateProviderAsync(someProvider);

            ProviderDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderDependencyException>(
                 createProviderTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProviderDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAsync(It.IsAny<Provider>()),
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
            Provider someProvider = CreateRandomProvider(dateTime);
            someProvider.UpdatedBy = someProvider.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedProviderStorageException =
                new FailedProviderStorageException(
                    message: "Failed Provider storage error occurred, please contact support.",
                    databaseUpdateException);

            var expectedProviderDependencyException =
                new ProviderDependencyException(
                    message: "Provider dependency error occurred, contact support.",
                    expectedFailedProviderStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Provider> createProviderTask =
                this.providerService.CreateProviderAsync(someProvider);

            ProviderDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<ProviderDependencyException>(
                     createProviderTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAsync(It.IsAny<Provider>()),
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
            Provider someProvider = CreateRandomProvider(dateTime);
            someProvider.UpdatedBy = someProvider.CreatedBy;
            var serviceException = new Exception();

            var failedProviderServiceException =
                new FailedProviderServiceException(
                    message: "Failed Provider service error occurred, contact support.",
                    innerException: serviceException);

            var expectedProviderServiceException =
                new ProviderServiceException(
                    message: "Provider service error occurred, contact support.",
                    innerException: failedProviderServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<Provider> createProviderTask =
                 this.providerService.CreateProviderAsync(someProvider);

            ProviderServiceException actualDependencyException =
                 await Assert.ThrowsAsync<ProviderServiceException>(
                     createProviderTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAsync(It.IsAny<Provider>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
