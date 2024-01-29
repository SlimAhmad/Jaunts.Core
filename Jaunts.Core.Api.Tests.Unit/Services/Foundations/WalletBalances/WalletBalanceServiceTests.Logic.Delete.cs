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
        public async Task ShouldDeleteWalletBalanceAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            WalletBalance randomWalletBalance = CreateRandomWalletBalance(dateTime);
            Guid inputWalletBalanceId = randomWalletBalance.Id;
            WalletBalance inputWalletBalance = randomWalletBalance;
            WalletBalance storageWalletBalance = randomWalletBalance;
            WalletBalance expectedWalletBalance = randomWalletBalance;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletBalanceByIdAsync(inputWalletBalanceId))
                    .ReturnsAsync(inputWalletBalance);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteWalletBalanceAsync(inputWalletBalance))
                    .ReturnsAsync(storageWalletBalance);

            // when
            WalletBalance actualWalletBalance =
                await this.walletBalanceService.RemoveWalletBalanceByIdAsync(inputWalletBalanceId);

            // then
            actualWalletBalance.Should().BeEquivalentTo(expectedWalletBalance);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(inputWalletBalanceId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteWalletBalanceAsync(inputWalletBalance),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
