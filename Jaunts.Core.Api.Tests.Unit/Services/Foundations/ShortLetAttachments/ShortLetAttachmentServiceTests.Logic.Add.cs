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
        public async Task ShouldAddShortLetAttachmentAsync()
        {
            // given
            ShortLetAttachment randomShortLetAttachment = CreateRandomShortLetAttachment();
            ShortLetAttachment inputShortLetAttachment = randomShortLetAttachment;
            ShortLetAttachment storageShortLetAttachment = randomShortLetAttachment;
            ShortLetAttachment expectedShortLetAttachment = storageShortLetAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertShortLetAttachmentAsync(inputShortLetAttachment))
                    .ReturnsAsync(storageShortLetAttachment);

            // when
            ShortLetAttachment actualShortLetAttachment =
                await this.shortLetAttachmentService.AddShortLetAttachmentAsync(inputShortLetAttachment);

            // then
            actualShortLetAttachment.Should().BeEquivalentTo(expectedShortLetAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAttachmentAsync(inputShortLetAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
