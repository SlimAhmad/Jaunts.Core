// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLetAttachments
{
    public partial class ShortLetAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveShortLetAttachmentByIdAsync()
        {
            // given
            ShortLetAttachment randomShortLetAttachment = CreateRandomShortLetAttachment();
            ShortLetAttachment storageShortLetAttachment = randomShortLetAttachment;
            ShortLetAttachment expectedShortLetAttachment = storageShortLetAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetAttachmentByIdAsync
                (randomShortLetAttachment.ShortLetId, randomShortLetAttachment.AttachmentId))
                    .ReturnsAsync(storageShortLetAttachment);

            // when
            ShortLetAttachment actualShortLetAttachment = await
                this.shortLetAttachmentService.RetrieveShortLetAttachmentByIdAsync(
                    randomShortLetAttachment.ShortLetId, randomShortLetAttachment.AttachmentId);

            // then
            actualShortLetAttachment.Should().BeEquivalentTo(expectedShortLetAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetAttachmentByIdAsync
                (randomShortLetAttachment.ShortLetId, randomShortLetAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
