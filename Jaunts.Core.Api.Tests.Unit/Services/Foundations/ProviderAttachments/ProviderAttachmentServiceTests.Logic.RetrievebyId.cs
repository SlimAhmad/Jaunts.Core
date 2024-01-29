// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderAttachments
{
    public partial class ProviderAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveProviderAttachmentByIdAsync()
        {
            // given
            ProviderAttachment randomProviderAttachment = CreateRandomProviderAttachment();
            ProviderAttachment storageProviderAttachment = randomProviderAttachment;
            ProviderAttachment expectedProviderAttachment = storageProviderAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderAttachmentByIdAsync
                (randomProviderAttachment.ProviderId, randomProviderAttachment.AttachmentId))
                    .ReturnsAsync(storageProviderAttachment);

            // when
            ProviderAttachment actualProviderAttachment = await
                this.providerAttachmentService.RetrieveProviderAttachmentByIdAsync(
                    randomProviderAttachment.ProviderId, randomProviderAttachment.AttachmentId);

            // then
            actualProviderAttachment.Should().BeEquivalentTo(expectedProviderAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderAttachmentByIdAsync
                (randomProviderAttachment.ProviderId, randomProviderAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
