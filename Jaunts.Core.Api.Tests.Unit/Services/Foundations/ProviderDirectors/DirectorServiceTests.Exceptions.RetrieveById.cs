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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
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
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ProvidersDirector> retrieveByIdProvidersDirectorTask =
                this.providersDirectorService.RetrieveProvidersDirectorByIdAsync(someProvidersDirectorId);

            ProvidersDirectorDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProvidersDirectorDependencyException>(
                 retrieveByIdProvidersDirectorTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
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
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ProvidersDirector> retrieveByIdProvidersDirectorTask =
                this.providersDirectorService.RetrieveProvidersDirectorByIdAsync(someProvidersDirectorId);

            ProvidersDirectorDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProvidersDirectorDependencyException>(
                 retrieveByIdProvidersDirectorTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
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
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
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
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ProvidersDirector> retrieveByIdProvidersDirectorTask =
                this.providersDirectorService.RetrieveProvidersDirectorByIdAsync(someProvidersDirectorId);

            ProvidersDirectorDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProvidersDirectorDependencyException>(
                 retrieveByIdProvidersDirectorTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
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
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ProvidersDirector> retrieveByIdProvidersDirectorTask =
                this.providersDirectorService.RetrieveProvidersDirectorByIdAsync(someProvidersDirectorId);

            ProvidersDirectorServiceException actualServiceException =
                 await Assert.ThrowsAsync<ProvidersDirectorServiceException>(
                     retrieveByIdProvidersDirectorTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProvidersDirectorServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
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
