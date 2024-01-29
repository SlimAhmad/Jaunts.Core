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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Attachment someAttachment = CreateRandomAttachment(randomDateTime);
            someAttachment.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

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
                    .ThrowsAsync(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Attachment> modifyAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(someAttachment);

            AttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<AttachmentDependencyException>(
                     modifyAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAttachmentDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAttachmentDependencyException))),
                        Times.Once);

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
            Attachment someAttachment = CreateRandomAttachment(randomDateTime);
            someAttachment.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Attachment> modifyAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(someAttachment);

            AttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<AttachmentDependencyException>(
                     modifyAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAttachmentDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentDependencyException))),
                        Times.Once);

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
            Attachment someAttachment = CreateRandomAttachment(randomDateTime);
            someAttachment.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Attachment> modifyAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(someAttachment);

            AttachmentServiceException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<AttachmentServiceException>(
                     modifyAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAttachmentServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentServiceException))),
                        Times.Once);

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
            Attachment someAttachment = CreateRandomAttachment(randomDateTime);
            someAttachment.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var failedLockedAttachmentException =
                new LockedAttachmentException(
                    message: "Locked attachment record exception, please try again later.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(
                    message: "Attachment dependency error occurred, contact support.",
                    innerException: failedLockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Attachment> modifyAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(someAttachment);

            AttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<AttachmentDependencyException>(
                     modifyAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAttachmentDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
