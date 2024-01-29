// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedAttachmentStorageException =
             new FailedAttachmentStorageException(
                 message: "Failed Attachment storage error occurred, contact support.",
                 innerException: sqlException);

            var expectedAttachmentServiceException =
                new AttachmentDependencyException(
                    message: "Attachment dependency error occurred, contact support.",
                    innerException: failedAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Attachment> deleteAttachmentTask =
                this.attachmentService.RemoveAttachmentByIdAsync(someAttachmentId);

            AttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<AttachmentDependencyException>(
                     deleteAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAttachmentServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAttachmentServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedAttachmentStorageException =
                new FailedAttachmentStorageException(
                    message: "Failed Attachment storage error occurred, contact support.",
                    innerException: databaseUpdateException);

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(
                    message: "Attachment dependency error occurred, contact support.",
                    innerException: failedAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Attachment> deleteAttachmentTask =
                this.attachmentService.RemoveAttachmentByIdAsync(someAttachmentId);

            AttachmentDependencyException actualAttachmentDependencyException =
             await Assert.ThrowsAsync<AttachmentDependencyException>(
                 deleteAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAttachmentDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var failedLockedAttachmentException =
                new LockedAttachmentException(
                    message: "Locked attachment record exception, please try again later.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedAttachmentServiceException =
                new AttachmentDependencyException(
                    message: "Attachment dependency error occurred, contact support.",
                    innerException: failedLockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Attachment> deleteAttachmentTask =
                this.attachmentService.RemoveAttachmentByIdAsync(someAttachmentId);

            AttachmentDependencyException actualAttachmentServiceException =
                 await Assert.ThrowsAsync<AttachmentDependencyException>(
                     deleteAttachmentTask.AsTask);

            // then
            actualAttachmentServiceException.Should().BeEquivalentTo(
                expectedAttachmentServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedAttachmentServiceException =
                new FailedAttachmentServiceException(
                    message: "Failed Attachment Service Exception occurred,contact support.",
                    innerException: serviceException);

            var expectedAttachmentServiceException =
                new AttachmentServiceException(
                    message: "Attachment Service error occurred, contact support.",
                    innerException: failedAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Attachment> deleteAttachmentTask =
                this.attachmentService.RemoveAttachmentByIdAsync(someAttachmentId);

            AttachmentServiceException actualAttachmentServiceException =
                 await Assert.ThrowsAsync<AttachmentServiceException>(
                     deleteAttachmentTask.AsTask);

            // then
            actualAttachmentServiceException.Should().BeEquivalentTo(
                expectedAttachmentServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
