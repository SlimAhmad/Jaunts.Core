// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.WalletBalances
{
    public partial class WalletBalanceServiceTests
    {
        [Fact]
        async Task ShouldRetrieveWalletBalanceById()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            WalletBalance randomWalletBalance = CreateRandomWalletBalance(dateTime);
            Guid inputWalletBalanceId = randomWalletBalance.Id;
            WalletBalance inputWalletBalance = randomWalletBalance;
            WalletBalance expectedWalletBalance = randomWalletBalance;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletBalanceByIdAsync(inputWalletBalanceId))
                    .ReturnsAsync(inputWalletBalance);

            // when
            WalletBalance actualWalletBalance =
                await this.walletBalanceService.RetrieveWalletBalanceByIdAsync(inputWalletBalanceId);

            // then
            actualWalletBalance.Should().BeEquivalentTo(expectedWalletBalance);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(inputWalletBalanceId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
