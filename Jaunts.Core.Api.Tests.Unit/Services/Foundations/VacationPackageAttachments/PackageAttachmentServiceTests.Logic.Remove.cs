// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PackageAttachments
{
    public partial class PackageAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemovePackageAttachmentAsync()
        {
            // given
            var randomPackageId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputPackageId = randomPackageId;
            Guid inputAttachmentId = randomAttachmentId;
            PackageAttachment randomPackageAttachment = CreateRandomPackageAttachment();
            randomPackageAttachment.PackageId = inputPackageId;
            randomPackageAttachment.AttachmentId = inputAttachmentId;
            PackageAttachment storagePackageAttachment = randomPackageAttachment;
            PackageAttachment expectedPackageAttachment = storagePackageAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageAttachmentByIdAsync(inputPackageId, inputAttachmentId))
                    .ReturnsAsync(storagePackageAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeletePackageAttachmentAsync(storagePackageAttachment))
                    .ReturnsAsync(expectedPackageAttachment);

            // when
            PackageAttachment actualPackageAttachment =
                await this.packageAttachmentService.RemovePackageAttachmentByIdAsync(
                    inputPackageId, inputAttachmentId);

            // then
            actualPackageAttachment.Should().BeEquivalentTo(expectedPackageAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageAttachmentByIdAsync(inputPackageId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePackageAttachmentAsync(storagePackageAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
