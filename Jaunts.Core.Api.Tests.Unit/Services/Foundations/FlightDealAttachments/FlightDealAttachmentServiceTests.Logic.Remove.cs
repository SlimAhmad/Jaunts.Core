// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDealAttachments
{
    public partial class FlightDealAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveFlightDealAttachmentAsync()
        {
            // given
            var randomFlightDealId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputFlightDealId = randomFlightDealId;
            Guid inputAttachmentId = randomAttachmentId;
            FlightDealAttachment randomFlightDealAttachment = CreateRandomFlightDealAttachment();
            randomFlightDealAttachment.FlightDealId = inputFlightDealId;
            randomFlightDealAttachment.AttachmentId = inputAttachmentId;
            FlightDealAttachment storageFlightDealAttachment = randomFlightDealAttachment;
            FlightDealAttachment expectedFlightDealAttachment = storageFlightDealAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealAttachmentByIdAsync(inputFlightDealId, inputAttachmentId))
                    .ReturnsAsync(storageFlightDealAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteFlightDealAttachmentAsync(storageFlightDealAttachment))
                    .ReturnsAsync(expectedFlightDealAttachment);

            // when
            FlightDealAttachment actualFlightDealAttachment =
                await this.flightDealAttachmentService.RemoveFlightDealAttachmentByIdAsync(
                    inputFlightDealId, inputAttachmentId);

            // then
            actualFlightDealAttachment.Should().BeEquivalentTo(expectedFlightDealAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealAttachmentByIdAsync(inputFlightDealId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFlightDealAttachmentAsync(storageFlightDealAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
