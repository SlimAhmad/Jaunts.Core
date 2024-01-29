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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Provider someProvider = CreateRandomProvider(randomDateTime);
            someProvider.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

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
            ValueTask<Provider> modifyProviderTask =
                this.providerService.ModifyProviderAsync(someProvider);

                ProviderDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<ProviderDependencyException>(
                     modifyProviderTask.AsTask);

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
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
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
            Provider someProvider = CreateRandomProvider(randomDateTime);
            someProvider.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Provider> modifyProviderTask =
                this.providerService.ModifyProviderAsync(someProvider);

            ProviderDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ProviderDependencyException>(
                    modifyProviderTask.AsTask);

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
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
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
            Provider randomProvider = CreateRandomProvider(randomDateTime);
            Provider someProvider = randomProvider;
            someProvider.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedProviderException = new LockedProviderException(
                message: "Locked Provider record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedProviderDependencyException =
                new ProviderDependencyException(
                    message: "Provider dependency error occurred, contact support.",
                    innerException: lockedProviderException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Provider> modifyProviderTask =
                this.providerService.ModifyProviderAsync(someProvider);

            ProviderDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderDependencyException>(
                 modifyProviderTask.AsTask);

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
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
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
            Provider randomProvider = CreateRandomProvider(randomDateTime);
            Provider someProvider = randomProvider;
            someProvider.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<Provider> modifyProviderTask =
                this.providerService.ModifyProviderAsync(someProvider);

            ProviderServiceException actualServiceException =
             await Assert.ThrowsAsync<ProviderServiceException>(
                 modifyProviderTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProviderServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
