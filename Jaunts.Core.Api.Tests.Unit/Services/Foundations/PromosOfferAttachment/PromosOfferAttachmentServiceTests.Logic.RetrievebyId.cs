// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOfferAttachments
{
    public partial class PromosOfferAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrievePromosOfferAttachmentByIdAsync()
        {
            // given
            PromosOfferAttachment randomPromosOfferAttachment = CreateRandomPromosOfferAttachment();
            PromosOfferAttachment storagePromosOfferAttachment = randomPromosOfferAttachment;
            PromosOfferAttachment expectedPromosOfferAttachment = storagePromosOfferAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync
                (randomPromosOfferAttachment.PromosOfferId, randomPromosOfferAttachment.AttachmentId))
                    .ReturnsAsync(storagePromosOfferAttachment);

            // when
            PromosOfferAttachment actualPromosOfferAttachment = await
                this.promosOfferAttachmentService.RetrievePromosOfferAttachmentByIdAsync(
                    randomPromosOfferAttachment.PromosOfferId, randomPromosOfferAttachment.AttachmentId);

            // then
            actualPromosOfferAttachment.Should().BeEquivalentTo(expectedPromosOfferAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync
                (randomPromosOfferAttachment.PromosOfferId, randomPromosOfferAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
