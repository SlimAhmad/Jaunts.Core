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
        public async Task ShouldAddPromosOfferAttachmentAsync()
        {
            // given
            PromosOfferAttachment randomPromosOfferAttachment = CreateRandomPromosOfferAttachment();
            PromosOfferAttachment inputPromosOfferAttachment = randomPromosOfferAttachment;
            PromosOfferAttachment storagePromosOfferAttachment = randomPromosOfferAttachment;
            PromosOfferAttachment expectedPromosOfferAttachment = storagePromosOfferAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPromosOfferAttachmentAsync(inputPromosOfferAttachment))
                    .ReturnsAsync(storagePromosOfferAttachment);

            // when
            PromosOfferAttachment actualPromosOfferAttachment =
                await this.promosOfferAttachmentService.AddPromosOfferAttachmentAsync(inputPromosOfferAttachment);

            // then
            actualPromosOfferAttachment.Should().BeEquivalentTo(expectedPromosOfferAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPromosOfferAttachmentAsync(inputPromosOfferAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
