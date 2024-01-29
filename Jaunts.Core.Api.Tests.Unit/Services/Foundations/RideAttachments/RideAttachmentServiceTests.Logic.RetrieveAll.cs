// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.RideAttachments
{
    public partial class RideAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllRideAttachments()
        {
            // given
            IQueryable<RideAttachment> randomRideAttachments = CreateRandomRideAttachments();
            IQueryable<RideAttachment> storageRideAttachments = randomRideAttachments;
            IQueryable<RideAttachment> expectedRideAttachments = storageRideAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRideAttachments())
                    .Returns(storageRideAttachments);

            // when
            IQueryable<RideAttachment> actualRideAttachments =
                this.rideAttachmentService.RetrieveAllRideAttachments();

            // then
            actualRideAttachments.Should().BeEquivalentTo(expectedRideAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRideAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
