// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectors
{
    public partial class ProvidersDirectorServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProvidersDirectorId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedProvidersDirectorStorageException =
              new FailedProvidersDirectorStorageException(
                  message: "Failed ProvidersDirector storage error occurred, please contact support.",
                  sqlException);

            var expectedProvidersDirectorDependencyException =
                new ProvidersDirectorDependencyException(
                    message: "ProvidersDirector dependency error occurred, contact support.",
                    expectedFailedProvidersDirectorStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorByIdAsync(someProvidersDirectorId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ProvidersDirector> deleteProvidersDirectorTask =
                this.providersDirectorService.RemoveProvidersDirectorByIdAsync(someProvidersDirectorId);

            ProvidersDirectorDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ProvidersDirectorDependencyException>(
                    deleteProvidersDirectorTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(someProvidersDirectorId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProvidersDirectorDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProvidersDirectorId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedProvidersDirectorStorageException =
              new FailedProvidersDirectorStorageException(
                  message: "Failed ProvidersDirector storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedProvidersDirectorDependencyException =
                new ProvidersDirectorDependencyException(
                    message: "ProvidersDirector dependency error occurred, contact support.",
                    expectedFailedProvidersDirectorStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorByIdAsync(someProvidersDirectorId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ProvidersDirector> deleteProvidersDirectorTask =
                this.providersDirectorService.RemoveProvidersDirectorByIdAsync(someProvidersDirectorId);

            ProvidersDirectorDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ProvidersDirectorDependencyException>(
                    deleteProvidersDirectorTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(someProvidersDirectorId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProvidersDirectorId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedProvidersDirectorException = new LockedProvidersDirectorException(
                message: "Locked ProvidersDirector record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedProvidersDirectorDependencyException =
                new ProvidersDirectorDependencyException(
                    message: "ProvidersDirector dependency error occurred, contact support.",
                    innerException: lockedProvidersDirectorException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorByIdAsync(someProvidersDirectorId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ProvidersDirector> deleteProvidersDirectorTask =
                this.providersDirectorService.RemoveProvidersDirectorByIdAsync(someProvidersDirectorId);

            ProvidersDirectorDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ProvidersDirectorDependencyException>(
                    deleteProvidersDirectorTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(someProvidersDirectorId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProvidersDirectorId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedProvidersDirectorServiceException =
             new FailedProvidersDirectorServiceException(
                 message: "Failed ProvidersDirector service error occurred, Please contact support.",
                 innerException: serviceException);

            var expectedProvidersDirectorServiceException =
                new ProvidersDirectorServiceException(
                    message: "ProvidersDirector service error occurred, contact support.",
                    innerException: failedProvidersDirectorServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorByIdAsync(someProvidersDirectorId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ProvidersDirector> deleteProvidersDirectorTask =
                this.providersDirectorService.RemoveProvidersDirectorByIdAsync(someProvidersDirectorId);

            ProvidersDirectorServiceException actualServiceException =
             await Assert.ThrowsAsync<ProvidersDirectorServiceException>(
                 deleteProvidersDirectorTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProvidersDirectorServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(someProvidersDirectorId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
