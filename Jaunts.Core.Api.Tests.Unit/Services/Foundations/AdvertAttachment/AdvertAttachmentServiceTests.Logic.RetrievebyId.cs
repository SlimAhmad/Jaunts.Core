// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.AdvertAttachments
{
    public partial class AdvertAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAdvertAttachmentByIdAsync()
        {
            // given
            AdvertAttachment randomAdvertAttachment = CreateRandomAdvertAttachment();
            AdvertAttachment storageAdvertAttachment = randomAdvertAttachment;
            AdvertAttachment expectedAdvertAttachment = storageAdvertAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertAttachmentByIdAsync
                (randomAdvertAttachment.AdvertId, randomAdvertAttachment.AttachmentId))
                    .ReturnsAsync(storageAdvertAttachment);

            // when
            AdvertAttachment actualAdvertAttachment = await
                this.AdvertAttachmentService.RetrieveAdvertAttachmentByIdAsync(
                    randomAdvertAttachment.AdvertId, randomAdvertAttachment.AttachmentId);

            // then
            actualAdvertAttachment.Should().BeEquivalentTo(expectedAdvertAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertAttachmentByIdAsync
                (randomAdvertAttachment.AdvertId, randomAdvertAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
