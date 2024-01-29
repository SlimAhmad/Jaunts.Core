// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var someAttachmentId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var failedAttachmentStorageException =
                new FailedAttachmentStorageException(
                    message: "Failed Attachment storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(
                    message: "Attachment dependency error occurred, contact support.",
                    innerException: failedAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()))
                    .Throws(sqlException);

            // when 
            ValueTask<Attachment> retrieveTask =
                this.attachmentService.RetrieveAttachmentByIdAsync(someAttachmentId);

            AttachmentDependencyException actualAttachmentServiceException =
               await Assert.ThrowsAsync<AttachmentDependencyException>(
                   retrieveTask.AsTask);

            // then
            actualAttachmentServiceException.Should().BeEquivalentTo(
                expectedAttachmentDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAttachmentDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogIt()
        {
            // given
            var someAttachmentId = Guid.NewGuid();
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
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Attachment> retrieveTask =
                this.attachmentService.RetrieveAttachmentByIdAsync(someAttachmentId);

            AttachmentDependencyException actualAttachmentServiceException =
               await Assert.ThrowsAsync<AttachmentDependencyException>(
                   retrieveTask.AsTask);

            // then
            actualAttachmentServiceException.Should().BeEquivalentTo(
                expectedAttachmentDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogIt()
        {
            // given
            var someAttachmentId = Guid.NewGuid();
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
                    .Throws(serviceException);

            // when 
            ValueTask<Attachment> retrieveTask =
                this.attachmentService.RetrieveAttachmentByIdAsync(someAttachmentId);

            AttachmentServiceException actualAttachmentServiceException =
               await Assert.ThrowsAsync<AttachmentServiceException>(
                   retrieveTask.AsTask);

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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
