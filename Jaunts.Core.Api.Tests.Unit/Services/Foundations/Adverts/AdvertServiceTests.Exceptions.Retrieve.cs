// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Adverts
{
    public partial class AdvertServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllAdverts())
                    .Throws(sqlException);

            // when
            Action retrieveAllAdvertsAction = () =>
                this.advertService.RetrieveAllAdverts();

            AdvertDependencyException actualDependencyException =
              Assert.Throws<AdvertDependencyException>(
                 retrieveAllAdvertsAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAdvertDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAdverts(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllAdverts())
                    .Throws(serviceException);

            // when
            Action retrieveAllAdvertsAction = () =>
                this.advertService.RetrieveAllAdverts();

            AdvertServiceException actualServiceException =
              Assert.Throws<AdvertServiceException>(
                 retrieveAllAdvertsAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedAdvertServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAdverts(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
