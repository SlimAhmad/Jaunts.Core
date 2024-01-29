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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
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
                new ProvidersDirectorAttachmentDependencyException(
                    message: "ProvidersDirectorAttachment dependency error occurred, contact support.",
                    innerException: failedProvidersDirectorAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ProvidersDirectorAttachment> retrieveProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.RetrieveProvidersDirectorAttachmentByIdAsync(someProviderDirectorsId, someAttachmentId);

            ProvidersDirectorAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProvidersDirectorAttachmentDependencyException>(
                retrieveProvidersDirectorAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
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
            ValueTask<ProvidersDirectorAttachment> retrieveProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.RetrieveProvidersDirectorAttachmentByIdAsync(someProviderDirectorsId, someAttachmentId);

            ProvidersDirectorAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProvidersDirectorAttachmentDependencyException>(
                retrieveProvidersDirectorAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
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
                    lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ProvidersDirectorAttachment> retrieveProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.RetrieveProvidersDirectorAttachmentByIdAsync(someProviderDirectorsId, someAttachmentId);

            ProvidersDirectorAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProvidersDirectorAttachmentDependencyException>(
                retrieveProvidersDirectorAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someProviderDirectorsId = Guid.NewGuid();
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
                broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ProvidersDirectorAttachment> retrieveProvidersDirectorAttachmentTask =
                this.providersDirectorAttachmentService.RetrieveProvidersDirectorAttachmentByIdAsync(someProviderDirectorsId, someAttachmentId);

            ProvidersDirectorAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProvidersDirectorAttachmentServiceException>(
                retrieveProvidersDirectorAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
