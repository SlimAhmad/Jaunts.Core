// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.CustomerAttachments
{
    public partial class CustomerAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveCustomerAttachmentAsync()
        {
            // given
            var randomCustomerId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputCustomerId = randomCustomerId;
            Guid inputAttachmentId = randomAttachmentId;
            CustomerAttachment randomCustomerAttachment = CreateRandomCustomerAttachment();
            randomCustomerAttachment.CustomerId = inputCustomerId;
            randomCustomerAttachment.AttachmentId = inputAttachmentId;
            CustomerAttachment storageCustomerAttachment = randomCustomerAttachment;
            CustomerAttachment expectedCustomerAttachment = storageCustomerAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerAttachmentByIdAsync(inputCustomerId, inputAttachmentId))
                    .ReturnsAsync(storageCustomerAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteCustomerAttachmentAsync(storageCustomerAttachment))
                    .ReturnsAsync(expectedCustomerAttachment);

            // when
            CustomerAttachment actualCustomerAttachment =
                await this.customerAttachmentService.RemoveCustomerAttachmentByIdAsync(
                    inputCustomerId, inputAttachmentId);

            // then
            actualCustomerAttachment.Should().BeEquivalentTo(expectedCustomerAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerAttachmentByIdAsync(inputCustomerId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCustomerAttachmentAsync(storageCustomerAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
