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
        public async Task ShouldDeleteWalletAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Wallet randomWallet = CreateRandomWallet(dateTime);
            Guid inputWalletId = randomWallet.Id;
            Wallet inputWallet = randomWallet;
            Wallet storageWallet = randomWallet;
            Wallet expectedWallet = randomWallet;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletByIdAsync(inputWalletId))
                    .ReturnsAsync(inputWallet);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteWalletAsync(inputWallet))
                    .ReturnsAsync(storageWallet);

            // when
            Wallet actualWallet =
                await this.walletService.RemoveWalletByIdAsync(inputWalletId);

            // then
            actualWallet.Should().BeEquivalentTo(expectedWallet);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(inputWalletId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteWalletAsync(inputWallet),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
