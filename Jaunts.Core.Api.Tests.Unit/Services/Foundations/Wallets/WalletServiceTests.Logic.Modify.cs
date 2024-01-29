// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyWalletAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Wallet randomWallet = CreateRandomWallet(randomInputDate);
            Wallet inputWallet = randomWallet;
            Wallet afterUpdateStorageWallet = inputWallet;
            Wallet expectedWallet = afterUpdateStorageWallet;
            Wallet beforeUpdateStorageWallet = randomWallet.DeepClone();
            inputWallet.UpdatedDate = randomDate;
            Guid WalletId = inputWallet.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectWalletByIdAsync(WalletId))
                    .ReturnsAsync(beforeUpdateStorageWallet);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateWalletAsync(inputWallet))
                    .ReturnsAsync(afterUpdateStorageWallet);

            // when
            Wallet actualWallet =
                await this.walletService.ModifyWalletAsync(inputWallet);

            // then
            actualWallet.Should().BeEquivalentTo(expectedWallet);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectWalletByIdAsync(WalletId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateWalletAsync(inputWallet),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
