// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.RideAttachments
{
    public partial class RideAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldAddRideAttachmentAsync()
        {
            // given
            RideAttachment randomRideAttachment = CreateRandomRideAttachment();
            RideAttachment inputRideAttachment = randomRideAttachment;
            RideAttachment storageRideAttachment = randomRideAttachment;
            RideAttachment expectedRideAttachment = storageRideAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRideAttachmentAsync(inputRideAttachment))
                    .ReturnsAsync(storageRideAttachment);

            // when
            RideAttachment actualRideAttachment =
                await this.rideAttachmentService.AddRideAttachmentAsync(inputRideAttachment);

            // then
            actualRideAttachment.Should().BeEquivalentTo(expectedRideAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRideAttachmentAsync(inputRideAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
