// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
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
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someProviderId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedProviderAttachmentStorageException =
                new FailedProviderAttachmentStorageException(
                    message: "Failed ProviderAttachment storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedProviderAttachmentDependencyException =
                new ProviderAttachmentDependencyException
                (message: "ProviderAttachment dependency error occurred, contact support.", 
                innerException: failedProviderAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ProviderAttachment> removeProviderAttachmentTask =
                this.providerAttachmentService.RemoveProviderAttachmentByIdAsync(
                    someProviderId,
                    someAttachmentId);

            ProviderAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProviderAttachmentDependencyException>(
                removeProviderAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProviderAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProviderAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderAttachmentAsync(It.IsAny<ProviderAttachment>()),
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
            Guid someProviderId = Guid.NewGuid();
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
                broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ProviderAttachment> removeProviderAttachmentTask =
                this.providerAttachmentService.RemoveProviderAttachmentByIdAsync
                (someProviderId, someAttachmentId);

            ProviderAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProviderAttachmentDependencyException>(
                removeProviderAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProviderAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderAttachmentAsync(It.IsAny<ProviderAttachment>()),
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
            Guid someProviderId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedProviderAttachmentException(
                    message: "Locked ProviderAttachment record exception, please try again later.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedProviderAttachmentException =
                new ProviderAttachmentDependencyException(
                    message: "ProviderAttachment dependency error occurred, contact support.",
                    innerException: lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ProviderAttachment> removeProviderAttachmentTask =
                this.providerAttachmentService.RemoveProviderAttachmentByIdAsync(someProviderId, someAttachmentId);

            ProviderAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProviderAttachmentDependencyException>(
                removeProviderAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProviderAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderAttachmentAsync(It.IsAny<ProviderAttachment>()),
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
            var randomProviderId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someProviderId = randomProviderId;
            var serviceException = new Exception();

            var failedProviderAttachmentServiceException =
                new FailedProviderAttachmentServiceException(
                    message: "Failed ProviderAttachment service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedProviderAttachmentException =
                new ProviderAttachmentServiceException(
                    message: "ProviderAttachment service error occurred, contact support.",
                    innerException: failedProviderAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ProviderAttachment> removeProviderAttachmentTask =
                this.providerAttachmentService.RemoveProviderAttachmentByIdAsync(
                    someProviderId,
                    someAttachmentId);

            ProviderAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProviderAttachmentServiceException>(
                removeProviderAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProviderAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderAttachmentAsync(It.IsAny<ProviderAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}