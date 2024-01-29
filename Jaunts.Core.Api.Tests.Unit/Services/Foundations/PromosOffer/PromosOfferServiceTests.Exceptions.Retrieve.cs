// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOffers
{
    public partial class PromosOfferServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedFailedPromosOffersStorageException =
              new FailedPromosOffersStorageException(
                  message: "Failed PromosOffer storage error occurred, please contact support.",
                  sqlException);

            var expectedPromosOffersDependencyException =
                new PromosOffersDependencyException(
                    message: "PromosOffer dependency error occurred, contact support.",
                    expectedFailedPromosOffersStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPromosOffers())
                    .Throws(sqlException);

            // when
            Action retrieveAllPromosOffersAction = () =>
                this.promosOfferService.RetrieveAllPromosOffers();

            PromosOffersDependencyException actualDependencyException =
              Assert.Throws<PromosOffersDependencyException>(
                 retrieveAllPromosOffersAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPromosOffersDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPromosOffers(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPromosOffersDependencyException))),
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

            var failedPromosOfferServiceException =
              new FailedPromosOffersServiceException(
                  message: "Failed PromosOffer service error occurred, contact support.",
                  innerException: serviceException);

            var expectedPromosOfferServiceException =
                new PromosOffersServiceException(
                    message: "PromosOffer service error occurred, contact support.",
                    innerException: failedPromosOfferServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPromosOffers())
                    .Throws(serviceException);

            // when
            Action retrieveAllPromosOffersAction = () =>
                this.promosOfferService.RetrieveAllPromosOffers();

            PromosOffersServiceException actualServiceException =
              Assert.Throws<PromosOffersServiceException>(
                 retrieveAllPromosOffersAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedPromosOfferServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPromosOffers(),
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
