// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Packages;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Packages
{
    public partial class PackageServiceTests
    {
        [Fact]
        public async Task ShouldDeletePackageAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Package randomPackage = CreateRandomPackage(dateTime);
            Guid inputPackageId = randomPackage.Id;
            Package inputPackage = randomPackage;
            Package storagePackage = randomPackage;
            Package expectedPackage = randomPackage;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageByIdAsync(inputPackageId))
                    .ReturnsAsync(inputPackage);

            this.storageBrokerMock.Setup(broker =>
                broker.DeletePackageAsync(inputPackage))
                    .ReturnsAsync(storagePackage);

            // when
            Package actualPackage =
                await this.packageService.RemovePackageByIdAsync(inputPackageId);

            // then
            actualPackage.Should().BeEquivalentTo(expectedPackage);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(inputPackageId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePackageAsync(inputPackage),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
