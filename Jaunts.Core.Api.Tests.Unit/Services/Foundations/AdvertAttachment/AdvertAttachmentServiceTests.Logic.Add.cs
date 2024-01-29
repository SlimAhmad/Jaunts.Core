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
        public async Task ShouldAddAdvertAttachmentAsync()
        {
            // given
            AdvertAttachment randomAdvertAttachment = CreateRandomAdvertAttachment();
            AdvertAttachment inputAdvertAttachment = randomAdvertAttachment;
            AdvertAttachment storageAdvertAttachment = randomAdvertAttachment;
            AdvertAttachment expectedAdvertAttachment = storageAdvertAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAdvertAttachmentAsync(inputAdvertAttachment))
                    .ReturnsAsync(storageAdvertAttachment);

            // when
            AdvertAttachment actualAdvertAttachment =
                await this.AdvertAttachmentService.AddAdvertAttachmentAsync(inputAdvertAttachment);

            // then
            actualAdvertAttachment.Should().BeEquivalentTo(expectedAdvertAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAdvertAttachmentAsync(inputAdvertAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
