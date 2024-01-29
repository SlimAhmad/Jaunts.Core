// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOfferAttachments
{
    public partial class PromosOfferAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllPromosOfferAttachments()
        {
            // given
            IQueryable<PromosOfferAttachment> randomPromosOfferAttachments = CreateRandomPromosOfferAttachments();
            IQueryable<PromosOfferAttachment> storagePromosOfferAttachments = randomPromosOfferAttachments;
            IQueryable<PromosOfferAttachment> expectedPromosOfferAttachments = storagePromosOfferAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPromosOfferAttachments())
                    .Returns(storagePromosOfferAttachments);

            // when
            IQueryable<PromosOfferAttachment> actualPromosOfferAttachments =
                this.promosOfferAttachmentService.RetrieveAllPromosOfferAttachments();

            // then
            actualPromosOfferAttachments.Should().BeEquivalentTo(expectedPromosOfferAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPromosOfferAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
