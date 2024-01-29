// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            ShortLetAttachment randomShortLetAttachment = CreateRandomShortLetAttachment();
            ShortLetAttachment inputShortLetAttachment = randomShortLetAttachment;
            var sqlException = GetSqlException();

            var failedShortLetAttachmentStorageException =
                new FailedShortLetAttachmentStorageException(
                    message: "Failed ShortLetAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedShortLetAttachmentDependencyException =
                new ShortLetAttachmentDependencyException(
                    message: "ShortLetAttachment dependency error occurred, contact support.",
                    innerException: failedShortLetAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ShortLetAttachment> addShortLetAttachmentTask =
                this.shortLetAttachmentService.AddShortLetAttachmentAsync(inputShortLetAttachment);

            ShortLetAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<ShortLetAttachmentDependencyException>(
                     addShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedShortLetAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            ShortLetAttachment randomShortLetAttachment = CreateRandomShortLetAttachment();
            ShortLetAttachment inputShortLetAttachment = randomShortLetAttachment;
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
                broker.InsertShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ShortLetAttachment> addShortLetAttachmentTask =
                this.shortLetAttachmentService.AddShortLetAttachmentAsync(inputShortLetAttachment);

            ShortLetAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<ShortLetAttachmentDependencyException>(
                     addShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedShortLetAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            ShortLetAttachment randomShortLetAttachment = CreateRandomShortLetAttachment();
            ShortLetAttachment inputShortLetAttachment = randomShortLetAttachment;
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
                broker.InsertShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ShortLetAttachment> addShortLetAttachmentTask =
                 this.shortLetAttachmentService.AddShortLetAttachmentAsync(inputShortLetAttachment);

            ShortLetAttachmentServiceException actualAttachmentDependencyException =
             await Assert.ThrowsAsync<ShortLetAttachmentServiceException>(
                 addShortLetAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedShortLetAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAttachmentAsync(It.IsAny<ShortLetAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
