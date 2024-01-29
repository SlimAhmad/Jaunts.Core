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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
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
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Provider> retrieveByIdProviderTask =
                this.providerService.RetrieveProviderByIdAsync(someProviderId);

            ProviderDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderDependencyException>(
                 retrieveByIdProviderTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
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
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Provider> retrieveByIdProviderTask =
                this.providerService.RetrieveProviderByIdAsync(someProviderId);

            ProviderDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderDependencyException>(
                 retrieveByIdProviderTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
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
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
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
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Provider> retrieveByIdProviderTask =
                this.providerService.RetrieveProviderByIdAsync(someProviderId);

            ProviderDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderDependencyException>(
                 retrieveByIdProviderTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
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
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Provider> retrieveByIdProviderTask =
                this.providerService.RetrieveProviderByIdAsync(someProviderId);

            ProviderServiceException actualServiceException =
                 await Assert.ThrowsAsync<ProviderServiceException>(
                     retrieveByIdProviderTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProviderServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
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
