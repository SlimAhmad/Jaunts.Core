// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.CustomerAttachments
{
    public partial class CustomerAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenCustomerIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomCustomerId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputCustomerId = randomCustomerId;

            var invalidCustomerAttachmentException =
                 new InvalidCustomerAttachmentException(
                     message: "Invalid CustomerAttachment. Please fix the errors and try again.");

            invalidCustomerAttachmentException.AddData(
              key: nameof(CustomerAttachment.CustomerId),
              values: "Id is required");

            var expectedCustomerAttachmentValidationException =
                new CustomerAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidCustomerAttachmentException);

            // when
            ValueTask<CustomerAttachment> actualCustomerAttachmentTask =
                this.customerAttachmentService.RetrieveCustomerAttachmentByIdAsync(inputCustomerId, inputAttachmentId);

            CustomerAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<CustomerAttachmentValidationException>(
                  actualCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = default;
            Guid randomCustomerId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputCustomerId = randomCustomerId;

            var invalidCustomerAttachmentException =
                 new InvalidCustomerAttachmentException(
                     message: "Invalid CustomerAttachment. Please fix the errors and try again.");

            invalidCustomerAttachmentException.AddData(
              key: nameof(CustomerAttachment.AttachmentId),
              values: "Id is required");

            var expectedCustomerAttachmentValidationException =
                new CustomerAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidCustomerAttachmentException);

            // when
            ValueTask<CustomerAttachment> actualCustomerAttachmentTask =
                this.customerAttachmentService.RetrieveCustomerAttachmentByIdAsync(inputCustomerId, inputAttachmentId);

            CustomerAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<CustomerAttachmentValidationException>(
                  actualCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageCustomerAttachmentIsInvalidAndLogItAsync()
        {
            // given
            CustomerAttachment randomCustomerAttachment = CreateRandomCustomerAttachment();
            Guid inputAttachmentId = randomCustomerAttachment.AttachmentId;
            Guid inputCustomerId = randomCustomerAttachment.CustomerId;
            CustomerAttachment nullStorageCustomerAttachment = null;

            var notFoundCustomerAttachmentException =
               new NotFoundCustomerAttachmentException(
                   message: $"Couldn't find attachment with Customer id: {inputCustomerId} " +
                        $"and attachment id: {inputAttachmentId}.");

            var expectedCustomerValidationException =
                new CustomerAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    notFoundCustomerAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectCustomerAttachmentByIdAsync(inputCustomerId, inputAttachmentId))
                    .ReturnsAsync(nullStorageCustomerAttachment);

            // when
            ValueTask<CustomerAttachment> actualCustomerAttachmentRetrieveTask =
                this.customerAttachmentService.RetrieveCustomerAttachmentByIdAsync(inputCustomerId, inputAttachmentId);

            CustomerAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<CustomerAttachmentValidationException>(
                  actualCustomerAttachmentRetrieveTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
