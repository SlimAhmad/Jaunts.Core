// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLetAttachments
{
    public partial class ShortLetAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveShortLetAttachmentAsync()
        {
            // given
            var randomShortLetId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputShortLetId = randomShortLetId;
            Guid inputAttachmentId = randomAttachmentId;
            ShortLetAttachment randomShortLetAttachment = CreateRandomShortLetAttachment();
            randomShortLetAttachment.ShortLetId = inputShortLetId;
            randomShortLetAttachment.AttachmentId = inputAttachmentId;
            ShortLetAttachment storageShortLetAttachment = randomShortLetAttachment;
            ShortLetAttachment expectedShortLetAttachment = storageShortLetAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetAttachmentByIdAsync(inputShortLetId, inputAttachmentId))
                    .ReturnsAsync(storageShortLetAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteShortLetAttachmentAsync(storageShortLetAttachment))
                    .ReturnsAsync(expectedShortLetAttachment);

            // when
            ShortLetAttachment actualShortLetAttachment =
                await this.shortLetAttachmentService.RemoveShortLetAttachmentByIdAsync(
                    inputShortLetId, inputAttachmentId);

            // then
            actualShortLetAttachment.Should().BeEquivalentTo(expectedShortLetAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetAttachmentByIdAsync(inputShortLetId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShortLetAttachmentAsync(storageShortLetAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
