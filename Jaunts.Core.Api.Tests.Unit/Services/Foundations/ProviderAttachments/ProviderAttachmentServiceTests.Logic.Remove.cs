// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderAttachments
{
    public partial class ProviderAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveProviderAttachmentAsync()
        {
            // given
            var randomProviderId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputProviderId = randomProviderId;
            Guid inputAttachmentId = randomAttachmentId;
            ProviderAttachment randomProviderAttachment = CreateRandomProviderAttachment();
            randomProviderAttachment.ProviderId = inputProviderId;
            randomProviderAttachment.AttachmentId = inputAttachmentId;
            ProviderAttachment storageProviderAttachment = randomProviderAttachment;
            ProviderAttachment expectedProviderAttachment = storageProviderAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderAttachmentByIdAsync(inputProviderId, inputAttachmentId))
                    .ReturnsAsync(storageProviderAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteProviderAttachmentAsync(storageProviderAttachment))
                    .ReturnsAsync(expectedProviderAttachment);

            // when
            ProviderAttachment actualProviderAttachment =
                await this.providerAttachmentService.RemoveProviderAttachmentByIdAsync(
                    inputProviderId, inputAttachmentId);

            // then
            actualProviderAttachment.Should().BeEquivalentTo(expectedProviderAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderAttachmentByIdAsync(inputProviderId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProviderAttachmentAsync(storageProviderAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
