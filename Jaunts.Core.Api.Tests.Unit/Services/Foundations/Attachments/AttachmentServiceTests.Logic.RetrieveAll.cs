// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {

        [Fact]
        public void ShouldRetrieveAllAttachments()
        {
            // given
            IQueryable<Attachment> randomAttachments = CreateRandomAttachments();
            IQueryable<Attachment> storageAttachments = randomAttachments;
            IQueryable<Attachment> expectedAttachments = storageAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAttachments())
                    .Returns(storageAttachments);

            // when
            IQueryable<Attachment> actualAttachments =
                this.attachmentService.RetrieveAllAttachments();

            // then
            actualAttachments.Should().BeEquivalentTo(expectedAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
