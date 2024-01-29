// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.DriverAttachments
{
    public partial class DriverAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenDriverIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomDriverId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputDriverId = randomDriverId;

            var invalidDriverAttachmentException =
              new InvalidDriverAttachmentException(
                  message: "Invalid DriverAttachment. Please correct the errors and try again.");

            invalidDriverAttachmentException.AddData(
                key: nameof(DriverAttachment.DriverId),
                values: "Id is required");

            var expectedDriverAttachmentValidationException =
                new DriverAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidDriverAttachmentException);

            // when
            ValueTask<DriverAttachment> removeDriverAttachmentTask =
                this.driverAttachmentService.RemoveDriverAttachmentByIdAsync(inputDriverId, inputAttachmentId);

            DriverAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<DriverAttachmentValidationException>(
                  removeDriverAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDriverAttachmentAsync(It.IsAny<DriverAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = default;
            Guid randomDriverId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputDriverId = randomDriverId;

            var invalidDriverAttachmentException =
              new InvalidDriverAttachmentException(
                  message: "Invalid DriverAttachment. Please correct the errors and try again.");

            invalidDriverAttachmentException.AddData(
              key: nameof(DriverAttachment.AttachmentId),
              values: "Id is required");

            var expectedDriverAttachmentValidationException =
                new DriverAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidDriverAttachmentException);

            // when
            ValueTask<DriverAttachment> removeDriverAttachmentTask =
                this.driverAttachmentService.RemoveDriverAttachmentByIdAsync(inputDriverId, inputAttachmentId);

            DriverAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<DriverAttachmentValidationException>(
                  removeDriverAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDriverAttachmentAsync(It.IsAny<DriverAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageDriverAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DriverAttachment randomDriverAttachment = CreateRandomDriverAttachment(randomDateTime);
            Guid inputAttachmentId = randomDriverAttachment.AttachmentId;
            Guid inputDriverId = randomDriverAttachment.DriverId;
            DriverAttachment nullStorageDriverAttachment = null;

            var notFoundDriverAttachmentException =
               new NotFoundDriverAttachmentException(
                   message: $"Couldn't find attachment with Driver id: {inputDriverId} " +
                        $"and attachment id: {inputAttachmentId}.");

            var expectedDriverValidationException =
                new DriverAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    notFoundDriverAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectDriverAttachmentByIdAsync(inputDriverId, inputAttachmentId))
                    .ReturnsAsync(nullStorageDriverAttachment);

            // when
            ValueTask<DriverAttachment> removeDriverAttachmentTask =
                this.driverAttachmentService.RemoveDriverAttachmentByIdAsync(inputDriverId, inputAttachmentId);

            DriverAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<DriverAttachmentValidationException>(
                  removeDriverAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedDriverValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDriverValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDriverAttachmentAsync(It.IsAny<DriverAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}