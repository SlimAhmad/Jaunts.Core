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
        public async Task ShouldAddDriverAttachmentAsync()
        {
            // given
            DriverAttachment randomDriverAttachment = CreateRandomDriverAttachment();
            DriverAttachment inputDriverAttachment = randomDriverAttachment;
            DriverAttachment storageDriverAttachment = randomDriverAttachment;
            DriverAttachment expectedDriverAttachment = storageDriverAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDriverAttachmentAsync(inputDriverAttachment))
                    .ReturnsAsync(storageDriverAttachment);

            // when
            DriverAttachment actualDriverAttachment =
                await this.driverAttachmentService.AddDriverAttachmentAsync(inputDriverAttachment);

            // then
            actualDriverAttachment.Should().BeEquivalentTo(expectedDriverAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDriverAttachmentAsync(inputDriverAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
