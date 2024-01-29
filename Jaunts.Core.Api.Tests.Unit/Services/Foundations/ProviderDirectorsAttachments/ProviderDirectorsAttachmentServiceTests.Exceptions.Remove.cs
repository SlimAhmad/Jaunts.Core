// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
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
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someProviderDirectorsId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedProvidersDirectorAttachmentStorageException =
                new FailedProvidersDirectorAttachmentStorageException(
                    message: "Failed ProvidersDirectorAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedProvidersDirectorAttachmentDependencyException =
                new ProvidersDirectorAttachmentDependencyException
                (message: "ProvidersDirectorAttachment dependency error occurred, contact support.", 
                innerException: failedProvidersDirectorAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ProvidersDirectorAttachment> removeProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.RemoveProvidersDirectorAttachmentByIdAsync(
                    someProviderDirectorsId,
                    someAttachmentId);

            ProvidersDirectorAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProvidersDirectorAttachmentDependencyException>(
                removeProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()),
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
            Guid someProviderDirectorsId = Guid.NewGuid();
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
                broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ProvidersDirectorAttachment> removeProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.RemoveProvidersDirectorAttachmentByIdAsync
                (someProviderDirectorsId, someAttachmentId);

            ProvidersDirectorAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProvidersDirectorAttachmentDependencyException>(
                removeProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()),
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
            Guid someProviderDirectorsId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedProvidersDirectorAttachmentException(
                    message: "Locked ProvidersDirectorAttachment record exception, Please try again later.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedProvidersDirectorAttachmentException =
                new ProvidersDirectorAttachmentDependencyException(
                    message: "ProvidersDirectorAttachment dependency error occurred, contact support.",
                    innerException: lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ProvidersDirectorAttachment> removeProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.RemoveProvidersDirectorAttachmentByIdAsync(someProviderDirectorsId, someAttachmentId);

            ProvidersDirectorAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProvidersDirectorAttachmentDependencyException>(
                removeProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()),
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
            var randomProviderDirectorsId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someProviderDirectorsId = randomProviderDirectorsId;
            var serviceException = new Exception();

            var failedProvidersDirectorAttachmentServiceException =
                new FailedProvidersDirectorAttachmentServiceException(
                    message: "Failed ProvidersDirectorAttachment service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedProvidersDirectorAttachmentException =
                new ProvidersDirectorAttachmentServiceException(
                    message: "ProvidersDirectorAttachment service error occurred, contact support.",
                    innerException: failedProvidersDirectorAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ProvidersDirectorAttachment> removeProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.RemoveProvidersDirectorAttachmentByIdAsync(
                    someProviderDirectorsId,
                    someAttachmentId);

            ProvidersDirectorAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProvidersDirectorAttachmentServiceException>(
                removeProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProvidersDirectorAttachmentAsync(It.IsAny<ProvidersDirectorAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}