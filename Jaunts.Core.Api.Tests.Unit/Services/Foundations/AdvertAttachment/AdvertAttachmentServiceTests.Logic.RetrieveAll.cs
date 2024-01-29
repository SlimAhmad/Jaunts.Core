// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.AdvertAttachments
{
    public partial class AdvertAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllAdvertAttachments()
        {
            // given
            IQueryable<AdvertAttachment> randomAdvertAttachments = CreateRandomAdvertAttachments();
            IQueryable<AdvertAttachment> storageAdvertAttachments = randomAdvertAttachments;
            IQueryable<AdvertAttachment> expectedAdvertAttachments = storageAdvertAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAdvertAttachments())
                    .Returns(storageAdvertAttachments);

            // when
            IQueryable<AdvertAttachment> actualAdvertAttachments =
                this.AdvertAttachmentService.RetrieveAllAdvertAttachments();

            // then
            actualAdvertAttachments.Should().BeEquivalentTo(expectedAdvertAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAdvertAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
