// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLetAttachments
{
    public partial class ShortLetAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllShortLetAttachments()
        {
            // given
            IQueryable<ShortLetAttachment> randomShortLetAttachments = CreateRandomShortLetAttachments();
            IQueryable<ShortLetAttachment> storageShortLetAttachments = randomShortLetAttachments;
            IQueryable<ShortLetAttachment> expectedShortLetAttachments = storageShortLetAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllShortLetAttachments())
                    .Returns(storageShortLetAttachments);

            // when
            IQueryable<ShortLetAttachment> actualShortLetAttachments =
                this.shortLetAttachmentService.RetrieveAllShortLetAttachments();

            // then
            actualShortLetAttachments.Should().BeEquivalentTo(expectedShortLetAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllShortLetAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
