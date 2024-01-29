// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOfferAttachments
{
    public partial class PromosOfferAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemovePromosOfferAttachmentAsync()
        {
            // given
            var randomPromosOfferId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputPromosOfferId = randomPromosOfferId;
            Guid inputAttachmentId = randomAttachmentId;
            PromosOfferAttachment randomPromosOfferAttachment = CreateRandomPromosOfferAttachment();
            randomPromosOfferAttachment.PromosOfferId = inputPromosOfferId;
            randomPromosOfferAttachment.AttachmentId = inputAttachmentId;
            PromosOfferAttachment storagePromosOfferAttachment = randomPromosOfferAttachment;
            PromosOfferAttachment expectedPromosOfferAttachment = storagePromosOfferAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync(inputPromosOfferId, inputAttachmentId))
                    .ReturnsAsync(storagePromosOfferAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeletePromosOfferAttachmentAsync(storagePromosOfferAttachment))
                    .ReturnsAsync(expectedPromosOfferAttachment);

            // when
            PromosOfferAttachment actualPromosOfferAttachment =
                await this.promosOfferAttachmentService.RemovePromosOfferAttachmentByIdAsync(
                    inputPromosOfferId, inputAttachmentId);

            // then
            actualPromosOfferAttachment.Should().BeEquivalentTo(expectedPromosOfferAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPromosOfferAttachmentByIdAsync(inputPromosOfferId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePromosOfferAttachmentAsync(storagePromosOfferAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
