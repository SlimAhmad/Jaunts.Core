// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOfferAttachments
{
    public partial class PromosOfferAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid somePromosOfferId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedPromosOfferAttachmentStorageException =
             new FailedPromosOfferAttachmentStorageException(
                 message: "Failed PromosOfferAttachment storage error occurred, please contact support.",
                 innerException: sqlException);

            var expectedPromosOfferAttachmentDependencyException =
                new PromosOfferAttachmentDependencyException(
                    message: "PromosOfferAttachment dependency error occurred, contact support.",
                    innerException: failedPromosOfferAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectPromosOfferAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<PromosOfferAttachment> retrievePromosOfferAttachmentTask =
                this.promosOfferAttachmentService.RetrievePromosOfferAttachmentByIdAsync(somePromosOfferId, someAttachmentId);

            PromosOfferAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<PromosOfferAttachmentDependencyException>(
                retrievePromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid somePromosOfferId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedPromosOfferAttachmentStorageException =
             new FailedPromosOfferAttachmentStorageException(
                 message: "Failed PromosOfferAttachment storage error occurred, please contact support.",
                 innerException: databaseUpdateException);

            var expectedPromosOfferAttachmentDependencyException =
                new PromosOfferAttachmentDependencyException(
                    message: "PromosOfferAttachment dependency error occurred, contact support.",
                    innerException: failedPromosOfferAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<PromosOfferAttachment> retrievePromosOfferAttachmentTask =
                this.promosOfferAttachmentService.RetrievePromosOfferAttachmentByIdAsync(somePromosOfferId, someAttachmentId);

            PromosOfferAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<PromosOfferAttachmentDependencyException>(
                retrievePromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid somePromosOfferId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedPromosOfferAttachmentException(
                    message: "Locked PromosOfferAttachment record exception, please try again later.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedPromosOfferAttachmentException =
                new PromosOfferAttachmentDependencyException(
                    message: "PromosOfferAttachment dependency error occurred, contact support.",
                    lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<PromosOfferAttachment> retrievePromosOfferAttachmentTask =
                this.promosOfferAttachmentService.RetrievePromosOfferAttachmentByIdAsync(somePromosOfferId, someAttachmentId);

            PromosOfferAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<PromosOfferAttachmentDependencyException>(
                retrievePromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid somePromosOfferId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedPromosOfferAttachmentServiceException =
                 new FailedPromosOfferAttachmentServiceException(
                     message: "Failed PromosOfferAttachment service error occurred, please contact support.",
                     innerException: serviceException);

            var expectedPromosOfferAttachmentServiceException =
                new PromosOfferAttachmentServiceException(
                    message: "PromosOfferAttachment service error occurred, contact support.",
                    innerException: failedPromosOfferAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<PromosOfferAttachment> retrievePromosOfferAttachmentTask =
                this.promosOfferAttachmentService.RetrievePromosOfferAttachmentByIdAsync(somePromosOfferId, someAttachmentId);

            PromosOfferAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<PromosOfferAttachmentServiceException>(
                retrievePromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
