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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
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
                new ProviderAttachmentDependencyException(
                    message: "ProviderAttachment dependency error occurred, contact support.",
                    innerException: failedProviderAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ProviderAttachment> retrieveProviderAttachmentTask =
                this.providerAttachmentService.RetrieveProviderAttachmentByIdAsync(someProviderId, someAttachmentId);

            ProviderAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProviderAttachmentDependencyException>(
                retrieveProviderAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
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
            ValueTask<ProviderAttachment> retrieveProviderAttachmentTask =
                this.providerAttachmentService.RetrieveProviderAttachmentByIdAsync(someProviderId, someAttachmentId);

            ProviderAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProviderAttachmentDependencyException>(
                retrieveProviderAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
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
                    lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ProviderAttachment> retrieveProviderAttachmentTask =
                this.providerAttachmentService.RetrieveProviderAttachmentByIdAsync(someProviderId, someAttachmentId);

            ProviderAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProviderAttachmentDependencyException>(
                retrieveProviderAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someProviderId = Guid.NewGuid();
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
                broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ProviderAttachment> retrieveProviderAttachmentTask =
                this.providerAttachmentService.RetrieveProviderAttachmentByIdAsync(someProviderId, someAttachmentId);

            ProviderAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<ProviderAttachmentServiceException>(
                retrieveProviderAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedProviderAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
