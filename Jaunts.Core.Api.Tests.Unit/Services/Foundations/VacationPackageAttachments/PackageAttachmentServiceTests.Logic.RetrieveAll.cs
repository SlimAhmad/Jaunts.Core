// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PackageAttachments
{
    public partial class PackageAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllPackageAttachments()
        {
            // given
            IQueryable<PackageAttachment> randomPackageAttachments = CreateRandomPackageAttachments();
            IQueryable<PackageAttachment> storagePackageAttachments = randomPackageAttachments;
            IQueryable<PackageAttachment> expectedPackageAttachments = storagePackageAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPackageAttachments())
                    .Returns(storagePackageAttachments);

            // when
            IQueryable<PackageAttachment> actualPackageAttachments =
                this.packageAttachmentService.RetrieveAllPackageAttachments();

            // then
            actualPackageAttachments.Should().BeEquivalentTo(expectedPackageAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPackageAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
