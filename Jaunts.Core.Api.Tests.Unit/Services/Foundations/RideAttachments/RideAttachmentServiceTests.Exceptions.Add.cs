// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.RideAttachments
{
    public partial class RideAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            RideAttachment randomRideAttachment = CreateRandomRideAttachment();
            RideAttachment inputRideAttachment = randomRideAttachment;
            var sqlException = GetSqlException();

            var failedRideAttachmentStorageException =
                new FailedRideAttachmentStorageException(
                    message: "Failed RideAttachment storage error occurred, Please contact support.",
                    innerException: sqlException);

            var expectedRideAttachmentDependencyException =
                new RideAttachmentDependencyException(
                    message: "RideAttachment dependency error occurred, contact support.",
                    innerException: failedRideAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRideAttachmentAsync(It.IsAny<RideAttachment>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<RideAttachment> addRideAttachmentTask =
                this.rideAttachmentService.AddRideAttachmentAsync(inputRideAttachment);

            RideAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<RideAttachmentDependencyException>(
                     addRideAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedRideAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRideAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAttachmentAsync(It.IsAny<RideAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            RideAttachment randomRideAttachment = CreateRandomRideAttachment();
            RideAttachment inputRideAttachment = randomRideAttachment;
            var databaseUpdateException = new DbUpdateException();

            var failedRideAttachmentStorageException =
              new FailedRideAttachmentStorageException(
                  message: "Failed RideAttachment storage error occurred, Please contact support.",
                  innerException: databaseUpdateException);

            var expectedRideAttachmentDependencyException =
                new RideAttachmentDependencyException(
                    message: "RideAttachment dependency error occurred, contact support.",
                    innerException: failedRideAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRideAttachmentAsync(It.IsAny<RideAttachment>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<RideAttachment> addRideAttachmentTask =
                this.rideAttachmentService.AddRideAttachmentAsync(inputRideAttachment);

            RideAttachmentDependencyException actualAttachmentDependencyException =
                 await Assert.ThrowsAsync<RideAttachmentDependencyException>(
                     addRideAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedRideAttachmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAttachmentAsync(It.IsAny<RideAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            RideAttachment randomRideAttachment = CreateRandomRideAttachment();
            RideAttachment inputRideAttachment = randomRideAttachment;
            var serviceException = new Exception();

            var failedRideAttachmentServiceException =
                new FailedRideAttachmentServiceException(
                    message: "Failed RideAttachment service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedRideAttachmentServiceException =
                new RideAttachmentServiceException(
                    message: "RideAttachment service error occurred, contact support.",
                    innerException: failedRideAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRideAttachmentAsync(It.IsAny<RideAttachment>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<RideAttachment> addRideAttachmentTask =
                 this.rideAttachmentService.AddRideAttachmentAsync(inputRideAttachment);

            RideAttachmentServiceException actualAttachmentDependencyException =
             await Assert.ThrowsAsync<RideAttachmentServiceException>(
                 addRideAttachmentTask.AsTask);

            // then
            actualAttachmentDependencyException.Should().BeEquivalentTo(
                expectedRideAttachmentServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRideAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAttachmentAsync(It.IsAny<RideAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
