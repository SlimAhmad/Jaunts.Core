// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.CustomerAttachments
{
    public partial class CustomerAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveCustomerAttachmentByIdAsync()
        {
            // given
            CustomerAttachment randomCustomerAttachment = CreateRandomCustomerAttachment();
            CustomerAttachment storageCustomerAttachment = randomCustomerAttachment;
            CustomerAttachment expectedCustomerAttachment = storageCustomerAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerAttachmentByIdAsync
                (randomCustomerAttachment.CustomerId, randomCustomerAttachment.AttachmentId))
                    .ReturnsAsync(storageCustomerAttachment);

            // when
            CustomerAttachment actualCustomerAttachment = await
                this.customerAttachmentService.RetrieveCustomerAttachmentByIdAsync(
                    randomCustomerAttachment.CustomerId, randomCustomerAttachment.AttachmentId);

            // then
            actualCustomerAttachment.Should().BeEquivalentTo(expectedCustomerAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerAttachmentByIdAsync
                (randomCustomerAttachment.CustomerId, randomCustomerAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
