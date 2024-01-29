// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PackageAttachments
{
    public partial class PackageAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrievePackageAttachmentByIdAsync()
        {
            // given
            PackageAttachment randomPackageAttachment = CreateRandomPackageAttachment();
            PackageAttachment storagePackageAttachment = randomPackageAttachment;
            PackageAttachment expectedPackageAttachment = storagePackageAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageAttachmentByIdAsync
                (randomPackageAttachment.PackageId, randomPackageAttachment.AttachmentId))
                    .ReturnsAsync(storagePackageAttachment);

            // when
            PackageAttachment actualPackageAttachment = await
                this.packageAttachmentService.RetrievePackageAttachmentByIdAsync(
                    randomPackageAttachment.PackageId, randomPackageAttachment.AttachmentId);

            // then
            actualPackageAttachment.Should().BeEquivalentTo(expectedPackageAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageAttachmentByIdAsync
                (randomPackageAttachment.PackageId, randomPackageAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
