// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Wallets
{
    public partial class WalletServiceTests
    {
        [Fact]
        public async Task ShouldCreateWalletAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            Wallet randomWallet = CreateRandomWallet(dateTime);
            randomWallet.UpdatedBy = randomWallet.CreatedBy;
            randomWallet.UpdatedDate = randomWallet.CreatedDate;
            Wallet inputWallet = randomWallet;
            Wallet storageWallet = randomWallet;
            Wallet expectedWallet = randomWallet;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertWalletAsync(inputWallet))
                    .ReturnsAsync(storageWallet);

            // when
            Wallet actualWallet =
                await this.walletService.CreateWalletAsync(inputWallet);

            // then
            actualWallet.Should().BeEquivalentTo(expectedWallet);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertWalletAsync(inputWallet),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
