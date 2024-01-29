// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyWalletBalanceAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            WalletBalance randomWalletBalance = CreateRandomWalletBalance(randomInputDate);
            WalletBalance inputWalletBalance = randomWalletBalance;
            WalletBalance afterUpdateStorageWalletBalance = inputWalletBalance;
            WalletBalance expectedWalletBalance = afterUpdateStorageWalletBalance;
            WalletBalance beforeUpdateStorageWalletBalance = randomWalletBalance.DeepClone();
            inputWalletBalance.UpdatedDate = randomDate;
            Guid WalletBalanceId = inputWalletBalance.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletBalanceByIdAsync(WalletBalanceId))
                    .ReturnsAsync(beforeUpdateStorageWalletBalance);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateWalletBalanceAsync(inputWalletBalance))
                    .ReturnsAsync(afterUpdateStorageWalletBalance);

            // when
            WalletBalance actualWalletBalance =
                await this.walletBalanceService.ModifyWalletBalanceAsync(inputWalletBalance);

            // then
            actualWalletBalance.Should().BeEquivalentTo(expectedWalletBalance);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletBalanceByIdAsync(WalletBalanceId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletBalanceAsync(inputWalletBalance),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
