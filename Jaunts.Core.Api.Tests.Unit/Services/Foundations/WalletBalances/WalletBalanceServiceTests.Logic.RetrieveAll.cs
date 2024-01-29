// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.WalletBalances
{
    public partial class WalletBalanceServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllWalletBalances()
        {
            // given
            IQueryable<WalletBalance> randomWalletBalances = CreateRandomWalletBalances();
            IQueryable<WalletBalance> storageWalletBalances = randomWalletBalances;
            IQueryable<WalletBalance> expectedWalletBalances = storageWalletBalances;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllWalletBalances())
                    .Returns(storageWalletBalances);

            // when
            IQueryable<WalletBalance> actualWalletBalances =
                this.walletBalanceService.RetrieveAllWalletBalances();

            // then
            actualWalletBalances.Should().BeEquivalentTo(expectedWalletBalances);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllWalletBalances(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
