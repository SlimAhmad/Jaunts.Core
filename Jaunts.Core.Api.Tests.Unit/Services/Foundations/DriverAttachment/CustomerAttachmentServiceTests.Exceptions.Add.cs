// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.DriverAttachments
{
    public partial class DriverAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DriverAttachment randomDriverAttachment = CreateRandomDriverAttachment();
            DriverAttachment inputDriverAttachment = randomDriverAttachment;
            var sqlException = GetSqlException();

            var failedDriverAttachmentStorageException =
                new FailedDriverAttachmentStorageException(
                    message: "Failed DriverAttachment storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDriverAttachmentDependencyException =
                new DriverAttachmentDependencyException(
                    message: "DriverAttachment dependency error occurred, contact support.",
                    innerException: failedDriverAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDriverAttachmentAsync(It.IsAny<DriverAttachment>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<DriverAttachment> addDriverAttachmentTask =
                this.driverAttachmentService.AddDriverAttachmentAsync(inputDriverAttachment);

            DriverAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<DriverAttachmentDependencyException>(
                     addDriverAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedDriverAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDriverAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAttachmentAsync(It.IsAny<DriverAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DriverAttachment randomDriverAttachment = CreateRandomDriverAttachment();
            DriverAttachment inputDriverAttachment = randomDriverAttachment;
            var databaseUpdateException = new DbUpdateException();

            var failedDriverAttachmentStorageException =
              new FailedDriverAttachmentStorageException(
                  message: "Failed DriverAttachment storage error occurred, please contact support.",
                  innerException: databaseUpdateException);

            var expectedDriverAttachmentDependencyException =
                new DriverAttachmentDependencyException(
                    message: "DriverAttachment dependency error occurred, contact support.",
                    innerException: failedDriverAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDriverAttachmentAsync(It.IsAny<DriverAttachment>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<DriverAttachment> addDriverAttachmentTask =
                this.driverAttachmentService.AddDriverAttachmentAsync(inputDriverAttachment);

            DriverAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<DriverAttachmentDependencyException>(
                     addDriverAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedDriverAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAttachmentAsync(It.IsAny<DriverAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            DriverAttachment randomDriverAttachment = CreateRandomDriverAttachment();
            DriverAttachment inputDriverAttachment = randomDriverAttachment;
            var serviceException = new Exception();

            var failedDriverAttachmentServiceException =
                new FailedDriverAttachmentServiceException(
                    message: "Failed DriverAttachment service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDriverAttachmentServiceException =
                new DriverAttachmentServiceException(
                    message: "DriverAttachment service error occurred, contact support.",
                    innerException: failedDriverAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDriverAttachmentAsync(It.IsAny<DriverAttachment>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DriverAttachment> addDriverAttachmentTask =
                 this.driverAttachmentService.AddDriverAttachmentAsync(inputDriverAttachment);

            DriverAttachmentServiceException actualAttachmentDependencyException =
             await Assert.ThrowsAsync<DriverAttachmentServiceException>(
                 addDriverAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedDriverAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAttachmentAsync(It.IsAny<DriverAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
