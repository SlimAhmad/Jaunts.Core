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
        public async Task ShouldCreateWalletBalanceAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            WalletBalance randomWalletBalance = CreateRandomWalletBalance(dateTime);
            randomWalletBalance.UpdatedBy = randomWalletBalance.CreatedBy;
            randomWalletBalance.UpdatedDate = randomWalletBalance.CreatedDate;
            WalletBalance inputWalletBalance = randomWalletBalance;
            WalletBalance storageWalletBalance = randomWalletBalance;
            WalletBalance expectedWalletBalance = randomWalletBalance;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertWalletBalanceAsync(inputWalletBalance))
                    .ReturnsAsync(storageWalletBalance);

            // when
            WalletBalance actualWalletBalance =
                await this.walletBalanceService.CreateWalletBalanceAsync(inputWalletBalance);

            // then
            actualWalletBalance.Should().BeEquivalentTo(expectedWalletBalance);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertWalletBalanceAsync(inputWalletBalance),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
