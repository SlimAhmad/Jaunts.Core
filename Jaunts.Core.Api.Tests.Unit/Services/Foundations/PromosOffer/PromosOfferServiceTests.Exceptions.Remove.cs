// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOffers
{
    public partial class PromosOfferServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid somePromosOfferId = Guid.NewGuid();
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
                broker.SelectPromosOffersByIdAsync(somePromosOfferId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<PromosOffer> deletePromosOfferTask =
                this.promosOfferService.RemovePromosOfferByIdAsync(somePromosOfferId);

            PromosOffersDependencyException actualDependencyException =
                await Assert.ThrowsAsync<PromosOffersDependencyException>(
                    deletePromosOfferTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPromosOffersDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(somePromosOfferId),
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
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid somePromosOfferId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedPromosOffersStorageException =
              new FailedPromosOffersStorageException(
                  message: "Failed PromosOffer storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedPromosOffersDependencyException =
                new PromosOffersDependencyException(
                    message: "PromosOffer dependency error occurred, contact support.",
                    expectedFailedPromosOffersStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOffersByIdAsync(somePromosOfferId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<PromosOffer> deletePromosOfferTask =
                this.promosOfferService.RemovePromosOfferByIdAsync(somePromosOfferId);

            PromosOffersDependencyException actualDependencyException =
                await Assert.ThrowsAsync<PromosOffersDependencyException>(
                    deletePromosOfferTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPromosOffersDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(somePromosOfferId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid somePromosOfferId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedPromosOfferException = new LockedPromosOffersException(
                message: "Locked PromosOffer record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedPromosOffersDependencyException =
                new PromosOffersDependencyException(
                    message: "PromosOffer dependency error occurred, contact support.",
                    innerException: lockedPromosOfferException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOffersByIdAsync(somePromosOfferId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<PromosOffer> deletePromosOfferTask =
                this.promosOfferService.RemovePromosOfferByIdAsync(somePromosOfferId);

            PromosOffersDependencyException actualDependencyException =
                await Assert.ThrowsAsync<PromosOffersDependencyException>(
                    deletePromosOfferTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPromosOffersDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(somePromosOfferId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid somePromosOfferId = Guid.NewGuid();
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
                broker.SelectPromosOffersByIdAsync(somePromosOfferId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<PromosOffer> deletePromosOfferTask =
                this.promosOfferService.RemovePromosOfferByIdAsync(somePromosOfferId);

            PromosOffersServiceException actualServiceException =
             await Assert.ThrowsAsync<PromosOffersServiceException>(
                 deletePromosOfferTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedPromosOfferServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(somePromosOfferId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
