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
        async Task ShouldRetrievePackageById()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Package randomPackage = CreateRandomPackage(dateTime);
            Guid inputPackageId = randomPackage.Id;
            Package inputPackage = randomPackage;
            Package expectedPackage = randomPackage;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPackageByIdAsync(inputPackageId))
                    .ReturnsAsync(inputPackage);

            // when
            Package actualPackage =
                await this.packageService.RetrievePackageByIdAsync(inputPackageId);

            // then
            actualPackage.Should().BeEquivalentTo(expectedPackage);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageByIdAsync(inputPackageId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
