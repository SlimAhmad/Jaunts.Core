// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.AdvertAttachments
{
    public partial class AdvertAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveAdvertAttachmentAsync()
        {
            // given
            var randomAdvertId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputAdvertId = randomAdvertId;
            Guid inputAttachmentId = randomAttachmentId;
            AdvertAttachment randomAdvertAttachment = CreateRandomAdvertAttachment();
            randomAdvertAttachment.AdvertId = inputAdvertId;
            randomAdvertAttachment.AttachmentId = inputAttachmentId;
            AdvertAttachment storageAdvertAttachment = randomAdvertAttachment;
            AdvertAttachment expectedAdvertAttachment = storageAdvertAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAdvertAttachmentByIdAsync(inputAdvertId, inputAttachmentId))
                    .ReturnsAsync(storageAdvertAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAdvertAttachmentAsync(storageAdvertAttachment))
                    .ReturnsAsync(expectedAdvertAttachment);

            // when
            AdvertAttachment actualAdvertAttachment =
                await this.AdvertAttachmentService.RemoveAdvertAttachmentByIdAsync(
                    inputAdvertId, inputAttachmentId);

            // then
            actualAdvertAttachment.Should().BeEquivalentTo(expectedAdvertAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertAttachmentByIdAsync(inputAdvertId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAdvertAttachmentAsync(storageAdvertAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
