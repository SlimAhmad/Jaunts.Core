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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            PromosOffer somePromosOffer = CreateRandomPromosOffer(randomDateTime);
            somePromosOffer.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedFailedPromosOffersStorageException =
              new FailedPromosOffersStorageException(
                  message: "Failed PromosOffer storage error occurred, please contact support.",
                  innerException: sqlException);

            var expectedPromosOffersDependencyException =
                new PromosOffersDependencyException(
                    message: "PromosOffer dependency error occurred, contact support.",
                    innerException: expectedFailedPromosOffersStorageException);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<PromosOffer> modifyPromosOfferTask =
                this.promosOfferService.ModifyPromosOfferAsync(somePromosOffer);

                PromosOffersDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<PromosOffersDependencyException>(
                     modifyPromosOfferTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPromosOffersDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPromosOffersDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(It.IsAny<Guid>()),
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
            PromosOffer somePromosOffer = CreateRandomPromosOffer(randomDateTime);
            somePromosOffer.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedPromosOffersStorageException =
              new FailedPromosOffersStorageException(
                  message: "Failed PromosOffer storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedPromosOffersDependencyException =
                new PromosOffersDependencyException(
                    message: "PromosOffer dependency error occurred, contact support.",
                    expectedFailedPromosOffersStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<PromosOffer> modifyPromosOfferTask =
                this.promosOfferService.ModifyPromosOfferAsync(somePromosOffer);

            PromosOffersDependencyException actualDependencyException =
                await Assert.ThrowsAsync<PromosOffersDependencyException>(
                    modifyPromosOfferTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPromosOffersDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(It.IsAny<Guid>()),
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
            PromosOffer randomPromosOffer = CreateRandomPromosOffer(randomDateTime);
            PromosOffer somePromosOffer = randomPromosOffer;
            somePromosOffer.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedPromosOfferException = new LockedPromosOffersException(
                message: "Locked PromosOffer record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedPromosOffersDependencyException =
                new PromosOffersDependencyException(
                    message: "PromosOffer dependency error occurred, contact support.",
                    innerException: lockedPromosOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<PromosOffer> modifyPromosOfferTask =
                this.promosOfferService.ModifyPromosOfferAsync(somePromosOffer);

            PromosOffersDependencyException actualDependencyException =
             await Assert.ThrowsAsync<PromosOffersDependencyException>(
                 modifyPromosOfferTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedPromosOffersDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOffersDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(It.IsAny<Guid>()),
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
            PromosOffer randomPromosOffer = CreateRandomPromosOffer(randomDateTime);
            PromosOffer somePromosOffer = randomPromosOffer;
            somePromosOffer.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<PromosOffer> modifyPromosOfferTask =
                this.promosOfferService.ModifyPromosOfferAsync(somePromosOffer);

            PromosOffersServiceException actualServiceException =
             await Assert.ThrowsAsync<PromosOffersServiceException>(
                 modifyPromosOfferTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedPromosOfferServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOffersByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
