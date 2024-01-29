// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.AdvertAttachments
{
    public partial class AdvertAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someAdvertId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedAdvertAttachmentStorageException =
                new FailedAdvertAttachmentStorageException(
                    message: "Failed AdvertAttachment storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedAdvertAttachmentDependencyException =
                new AdvertAttachmentDependencyException
                (message: "AdvertAttachment dependency error occurred, contact support.", 
                innerException: failedAdvertAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectAdvertAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<AdvertAttachment> removeAdvertAttachmentTask =
                this.AdvertAttachmentService.RemoveAdvertAttachmentByIdAsync(
                    someAdvertId,
                    someAttachmentId);

            AdvertAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<AdvertAttachmentDependencyException>(
                removeAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAdvertAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someAdvertId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedAdvertAttachmentStorageException =
                new FailedAdvertAttachmentStorageException(
                    message: "Failed AdvertAttachment storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedAdvertAttachmentDependencyException =
                new AdvertAttachmentDependencyException(
                    message: "AdvertAttachment dependency error occurred, contact support.",
                    innerException: failedAdvertAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<AdvertAttachment> removeAdvertAttachmentTask =
                this.AdvertAttachmentService.RemoveAdvertAttachmentByIdAsync
                (someAdvertId, someAttachmentId);

            AdvertAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<AdvertAttachmentDependencyException>(
                removeAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAdvertAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someAdvertId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedAdvertAttachmentException(
                    message: "Locked AdvertAttachment record exception, please try again later.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedAdvertAttachmentException =
                new AdvertAttachmentDependencyException(
                    message: "AdvertAttachment dependency error occurred, contact support.",
                    innerException: lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<AdvertAttachment> removeAdvertAttachmentTask =
                this.AdvertAttachmentService.RemoveAdvertAttachmentByIdAsync(someAdvertId, someAttachmentId);

            AdvertAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<AdvertAttachmentDependencyException>(
                removeAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAdvertAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            var randomAttachmentId = Guid.NewGuid();
            var randomAdvertId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someAdvertId = randomAdvertId;
            var serviceException = new Exception();

            var failedAdvertAttachmentServiceException =
                new FailedAdvertAttachmentServiceException(
                    message: "Failed AdvertAttachment service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAdvertAttachmentException =
                new AdvertAttachmentServiceException(
                    message: "AdvertAttachment service error occurred, contact support.",
                    innerException: failedAdvertAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<AdvertAttachment> removeAdvertAttachmentTask =
                this.AdvertAttachmentService.RemoveAdvertAttachmentByIdAsync(
                    someAdvertId,
                    someAttachmentId);

            AdvertAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<AdvertAttachmentServiceException>(
                removeAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAdvertAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}