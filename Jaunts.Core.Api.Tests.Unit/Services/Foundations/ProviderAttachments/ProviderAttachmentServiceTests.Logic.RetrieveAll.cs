// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderAttachments
{
    public partial class ProviderAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllProviderAttachments()
        {
            // given
            IQueryable<ProviderAttachment> randomProviderAttachments = CreateRandomProviderAttachments();
            IQueryable<ProviderAttachment> storageProviderAttachments = randomProviderAttachments;
            IQueryable<ProviderAttachment> expectedProviderAttachments = storageProviderAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProviderAttachments())
                    .Returns(storageProviderAttachments);

            // when
            IQueryable<ProviderAttachment> actualProviderAttachments =
                this.providerAttachmentService.RetrieveAllProviderAttachments();

            // then
            actualProviderAttachments.Should().BeEquivalentTo(expectedProviderAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProviderAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
