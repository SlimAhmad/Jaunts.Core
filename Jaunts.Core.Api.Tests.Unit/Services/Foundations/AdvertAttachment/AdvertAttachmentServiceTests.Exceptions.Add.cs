// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            AdvertAttachment randomAdvertAttachment = CreateRandomAdvertAttachment();
            AdvertAttachment inputAdvertAttachment = randomAdvertAttachment;
            var sqlException = GetSqlException();

            var failedAdvertAttachmentStorageException =
                new FailedAdvertAttachmentStorageException(
                    message: "Failed AdvertAttachment storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedAdvertAttachmentDependencyException =
                new AdvertAttachmentDependencyException(
                    message: "AdvertAttachment dependency error occurred, contact support.",
                    innerException: failedAdvertAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<AdvertAttachment> addAdvertAttachmentTask =
                this.AdvertAttachmentService.AddAdvertAttachmentAsync(inputAdvertAttachment);

            AdvertAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<AdvertAttachmentDependencyException>(
                     addAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAdvertAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            AdvertAttachment randomAdvertAttachment = CreateRandomAdvertAttachment();
            AdvertAttachment inputAdvertAttachment = randomAdvertAttachment;
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
                broker.InsertAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<AdvertAttachment> addAdvertAttachmentTask =
                this.AdvertAttachmentService.AddAdvertAttachmentAsync(inputAdvertAttachment);

            AdvertAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<AdvertAttachmentDependencyException>(
                     addAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAdvertAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            AdvertAttachment randomAdvertAttachment = CreateRandomAdvertAttachment();
            AdvertAttachment inputAdvertAttachment = randomAdvertAttachment;
            var serviceException = new Exception();

            var failedAdvertAttachmentServiceException =
                new FailedAdvertAttachmentServiceException(
                    message: "Failed AdvertAttachment service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAdvertAttachmentServiceException =
                new AdvertAttachmentServiceException(
                    message: "AdvertAttachment service error occurred, contact support.",
                    innerException: failedAdvertAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<AdvertAttachment> addAdvertAttachmentTask =
                 this.AdvertAttachmentService.AddAdvertAttachmentAsync(inputAdvertAttachment);

            AdvertAttachmentServiceException actualAttachmentDependencyException =
             await Assert.ThrowsAsync<AdvertAttachmentServiceException>(
                 addAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedAdvertAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
