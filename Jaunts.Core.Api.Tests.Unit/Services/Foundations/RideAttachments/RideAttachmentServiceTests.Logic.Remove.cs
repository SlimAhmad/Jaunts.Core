// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.RideAttachments
{
    public partial class RideAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveRideAttachmentAsync()
        {
            // given
            var randomRideId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputRideId = randomRideId;
            Guid inputAttachmentId = randomAttachmentId;
            RideAttachment randomRideAttachment = CreateRandomRideAttachment();
            randomRideAttachment.RideId = inputRideId;
            randomRideAttachment.AttachmentId = inputAttachmentId;
            RideAttachment storageRideAttachment = randomRideAttachment;
            RideAttachment expectedRideAttachment = storageRideAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRideAttachmentByIdAsync(inputRideId, inputAttachmentId))
                    .ReturnsAsync(storageRideAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteRideAttachmentAsync(storageRideAttachment))
                    .ReturnsAsync(expectedRideAttachment);

            // when
            RideAttachment actualRideAttachment =
                await this.rideAttachmentService.RemoveRideAttachmentByIdAsync(
                    inputRideId, inputAttachmentId);

            // then
            actualRideAttachment.Should().BeEquivalentTo(expectedRideAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRideAttachmentByIdAsync(inputRideId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRideAttachmentAsync(storageRideAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
