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
        public async Task ShouldAddProviderAttachmentAsync()
        {
            // given
            ProviderAttachment randomProviderAttachment = CreateRandomProviderAttachment();
            ProviderAttachment inputProviderAttachment = randomProviderAttachment;
            ProviderAttachment storageProviderAttachment = randomProviderAttachment;
            ProviderAttachment expectedProviderAttachment = storageProviderAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProviderAttachmentAsync(inputProviderAttachment))
                    .ReturnsAsync(storageProviderAttachment);

            // when
            ProviderAttachment actualProviderAttachment =
                await this.providerAttachmentService.AddProviderAttachmentAsync(inputProviderAttachment);

            // then
            actualProviderAttachment.Should().BeEquivalentTo(expectedProviderAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderAttachmentAsync(inputProviderAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
