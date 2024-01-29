// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDealAttachments
{
    public partial class FlightDealAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllFlightDealAttachments()
        {
            // given
            IQueryable<FlightDealAttachment> randomFlightDealAttachments = CreateRandomFlightDealAttachments();
            IQueryable<FlightDealAttachment> storageFlightDealAttachments = randomFlightDealAttachments;
            IQueryable<FlightDealAttachment> expectedFlightDealAttachments = storageFlightDealAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFlightDealAttachments())
                    .Returns(storageFlightDealAttachments);

            // when
            IQueryable<FlightDealAttachment> actualFlightDealAttachments =
                this.flightDealAttachmentService.RetrieveAllFlightDealAttachments();

            // then
            actualFlightDealAttachments.Should().BeEquivalentTo(expectedFlightDealAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFlightDealAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
