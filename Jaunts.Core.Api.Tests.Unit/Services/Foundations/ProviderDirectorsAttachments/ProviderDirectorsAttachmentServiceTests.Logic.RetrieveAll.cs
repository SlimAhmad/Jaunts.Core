// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectorAttachments
{
    public partial class ProvidersDirectorAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllProvidersDirectorAttachments()
        {
            // given
            IQueryable<ProvidersDirectorAttachment> randomProvidersDirectorAttachments = CreateRandomProvidersDirectorAttachments();
            IQueryable<ProvidersDirectorAttachment> storageProvidersDirectorAttachments = randomProvidersDirectorAttachments;
            IQueryable<ProvidersDirectorAttachment> expectedProvidersDirectorAttachments = storageProvidersDirectorAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProvidersDirectorAttachments())
                    .Returns(storageProvidersDirectorAttachments);

            // when
            IQueryable<ProvidersDirectorAttachment> actualProvidersDirectorAttachments =
                this.providersDirectorAttachmentService.RetrieveAllProvidersDirectorAttachments();

            // then
            actualProvidersDirectorAttachments.Should().BeEquivalentTo(expectedProvidersDirectorAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProvidersDirectorAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
