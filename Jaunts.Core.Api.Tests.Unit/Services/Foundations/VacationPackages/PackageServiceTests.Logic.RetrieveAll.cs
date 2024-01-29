// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Packages;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Packages
{
    public partial class PackageServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllPackage()
        {
            // given
            IQueryable<Package> randomPackage = CreateRandomPackages();
            IQueryable<Package> storagePackage = randomPackage;
            IQueryable<Package> expectedPackage = storagePackage;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPackage())
                    .Returns(storagePackage);

            // when
            IQueryable<Package> actualPackage =
                this.packageService.RetrieveAllPackage();

            // then
            actualPackage.Should().BeEquivalentTo(expectedPackage);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPackage(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
