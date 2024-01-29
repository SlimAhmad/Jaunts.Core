// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.CustomerAttachments
{
    public partial class CustomerAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllCustomerAttachments()
        {
            // given
            IQueryable<CustomerAttachment> randomCustomerAttachments = CreateRandomCustomerAttachments();
            IQueryable<CustomerAttachment> storageCustomerAttachments = randomCustomerAttachments;
            IQueryable<CustomerAttachment> expectedCustomerAttachments = storageCustomerAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCustomerAttachments())
                    .Returns(storageCustomerAttachments);

            // when
            IQueryable<CustomerAttachment> actualCustomerAttachments =
                this.customerAttachmentService.RetrieveAllCustomerAttachments();

            // then
            actualCustomerAttachments.Should().BeEquivalentTo(expectedCustomerAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCustomerAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
