// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Wallets
{
    public partial class WalletServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllWallets()
        {
            // given
            IQueryable<Wallet> randomWallets = CreateRandomWallets();
            IQueryable<Wallet> storageWallets = randomWallets;
            IQueryable<Wallet> expectedWallets = storageWallets;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllWallets())
                    .Returns(storageWallets);

            // when
            IQueryable<Wallet> actualWallets =
                this.walletService.RetrieveAllWallets();

            // then
            actualWallets.Should().BeEquivalentTo(expectedWallets);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllWallets(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
