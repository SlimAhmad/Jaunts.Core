// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.DriverAttachments
{
    public partial class DriverAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveDriverAttachmentByIdAsync()
        {
            // given
            DriverAttachment randomDriverAttachment = CreateRandomDriverAttachment();
            DriverAttachment storageDriverAttachment = randomDriverAttachment;
            DriverAttachment expectedDriverAttachment = storageDriverAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDriverAttachmentByIdAsync
                (randomDriverAttachment.DriverId, randomDriverAttachment.AttachmentId))
                    .ReturnsAsync(storageDriverAttachment);

            // when
            DriverAttachment actualDriverAttachment = await
                this.driverAttachmentService.RetrieveDriverAttachmentByIdAsync(
                    randomDriverAttachment.DriverId, randomDriverAttachment.AttachmentId);

            // then
            actualDriverAttachment.Should().BeEquivalentTo(expectedDriverAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDriverAttachmentByIdAsync
                (randomDriverAttachment.DriverId, randomDriverAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
