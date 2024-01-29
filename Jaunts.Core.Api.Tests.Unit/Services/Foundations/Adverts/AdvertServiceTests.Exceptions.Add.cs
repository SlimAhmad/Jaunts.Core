// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Advert someAdvert = CreateRandomAdvert(dateTime);
            someAdvert.UpdatedBy = someAdvert.CreatedBy;
            var sqlException = GetSqlException();

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
            ValueTask<Advert> createAdvertTask =
                this.advertService.CreateAdvertAsync(someAdvert);

            AdvertDependencyException actualDependencyException =
             await Assert.ThrowsAsync<AdvertDependencyException>(
                 createAdvertTask.AsTask);

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
                broker.InsertAdvertAsync(It.IsAny<Advert>()),
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
            Advert someAdvert = CreateRandomAdvert(dateTime);
            someAdvert.UpdatedBy = someAdvert.CreatedBy;
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
            ValueTask<Advert> createAdvertTask =
                this.advertService.CreateAdvertAsync(someAdvert);

            AdvertDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<AdvertDependencyException>(
                     createAdvertTask.AsTask);

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
                broker.InsertAdvertAsync(It.IsAny<Advert>()),
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
            Advert someAdvert = CreateRandomAdvert(dateTime);
            someAdvert.UpdatedBy = someAdvert.CreatedBy;
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
            ValueTask<Advert> createAdvertTask =
                 this.advertService.CreateAdvertAsync(someAdvert);

            AdvertServiceException actualDependencyException =
                 await Assert.ThrowsAsync<AdvertServiceException>(
                     createAdvertTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAdvertServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAsync(It.IsAny<Advert>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
