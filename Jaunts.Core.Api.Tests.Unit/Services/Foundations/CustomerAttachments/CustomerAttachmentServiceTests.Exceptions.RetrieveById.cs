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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
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
                new CustomerAttachmentDependencyException(
                    message: "CustomerAttachment dependency error occurred, contact support.",
                    innerException: failedCustomerAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<CustomerAttachment> retrieveCustomerAttachmentTask =
                this.customerAttachmentService.RetrieveCustomerAttachmentByIdAsync(someCustomerId, someAttachmentId);

            CustomerAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<CustomerAttachmentDependencyException>(
                retrieveCustomerAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
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
            ValueTask<CustomerAttachment> retrieveCustomerAttachmentTask =
                this.customerAttachmentService.RetrieveCustomerAttachmentByIdAsync(someCustomerId, someAttachmentId);

            CustomerAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<CustomerAttachmentDependencyException>(
                retrieveCustomerAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
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
                    lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<CustomerAttachment> retrieveCustomerAttachmentTask =
                this.customerAttachmentService.RetrieveCustomerAttachmentByIdAsync(someCustomerId, someAttachmentId);

            CustomerAttachmentDependencyException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<CustomerAttachmentDependencyException>(
                retrieveCustomerAttachmentTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someCustomerId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedCustomerAttachmentServiceException =
                 new FailedCustomerAttachmentServiceException(
                     message: "Failed CustomerAttachment service error occurred, Please contact support.",
                     innerException: serviceException);

            var expectedCustomerAttachmentServiceException =
                new CustomerAttachmentServiceException(
                    message: "CustomerAttachment service error occurred, contact support.",
                    innerException: failedCustomerAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<CustomerAttachment> retrieveCustomerAttachmentTask =
                this.customerAttachmentService.RetrieveCustomerAttachmentByIdAsync(someCustomerId, someAttachmentId);

            CustomerAttachmentServiceException actualAttachmentDependencyException =
            await Assert.ThrowsAsync<CustomerAttachmentServiceException>(
                retrieveCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedCustomerAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
