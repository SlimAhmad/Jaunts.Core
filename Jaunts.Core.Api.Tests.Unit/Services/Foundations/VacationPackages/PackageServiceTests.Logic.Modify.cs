// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyPackageAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Package randomPackage = CreateRandomPackage(randomInputDate);
            Package inputPackage = randomPackage;
            Package afterUpdateStoragePackage = inputPackage;
            Package expectedPackage = afterUpdateStoragePackage;
            Package beforeUpdateStoragePackage = randomPackage.DeepClone();
            inputPackage.UpdatedDate = randomDate;
            Guid PackageId = inputPackage.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageByIdAsync(PackageId))
                    .ReturnsAsync(beforeUpdateStoragePackage);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdatePackageAsync(inputPackage))
                    .ReturnsAsync(afterUpdateStoragePackage);

            // when
            Package actualPackage =
                await this.packageService.ModifyPackageAsync(inputPackage);

            // then
            actualPackage.Should().BeEquivalentTo(expectedPackage);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(PackageId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePackageAsync(inputPackage),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
