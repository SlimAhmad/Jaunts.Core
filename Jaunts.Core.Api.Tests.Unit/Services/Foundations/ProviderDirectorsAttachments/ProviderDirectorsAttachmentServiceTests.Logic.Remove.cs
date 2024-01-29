// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectorAttachments
{
    public partial class ProvidersDirectorAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveProvidersDirectorAttachmentAsync()
        {
            // given
            var randomProviderDirectorsId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputProviderDirectorsId = randomProviderDirectorsId;
            Guid inputAttachmentId = randomAttachmentId;
            ProvidersDirectorAttachment randomProvidersDirectorAttachment = CreateRandomProvidersDirectorAttachment();
            randomProvidersDirectorAttachment.ProviderDirectorId = inputProviderDirectorsId;
            randomProvidersDirectorAttachment.AttachmentId = inputAttachmentId;
            ProvidersDirectorAttachment storageProvidersDirectorAttachment = randomProvidersDirectorAttachment;
            ProvidersDirectorAttachment expectedProvidersDirectorAttachment = storageProvidersDirectorAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync(inputProviderDirectorsId, inputAttachmentId))
                    .ReturnsAsync(storageProvidersDirectorAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteProvidersDirectorAttachmentAsync(storageProvidersDirectorAttachment))
                    .ReturnsAsync(expectedProvidersDirectorAttachment);

            // when
            ProvidersDirectorAttachment actualProvidersDirectorAttachment =
                await this.providersDirectorAttachmentService.RemoveProvidersDirectorAttachmentByIdAsync(
                    inputProviderDirectorsId, inputAttachmentId);

            // then
            actualProvidersDirectorAttachment.Should().BeEquivalentTo(expectedProvidersDirectorAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorAttachmentByIdAsync(inputProviderDirectorsId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProvidersDirectorAttachmentAsync(storageProvidersDirectorAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
