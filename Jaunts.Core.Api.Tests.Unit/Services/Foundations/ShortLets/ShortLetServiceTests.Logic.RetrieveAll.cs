// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLets
{
    public partial class ShortLetServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllShortLets()
        {
            // given
            IQueryable<ShortLet> randomShortLets = CreateRandomShortLets();
            IQueryable<ShortLet> storageShortLets = randomShortLets;
            IQueryable<ShortLet> expectedShortLets = storageShortLets;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllShortLets())
                    .Returns(storageShortLets);

            // when
            IQueryable<ShortLet> actualShortLets =
                this.shortLetService.RetrieveAllShortLets();

            // then
            actualShortLets.Should().BeEquivalentTo(expectedShortLets);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllShortLets(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
