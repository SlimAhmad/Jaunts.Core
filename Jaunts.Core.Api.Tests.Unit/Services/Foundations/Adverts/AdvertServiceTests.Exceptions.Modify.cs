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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Advert someAdvert = CreateRandomAdvert(randomDateTime);
            someAdvert.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedFailedAdvertStorageException =
              new FailedAdvertStorageException(
                  message: "Failed advert storage error occurred, please contact support.",
                  innerException: sqlException);

            var expectedAdvertDependencyException =
                new AdvertDependencyException(
                    message: "Advert dependency error occurred, contact support.",
                    innerException: expectedFailedAdvertStorageException);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Advert> modifyAdvertTask =
                this.advertService.ModifyAdvertAsync(someAdvert);

                AdvertDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<AdvertDependencyException>(
                     modifyAdvertTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAdvertDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAdvertDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
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
            Advert someAdvert = CreateRandomAdvert(randomDateTime);
            someAdvert.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedAdvertStorageException =
              new FailedAdvertStorageException(
                  message: "Failed advert storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedAdvertDependencyException =
                new AdvertDependencyException(
                    message: "Advert dependency error occurred, contact support.",
                    expectedFailedAdvertStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Advert> modifyAdvertTask =
                this.advertService.ModifyAdvertAsync(someAdvert);

            AdvertDependencyException actualDependencyException =
                await Assert.ThrowsAsync<AdvertDependencyException>(
                    modifyAdvertTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAdvertDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
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
            Advert randomAdvert = CreateRandomAdvert(randomDateTime);
            Advert someAdvert = randomAdvert;
            someAdvert.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAdvertException = new LockedAdvertException(
                message: "Locked advert record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedAdvertDependencyException =
                new AdvertDependencyException(
                    message: "Advert dependency error occurred, contact support.",
                    innerException: lockedAdvertException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Advert> modifyAdvertTask =
                this.advertService.ModifyAdvertAsync(someAdvert);

            AdvertDependencyException actualDependencyException =
             await Assert.ThrowsAsync<AdvertDependencyException>(
                 modifyAdvertTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAdvertDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
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
            Advert randomAdvert = CreateRandomAdvert(randomDateTime);
            Advert someAdvert = randomAdvert;
            someAdvert.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var failedAdvertServiceException =
             new FailedAdvertServiceException(
                 message: "Failed advert service error occurred, contact support.",
                 innerException: serviceException);

            var expectedAdvertServiceException =
                new AdvertServiceException(
                    message: "Advert service error occurred, contact support.",
                    innerException: failedAdvertServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<Advert> modifyAdvertTask =
                this.advertService.ModifyAdvertAsync(someAdvert);

            AdvertServiceException actualServiceException =
             await Assert.ThrowsAsync<AdvertServiceException>(
                 modifyAdvertTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedAdvertServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
