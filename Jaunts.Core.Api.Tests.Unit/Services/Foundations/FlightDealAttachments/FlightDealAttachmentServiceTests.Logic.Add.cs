﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDealAttachments
{
    public partial class FlightDealAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldAddFlightDealAttachmentAsync()
        {
            // given
            FlightDealAttachment randomFlightDealAttachment = CreateRandomFlightDealAttachment();
            FlightDealAttachment inputFlightDealAttachment = randomFlightDealAttachment;
            FlightDealAttachment storageFlightDealAttachment = randomFlightDealAttachment;
            FlightDealAttachment expectedFlightDealAttachment = storageFlightDealAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertFlightDealAttachmentAsync(inputFlightDealAttachment))
                    .ReturnsAsync(storageFlightDealAttachment);

            // when
            FlightDealAttachment actualFlightDealAttachment =
                await this.flightDealAttachmentService.AddFlightDealAttachmentAsync(inputFlightDealAttachment);

            // then
            actualFlightDealAttachment.Should().BeEquivalentTo(expectedFlightDealAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFlightDealAttachmentAsync(inputFlightDealAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
