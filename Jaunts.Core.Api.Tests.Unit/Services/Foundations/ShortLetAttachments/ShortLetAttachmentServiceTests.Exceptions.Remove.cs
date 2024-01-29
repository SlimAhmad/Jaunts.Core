// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLetAttachments
{
    public partial class ShortLetAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someShortLetId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedShortLetAttachmentStorageException =
                new FailedShortLetAttachmentStorageException(
                    message: "Failed ShortLetAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedShortLetAttachmentDependencyException =
                new ShortLetAttachmentDependencyException
                (message: "ShortLetAttachment dependency error occurred, contact support.", 
                innerException: failedShortLetAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ShortLetAttachment> removeShortLetAttachmentTask =
                this.shortLetAttachmentService.RemoveShortLetAttachmentByIdAsync(
                    someShortLetId,
                    someAttachmentId);

            ShortLetAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ShortLetAttachmentDependencyException>(
                removeShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedShortLetAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()),
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
            Guid someShortLetId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedShortLetAttachmentStorageException =
                new FailedShortLetAttachmentStorageException(
                    message: "Failed ShortLetAttachment storage error occurred, Please contact support.",
                    innerException: databaseUpdateException);

            var expectedShortLetAttachmentDependencyException =
                new ShortLetAttachmentDependencyException(
                    message: "ShortLetAttachment dependency error occurred, contact support.",
                    innerException: failedShortLetAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ShortLetAttachment> removeShortLetAttachmentTask =
                this.shortLetAttachmentService.RemoveShortLetAttachmentByIdAsync
                (someShortLetId, someAttachmentId);

            ShortLetAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ShortLetAttachmentDependencyException>(
                removeShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedShortLetAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()),
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
            Guid someShortLetId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedShortLetAttachmentException(
                    message: "Locked ShortLetAttachment record exception, Please try again later.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedShortLetAttachmentException =
                new ShortLetAttachmentDependencyException(
                    message: "ShortLetAttachment dependency error occurred, contact support.",
                    innerException: lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ShortLetAttachment> removeShortLetAttachmentTask =
                this.shortLetAttachmentService.RemoveShortLetAttachmentByIdAsync(someShortLetId, someAttachmentId);

            ShortLetAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ShortLetAttachmentDependencyException>(
                removeShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedShortLetAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()),
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
            var randomShortLetId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someShortLetId = randomShortLetId;
            var serviceException = new Exception();

            var failedShortLetAttachmentServiceException =
                new FailedShortLetAttachmentServiceException(
                    message: "Failed ShortLetAttachment service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedShortLetAttachmentException =
                new ShortLetAttachmentServiceException(
                    message: "ShortLetAttachment service error occurred, contact support.",
                    innerException: failedShortLetAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ShortLetAttachment> removeShortLetAttachmentTask =
                this.shortLetAttachmentService.RemoveShortLetAttachmentByIdAsync(
                    someShortLetId,
                    someAttachmentId);

            ShortLetAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ShortLetAttachmentServiceException>(
                removeShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedShortLetAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}