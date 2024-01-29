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
        public async Task ShouldCreatePackageAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            Package randomPackage = CreateRandomPackage(dateTime);
            randomPackage.UpdatedBy = randomPackage.CreatedBy;
            randomPackage.UpdatedDate = randomPackage.CreatedDate;
            Package inputPackage = randomPackage;
            Package storagePackage = randomPackage;
            Package expectedPackage = randomPackage;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPackageAsync(inputPackage))
                    .ReturnsAsync(storagePackage);

            // when
            Package actualPackage =
                await this.packageService.CreatePackageAsync(inputPackage);

            // then
            actualPackage.Should().BeEquivalentTo(expectedPackage);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPackageAsync(inputPackage),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
