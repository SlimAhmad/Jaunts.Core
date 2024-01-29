// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOfferAttachments
{
    public partial class PromosOfferAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            PromosOfferAttachment randomPromosOfferAttachment = CreateRandomPromosOfferAttachment();
            PromosOfferAttachment inputPromosOfferAttachment = randomPromosOfferAttachment;
            var sqlException = GetSqlException();

            var failedPromosOfferAttachmentStorageException =
                new FailedPromosOfferAttachmentStorageException(
                    message: "Failed PromosOfferAttachment storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedPromosOfferAttachmentDependencyException =
                new PromosOfferAttachmentDependencyException(
                    message: "PromosOfferAttachment dependency error occurred, contact support.",
                    innerException: failedPromosOfferAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPromosOfferAttachmentAsync(It.IsAny<PromosOfferAttachment>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<PromosOfferAttachment> addPromosOfferAttachmentTask =
                this.promosOfferAttachmentService.AddPromosOfferAttachmentAsync(inputPromosOfferAttachment);

            PromosOfferAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<PromosOfferAttachmentDependencyException>(
                     addPromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAttachmentAsync(It.IsAny<PromosOfferAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            PromosOfferAttachment randomPromosOfferAttachment = CreateRandomPromosOfferAttachment();
            PromosOfferAttachment inputPromosOfferAttachment = randomPromosOfferAttachment;
            var databaseUpdateException = new DbUpdateException();

            var failedPromosOfferAttachmentStorageException =
              new FailedPromosOfferAttachmentStorageException(
                  message: "Failed PromosOfferAttachment storage error occurred, please contact support.",
                  innerException: databaseUpdateException);

            var expectedPromosOfferAttachmentDependencyException =
                new PromosOfferAttachmentDependencyException(
                    message: "PromosOfferAttachment dependency error occurred, contact support.",
                    innerException: failedPromosOfferAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPromosOfferAttachmentAsync(It.IsAny<PromosOfferAttachment>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<PromosOfferAttachment> addPromosOfferAttachmentTask =
                this.promosOfferAttachmentService.AddPromosOfferAttachmentAsync(inputPromosOfferAttachment);

            PromosOfferAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<PromosOfferAttachmentDependencyException>(
                     addPromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAttachmentAsync(It.IsAny<PromosOfferAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            PromosOfferAttachment randomPromosOfferAttachment = CreateRandomPromosOfferAttachment();
            PromosOfferAttachment inputPromosOfferAttachment = randomPromosOfferAttachment;
            var serviceException = new Exception();

            var failedPromosOfferAttachmentServiceException =
                new FailedPromosOfferAttachmentServiceException(
                    message: "Failed PromosOfferAttachment service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedPromosOfferAttachmentServiceException =
                new PromosOfferAttachmentServiceException(
                    message: "PromosOfferAttachment service error occurred, contact support.",
                    innerException: failedPromosOfferAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPromosOfferAttachmentAsync(It.IsAny<PromosOfferAttachment>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<PromosOfferAttachment> addPromosOfferAttachmentTask =
                 this.promosOfferAttachmentService.AddPromosOfferAttachmentAsync(inputPromosOfferAttachment);

            PromosOfferAttachmentServiceException actualAttachmentDependencyException =
             await Assert.ThrowsAsync<PromosOfferAttachmentServiceException>(
                 addPromosOfferAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedPromosOfferAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPromosOfferAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAttachmentAsync(It.IsAny<PromosOfferAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
