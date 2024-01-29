// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.CustomerAttachments
{
    public partial class CustomerAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someCustomerId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedCustomerAttachmentStorageException =
                new FailedCustomerAttachmentStorageException(
                    message: "Failed CustomerAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedCustomerAttachmentDependencyException =
                new CustomerAttachmentDependencyException
                (message: "CustomerAttachment dependency error occurred, contact support.", 
                innerException: failedCustomerAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<CustomerAttachment> removeCustomerAttachmentTask =
                this.customerAttachmentService.RemoveCustomerAttachmentByIdAsync(
                    someCustomerId,
                    someAttachmentId);

            CustomerAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<CustomerAttachmentDependencyException>(
                removeCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedCustomerAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCustomerAttachmentAsync(It.IsAny<CustomerAttachment>()),
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
            Guid someCustomerId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedCustomerAttachmentStorageException =
                new FailedCustomerAttachmentStorageException(
                    message: "Failed CustomerAttachment storage error occurred, Please contact support.",
                    innerException: databaseUpdateException);

            var expectedCustomerAttachmentDependencyException =
                new CustomerAttachmentDependencyException(
                    message: "CustomerAttachment dependency error occurred, contact support.",
                    innerException: failedCustomerAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<CustomerAttachment> removeCustomerAttachmentTask =
                this.customerAttachmentService.RemoveCustomerAttachmentByIdAsync
                (someCustomerId, someAttachmentId);

            CustomerAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<CustomerAttachmentDependencyException>(
                removeCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedCustomerAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCustomerAttachmentAsync(It.IsAny<CustomerAttachment>()),
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
            Guid someCustomerId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedCustomerAttachmentException(
                    message: "Locked CustomerAttachment record exception, Please try again later.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedCustomerAttachmentException =
                new CustomerAttachmentDependencyException(
                    message: "CustomerAttachment dependency error occurred, contact support.",
                    innerException: lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<CustomerAttachment> removeCustomerAttachmentTask =
                this.customerAttachmentService.RemoveCustomerAttachmentByIdAsync(someCustomerId, someAttachmentId);

            CustomerAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<CustomerAttachmentDependencyException>(
                removeCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedCustomerAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCustomerAttachmentAsync(It.IsAny<CustomerAttachment>()),
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
            var randomCustomerId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someCustomerId = randomCustomerId;
            var serviceException = new Exception();

            var failedCustomerAttachmentServiceException =
                new FailedCustomerAttachmentServiceException(
                    message: "Failed CustomerAttachment service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedCustomerAttachmentException =
                new CustomerAttachmentServiceException(
                    message: "CustomerAttachment service error occurred, contact support.",
                    innerException: failedCustomerAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<CustomerAttachment> removeCustomerAttachmentTask =
                this.customerAttachmentService.RemoveCustomerAttachmentByIdAsync(
                    someCustomerId,
                    someAttachmentId);

            CustomerAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<CustomerAttachmentServiceException>(
                removeCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedCustomerAttachmentException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCustomerAttachmentAsync(It.IsAny<CustomerAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}