// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            CustomerAttachment randomCustomerAttachment = CreateRandomCustomerAttachment();
            CustomerAttachment inputCustomerAttachment = randomCustomerAttachment;
            var sqlException = GetSqlException();

            var failedCustomerAttachmentStorageException =
                new FailedCustomerAttachmentStorageException(
                    message: "Failed CustomerAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedCustomerAttachmentDependencyException =
                new CustomerAttachmentDependencyException(
                    message: "CustomerAttachment dependency error occurred, contact support.",
                    innerException: failedCustomerAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCustomerAttachmentAsync(It.IsAny<CustomerAttachment>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<CustomerAttachment> addCustomerAttachmentTask =
                this.customerAttachmentService.AddCustomerAttachmentAsync(inputCustomerAttachment);

            CustomerAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<CustomerAttachmentDependencyException>(
                     addCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedCustomerAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAttachmentAsync(It.IsAny<CustomerAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            CustomerAttachment randomCustomerAttachment = CreateRandomCustomerAttachment();
            CustomerAttachment inputCustomerAttachment = randomCustomerAttachment;
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
                broker.InsertCustomerAttachmentAsync(It.IsAny<CustomerAttachment>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<CustomerAttachment> addCustomerAttachmentTask =
                this.customerAttachmentService.AddCustomerAttachmentAsync(inputCustomerAttachment);

            CustomerAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<CustomerAttachmentDependencyException>(
                     addCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedCustomerAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAttachmentAsync(It.IsAny<CustomerAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            CustomerAttachment randomCustomerAttachment = CreateRandomCustomerAttachment();
            CustomerAttachment inputCustomerAttachment = randomCustomerAttachment;
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
                broker.InsertCustomerAttachmentAsync(It.IsAny<CustomerAttachment>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<CustomerAttachment> addCustomerAttachmentTask =
                 this.customerAttachmentService.AddCustomerAttachmentAsync(inputCustomerAttachment);

            CustomerAttachmentServiceException actualAttachmentDependencyException =
             await Assert.ThrowsAsync<CustomerAttachmentServiceException>(
                 addCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedCustomerAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAttachmentAsync(It.IsAny<CustomerAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
