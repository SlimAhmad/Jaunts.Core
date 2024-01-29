// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectorAttachments
{
    public partial class ProvidersDirectorAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            ProvidersDirectorAttachment randomProvidersDirectorAttachment = CreateRandomProvidersDirectorAttachment();
            ProvidersDirectorAttachment inputProvidersDirectorAttachment = randomProvidersDirectorAttachment;
            var sqlException = GetSqlException();

            var failedProvidersDirectorAttachmentStorageException =
                new FailedProvidersDirectorAttachmentStorageException(
                    message: "Failed ProvidersDirectorAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedProvidersDirectorAttachmentDependencyException =
                new ProvidersDirectorAttachmentDependencyException(
                    message: "ProvidersDirectorAttachment dependency error occurred, contact support.",
                    innerException: failedProvidersDirectorAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ProvidersDirectorAttachment> addProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.AddProvidersDirectorAttachmentAsync(inputProvidersDirectorAttachment);

            ProvidersDirectorAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<ProvidersDirectorAttachmentDependencyException>(
                     addProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            ProvidersDirectorAttachment randomProvidersDirectorAttachment = CreateRandomProvidersDirectorAttachment();
            ProvidersDirectorAttachment inputProvidersDirectorAttachment = randomProvidersDirectorAttachment;
            var databaseUpdateException = new DbUpdateException();

            var failedProvidersDirectorAttachmentStorageException =
              new FailedProvidersDirectorAttachmentStorageException(
                  message: "Failed ProvidersDirectorAttachment storage error occurred, Please contact support.",
                  innerException: databaseUpdateException);

            var expectedProvidersDirectorAttachmentDependencyException =
                new ProvidersDirectorAttachmentDependencyException(
                    message: "ProvidersDirectorAttachment dependency error occurred, contact support.",
                    innerException: failedProvidersDirectorAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ProvidersDirectorAttachment> addProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.AddProvidersDirectorAttachmentAsync(inputProvidersDirectorAttachment);

            ProvidersDirectorAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<ProvidersDirectorAttachmentDependencyException>(
                     addProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            ProvidersDirectorAttachment randomProvidersDirectorAttachment = CreateRandomProvidersDirectorAttachment();
            ProvidersDirectorAttachment inputProvidersDirectorAttachment = randomProvidersDirectorAttachment;
            var serviceException = new Exception();

            var failedProvidersDirectorAttachmentServiceException =
                new FailedProvidersDirectorAttachmentServiceException(
                    message: "Failed ProvidersDirectorAttachment service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedProvidersDirectorAttachmentServiceException =
                new ProvidersDirectorAttachmentServiceException(
                    message: "ProvidersDirectorAttachment service error occurred, contact support.",
                    innerException: failedProvidersDirectorAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ProvidersDirectorAttachment> addProvidersDirectorAttachmentTask =
                 this.providersDirectorAttachmentService.AddProvidersDirectorAttachmentAsync(inputProvidersDirectorAttachment);

            ProvidersDirectorAttachmentServiceException actualAttachmentDependencyException =
             await Assert.ThrowsAsync<ProvidersDirectorAttachmentServiceException>(
                 addProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
