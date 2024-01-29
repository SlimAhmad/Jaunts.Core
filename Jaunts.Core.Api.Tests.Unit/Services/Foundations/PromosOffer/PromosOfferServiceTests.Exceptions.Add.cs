// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            PromosOffer somePromosOffer = CreateRandomPromosOffer(dateTime);
            somePromosOffer.UpdatedBy = somePromosOffer.CreatedBy;
            var sqlException = GetSqlException();

            var expectedFailedPromosOfferStorageException =
                new FailedPromosOffersStorageException(
                    message: "Failed PromosOffer storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedPromosOfferDependencyException =
                new PromosOffersDependencyException(
                    message: "PromosOffer dependency error occurred, contact support.",
                    innerException: expectedFailedPromosOfferStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<PromosOffer> createPromosOfferTask =
                this.promosOfferService.CreatePromosOfferAsync(somePromosOffer);

            PromosOffersDependencyException actualDependencyException =
             await Assert.ThrowsAsync<PromosOffersDependencyException>(
                 createPromosOfferTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPromosOfferDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPromosOfferDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAsync(It.IsAny<PromosOffer>()),
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
            PromosOffer somePromosOffer = CreateRandomPromosOffer(dateTime);
            somePromosOffer.UpdatedBy = somePromosOffer.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedPromosOfferStorageException =
                new FailedPromosOffersStorageException(
                    message: "Failed PromosOffer storage error occurred, please contact support.",
                    databaseUpdateException);

            var expectedPromosOfferDependencyException =
                new PromosOffersDependencyException(
                    message: "PromosOffer dependency error occurred, contact support.",
                    expectedFailedPromosOfferStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<PromosOffer> createPromosOfferTask =
                this.promosOfferService.CreatePromosOfferAsync(somePromosOffer);

            PromosOffersDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<PromosOffersDependencyException>(
                     createPromosOfferTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPromosOfferDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAsync(It.IsAny<PromosOffer>()),
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
            PromosOffer somePromosOffer = CreateRandomPromosOffer(dateTime);
            somePromosOffer.UpdatedBy = somePromosOffer.CreatedBy;
            var serviceException = new Exception();

            var failedPromosOfferServiceException =
                new FailedPromosOffersServiceException(
                    message: "Failed PromosOffer service error occurred, contact support.",
                    innerException: serviceException);

            var expectedPromosOfferServiceException =
                new PromosOffersServiceException(
                    message: "PromosOffer service error occurred, contact support.",
                    innerException: failedPromosOfferServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<PromosOffer> createPromosOfferTask =
                 this.promosOfferService.CreatePromosOfferAsync(somePromosOffer);

            PromosOffersServiceException actualDependencyException =
                 await Assert.ThrowsAsync<PromosOffersServiceException>(
                     createPromosOfferTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPromosOfferServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAsync(It.IsAny<PromosOffer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
