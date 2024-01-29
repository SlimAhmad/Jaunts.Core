// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.CustomerAttachments
{
    public partial class CustomerAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCustomerAttachmentIsNullAndLogItAsync()
        {
            // given
            CustomerAttachment invalidCustomerAttachment = null;

            var nullCustomerAttachmentException = new NullCustomerAttachmentException(
                message: "The CustomerAttachment is null.");

            var expectedCustomerAttachmentValidationException =
                new CustomerAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: nullCustomerAttachmentException);

            // when
            ValueTask<CustomerAttachment> addCustomerAttachmentTask =
                this.customerAttachmentService.AddCustomerAttachmentAsync(invalidCustomerAttachment);

            CustomerAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<CustomerAttachmentValidationException>(
                  addCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAttachmentAsync(It.IsAny<CustomerAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCustomerIdIsInvalidAndLogItAsync()
        {
            // given
            CustomerAttachment randomCustomerAttachment = CreateRandomCustomerAttachment();
            CustomerAttachment inputCustomerAttachment = randomCustomerAttachment;
            inputCustomerAttachment.CustomerId = default;

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
            ValueTask<CustomerAttachment> addCustomerAttachmentTask =
                this.customerAttachmentService.AddCustomerAttachmentAsync(inputCustomerAttachment);

            CustomerAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<CustomerAttachmentValidationException>(
                  addCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAttachmentAsync(It.IsAny<CustomerAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            CustomerAttachment randomCustomerAttachment = CreateRandomCustomerAttachment();
            CustomerAttachment inputCustomerAttachment = randomCustomerAttachment;
            inputCustomerAttachment.AttachmentId = default;

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
            ValueTask<CustomerAttachment> addCustomerAttachmentTask =
                this.customerAttachmentService.AddCustomerAttachmentAsync(inputCustomerAttachment);

            CustomerAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<CustomerAttachmentValidationException>(
                  addCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCustomerAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAttachmentAsync(It.IsAny<CustomerAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCustomerAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            CustomerAttachment randomCustomerAttachment = CreateRandomCustomerAttachment();
            CustomerAttachment alreadyExistsCustomerAttachment = randomCustomerAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsCustomerAttachmentException =
                new AlreadyExistsCustomerAttachmentException(
                    message: "CustomerAttachment  with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedCustomerAttachmentValidationException =
                new CustomerAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: alreadyExistsCustomerAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCustomerAttachmentAsync(alreadyExistsCustomerAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<CustomerAttachment> addCustomerAttachmentTask =
                this.customerAttachmentService.AddCustomerAttachmentAsync(alreadyExistsCustomerAttachment);

            CustomerAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<CustomerAttachmentValidationException>(
                  addCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCustomerAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAttachmentAsync(alreadyExistsCustomerAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferenceExceptionAndLogItAsync()
        {
            // given
            CustomerAttachment randomCustomerAttachment = CreateRandomCustomerAttachment();
            CustomerAttachment invalidCustomerAttachment = randomCustomerAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidCustomerAttachmentReferenceException =
                new InvalidCustomerAttachmentReferenceException(
                    message: "Invalid guardian attachment reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedCustomerAttachmentValidationException =
                new CustomerAttachmentValidationException(
                    message: "Invalid input, contact support.", 
                    innerException: invalidCustomerAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCustomerAttachmentAsync(invalidCustomerAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<CustomerAttachment> addCustomerAttachmentTask =
                this.customerAttachmentService.AddCustomerAttachmentAsync(invalidCustomerAttachment);

            CustomerAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<CustomerAttachmentValidationException>(
                  addCustomerAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedCustomerAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCustomerAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAttachmentAsync(invalidCustomerAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
