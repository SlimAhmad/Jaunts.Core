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
        public async Task ShouldAddPackageAttachmentAsync()
        {
            // given
            PackageAttachment randomPackageAttachment = CreateRandomPackageAttachment();
            PackageAttachment inputPackageAttachment = randomPackageAttachment;
            PackageAttachment storagePackageAttachment = randomPackageAttachment;
            PackageAttachment expectedPackageAttachment = storagePackageAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPackageAttachmentAsync(inputPackageAttachment))
                    .ReturnsAsync(storagePackageAttachment);

            // when
            PackageAttachment actualPackageAttachment =
                await this.packageAttachmentService.AddPackageAttachmentAsync(inputPackageAttachment);

            // then
            actualPackageAttachment.Should().BeEquivalentTo(expectedPackageAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAttachmentAsync(inputPackageAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
