// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectorAttachments
{
    public partial class ProvidersDirectorAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveProvidersDirectorAttachmentByIdAsync()
        {
            // given
            ProvidersDirectorAttachment randomProvidersDirectorAttachment = CreateRandomProvidersDirectorAttachment();
            ProvidersDirectorAttachment storageProvidersDirectorAttachment = randomProvidersDirectorAttachment;
            ProvidersDirectorAttachment expectedProvidersDirectorAttachment = storageProvidersDirectorAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync
                (randomProvidersDirectorAttachment.ProviderDirectorId, randomProvidersDirectorAttachment.AttachmentId))
                    .ReturnsAsync(storageProvidersDirectorAttachment);

            // when
            ProvidersDirectorAttachment actualProvidersDirectorAttachment = await
                this.providersDirectorAttachmentService.RetrieveProvidersDirectorAttachmentByIdAsync(
                    randomProvidersDirectorAttachment.ProviderDirectorId, randomProvidersDirectorAttachment.AttachmentId);

            // then
            actualProvidersDirectorAttachment.Should().BeEquivalentTo(expectedProvidersDirectorAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync
                (randomProvidersDirectorAttachment.ProviderDirectorId, randomProvidersDirectorAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
