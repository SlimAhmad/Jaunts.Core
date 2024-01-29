// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions;
using Microsoft.Data.SqlClient;
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
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProviderId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedProviderStorageException =
              new FailedProviderStorageException(
                  message: "Failed Provider storage error occurred, please contact support.",
                  sqlException);

            var expectedProviderDependencyException =
                new ProviderDependencyException(
                    message: "Provider dependency error occurred, contact support.",
                    expectedFailedProviderStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderByIdAsync(someProviderId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Provider> deleteProviderTask =
                this.providerService.RemoveProviderByIdAsync(someProviderId);

            ProviderDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ProviderDependencyException>(
                    deleteProviderTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(someProviderId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProviderDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProviderId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedProviderStorageException =
              new FailedProviderStorageException(
                  message: "Failed Provider storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedProviderDependencyException =
                new ProviderDependencyException(
                    message: "Provider dependency error occurred, contact support.",
                    expectedFailedProviderStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderByIdAsync(someProviderId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Provider> deleteProviderTask =
                this.providerService.RemoveProviderByIdAsync(someProviderId);

            ProviderDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ProviderDependencyException>(
                    deleteProviderTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(someProviderId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProviderId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedProviderException = new LockedProviderException(
                message: "Locked Provider record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedProviderDependencyException =
                new ProviderDependencyException(
                    message: "Provider dependency error occurred, contact support.",
                    innerException: lockedProviderException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderByIdAsync(someProviderId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Provider> deleteProviderTask =
                this.providerService.RemoveProviderByIdAsync(someProviderId);

            ProviderDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ProviderDependencyException>(
                    deleteProviderTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(someProviderId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProviderId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedProviderServiceException =
             new FailedProviderServiceException(
                 message: "Failed Provider service error occurred, contact support.",
                 innerException: serviceException);

            var expectedProviderServiceException =
                new ProviderServiceException(
                    message: "Provider service error occurred, contact support.",
                    innerException: failedProviderServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderByIdAsync(someProviderId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Provider> deleteProviderTask =
                this.providerService.RemoveProviderByIdAsync(someProviderId);

            ProviderServiceException actualServiceException =
             await Assert.ThrowsAsync<ProviderServiceException>(
                 deleteProviderTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProviderServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(someProviderId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
