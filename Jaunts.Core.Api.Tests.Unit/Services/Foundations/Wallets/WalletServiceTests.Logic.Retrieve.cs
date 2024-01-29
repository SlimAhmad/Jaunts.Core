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
        async Task ShouldRetrieveWalletById()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Wallet randomWallet = CreateRandomWallet(dateTime);
            Guid inputWalletId = randomWallet.Id;
            Wallet inputWallet = randomWallet;
            Wallet expectedWallet = randomWallet;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletByIdAsync(inputWalletId))
                    .ReturnsAsync(inputWallet);

            // when
            Wallet actualWallet =
                await this.walletService.RetrieveWalletByIdAsync(inputWalletId);

            // then
            actualWallet.Should().BeEquivalentTo(expectedWallet);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(inputWalletId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
