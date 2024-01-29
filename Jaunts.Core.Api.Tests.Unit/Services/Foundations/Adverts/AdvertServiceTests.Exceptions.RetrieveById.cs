// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Adverts
{
    public partial class AdvertServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAdvertId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedAdvertStorageException =
               new FailedAdvertStorageException(
                   message: "Failed advert storage error occurred, please contact support.",
                   sqlException);

            var expectedAdvertDependencyException =
                new AdvertDependencyException(
                    message: "Advert dependency error occurred, contact support.",
                    expectedFailedAdvertStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Advert> retrieveByIdAdvertTask =
                this.advertService.RetrieveAdvertByIdAsync(someAdvertId);

            AdvertDependencyException actualDependencyException =
             await Assert.ThrowsAsync<AdvertDependencyException>(
                 retrieveByIdAdvertTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAdvertDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAdvertDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAdvertId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedAdvertStorageException =
              new FailedAdvertStorageException(
                  message: "Failed advert storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedAdvertDependencyException =
                new AdvertDependencyException(
                    message: "Advert dependency error occurred, contact support.",
                    expectedFailedAdvertStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Advert> retrieveByIdAdvertTask =
                this.advertService.RetrieveAdvertByIdAsync(someAdvertId);

            AdvertDependencyException actualDependencyException =
             await Assert.ThrowsAsync<AdvertDependencyException>(
                 retrieveByIdAdvertTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAdvertDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertDependencyException))),
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
            Guid someAdvertId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();


            var lockedAdvertException = new LockedAdvertException(
                message: "Locked advert record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedAdvertDependencyException =
                new AdvertDependencyException(
                    message: "Advert dependency error occurred, contact support.",
                    innerException: lockedAdvertException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Advert> retrieveByIdAdvertTask =
                this.advertService.RetrieveAdvertByIdAsync(someAdvertId);

            AdvertDependencyException actualDependencyException =
             await Assert.ThrowsAsync<AdvertDependencyException>(
                 retrieveByIdAdvertTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAdvertDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAdvertId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedAdvertServiceException =
                 new FailedAdvertServiceException(
                     message: "Failed advert service error occurred, contact support.",
                     innerException: serviceException);

            var expectedAdvertServiceException =
                new AdvertServiceException(
                    message: "Advert service error occurred, contact support.",
                    innerException: failedAdvertServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Advert> retrieveByIdAdvertTask =
                this.advertService.RetrieveAdvertByIdAsync(someAdvertId);

            AdvertServiceException actualServiceException =
                 await Assert.ThrowsAsync<AdvertServiceException>(
                     retrieveByIdAdvertTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedAdvertServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
