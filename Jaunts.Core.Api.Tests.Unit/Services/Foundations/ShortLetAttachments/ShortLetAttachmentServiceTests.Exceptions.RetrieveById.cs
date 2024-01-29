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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
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
                new ShortLetAttachmentDependencyException(
                    message: "ShortLetAttachment dependency error occurred, contact support.",
                    innerException: failedShortLetAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ShortLetAttachment> retrieveShortLetAttachmentTask =
                this.shortLetAttachmentService.RetrieveShortLetAttachmentByIdAsync(someShortLetId, someAttachmentId);

            ShortLetAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ShortLetAttachmentDependencyException>(
                retrieveShortLetAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
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
            ValueTask<ShortLetAttachment> retrieveShortLetAttachmentTask =
                this.shortLetAttachmentService.RetrieveShortLetAttachmentByIdAsync(someShortLetId, someAttachmentId);

            ShortLetAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ShortLetAttachmentDependencyException>(
                retrieveShortLetAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
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
                    lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ShortLetAttachment> retrieveShortLetAttachmentTask =
                this.shortLetAttachmentService.RetrieveShortLetAttachmentByIdAsync(someShortLetId, someAttachmentId);

            ShortLetAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ShortLetAttachmentDependencyException>(
                retrieveShortLetAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someShortLetId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedShortLetAttachmentServiceException =
                 new FailedShortLetAttachmentServiceException(
                     message: "Failed ShortLetAttachment service error occurred, Please contact support.",
                     innerException: serviceException);

            var expectedShortLetAttachmentServiceException =
                new ShortLetAttachmentServiceException(
                    message: "ShortLetAttachment service error occurred, contact support.",
                    innerException: failedShortLetAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ShortLetAttachment> retrieveShortLetAttachmentTask =
                this.shortLetAttachmentService.RetrieveShortLetAttachmentByIdAsync(someShortLetId, someAttachmentId);

            ShortLetAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ShortLetAttachmentServiceException>(
                retrieveShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedShortLetAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
