// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.DriverAttachments
{
    public partial class DriverAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveDriverAttachmentAsync()
        {
            // given
            var randomDriverId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputDriverId = randomDriverId;
            Guid inputAttachmentId = randomAttachmentId;
            DriverAttachment randomDriverAttachment = CreateRandomDriverAttachment();
            randomDriverAttachment.DriverId = inputDriverId;
            randomDriverAttachment.AttachmentId = inputAttachmentId;
            DriverAttachment storageDriverAttachment = randomDriverAttachment;
            DriverAttachment expectedDriverAttachment = storageDriverAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverAttachmentByIdAsync(inputDriverId, inputAttachmentId))
                    .ReturnsAsync(storageDriverAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteDriverAttachmentAsync(storageDriverAttachment))
                    .ReturnsAsync(expectedDriverAttachment);

            // when
            DriverAttachment actualDriverAttachment =
                await this.driverAttachmentService.RemoveDriverAttachmentByIdAsync(
                    inputDriverId, inputAttachmentId);

            // then
            actualDriverAttachment.Should().BeEquivalentTo(expectedDriverAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverAttachmentByIdAsync(inputDriverId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDriverAttachmentAsync(storageDriverAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
