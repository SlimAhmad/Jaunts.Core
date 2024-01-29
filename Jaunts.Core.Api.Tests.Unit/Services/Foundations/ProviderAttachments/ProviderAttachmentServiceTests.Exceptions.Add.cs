// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderAttachments
{
    public partial class ProviderAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            ProviderAttachment randomProviderAttachment = CreateRandomProviderAttachment();
            ProviderAttachment inputProviderAttachment = randomProviderAttachment;
            var sqlException = GetSqlException();

            var failedProviderAttachmentStorageException =
                new FailedProviderAttachmentStorageException(
                    message: "Failed ProviderAttachment storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedProviderAttachmentDependencyException =
                new ProviderAttachmentDependencyException(
                    message: "ProviderAttachment dependency error occurred, contact support.",
                    innerException: failedProviderAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProviderAttachmentAsync(It.IsAny<ProviderAttachment>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ProviderAttachment> addProviderAttachmentTask =
                this.providerAttachmentService.AddProviderAttachmentAsync(inputProviderAttachment);

            ProviderAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<ProviderAttachmentDependencyException>(
                     addProviderAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProviderAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProviderAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAttachmentAsync(It.IsAny<ProviderAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            ProviderAttachment randomProviderAttachment = CreateRandomProviderAttachment();
            ProviderAttachment inputProviderAttachment = randomProviderAttachment;
            var databaseUpdateException = new DbUpdateException();

            var failedProviderAttachmentStorageException =
              new FailedProviderAttachmentStorageException(
                  message: "Failed ProviderAttachment storage error occurred, please contact support.",
                  innerException: databaseUpdateException);

            var expectedProviderAttachmentDependencyException =
                new ProviderAttachmentDependencyException(
                    message: "ProviderAttachment dependency error occurred, contact support.",
                    innerException: failedProviderAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProviderAttachmentAsync(It.IsAny<ProviderAttachment>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ProviderAttachment> addProviderAttachmentTask =
                this.providerAttachmentService.AddProviderAttachmentAsync(inputProviderAttachment);

            ProviderAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<ProviderAttachmentDependencyException>(
                     addProviderAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProviderAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAttachmentAsync(It.IsAny<ProviderAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            ProviderAttachment randomProviderAttachment = CreateRandomProviderAttachment();
            ProviderAttachment inputProviderAttachment = randomProviderAttachment;
            var serviceException = new Exception();

            var failedProviderAttachmentServiceException =
                new FailedProviderAttachmentServiceException(
                    message: "Failed ProviderAttachment service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedProviderAttachmentServiceException =
                new ProviderAttachmentServiceException(
                    message: "ProviderAttachment service error occurred, contact support.",
                    innerException: failedProviderAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProviderAttachmentAsync(It.IsAny<ProviderAttachment>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ProviderAttachment> addProviderAttachmentTask =
                 this.providerAttachmentService.AddProviderAttachmentAsync(inputProviderAttachment);

            ProviderAttachmentServiceException actualAttachmentDependencyException =
             await Assert.ThrowsAsync<ProviderAttachmentServiceException>(
                 addProviderAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProviderAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAttachmentAsync(It.IsAny<ProviderAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
