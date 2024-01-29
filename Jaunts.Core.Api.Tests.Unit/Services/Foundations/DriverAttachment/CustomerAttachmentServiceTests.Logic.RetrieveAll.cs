// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.DriverAttachments
{
    public partial class DriverAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllDriverAttachments()
        {
            // given
            IQueryable<DriverAttachment> randomDriverAttachments = CreateRandomDriverAttachments();
            IQueryable<DriverAttachment> storageDriverAttachments = randomDriverAttachments;
            IQueryable<DriverAttachment> expectedDriverAttachments = storageDriverAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDriverAttachments())
                    .Returns(storageDriverAttachments);

            // when
            IQueryable<DriverAttachment> actualDriverAttachments =
                this.driverAttachmentService.RetrieveAllDriverAttachments();

            // then
            actualDriverAttachments.Should().BeEquivalentTo(expectedDriverAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDriverAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
