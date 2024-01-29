// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.TransactionFees
{
    public partial class TransactionFeeServiceTests
    {
        [Fact]
        public async Task ShouldDeleteTransactionFeeAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(dateTime);
            Guid inputTransactionFeeId = randomTransactionFee.Id;
            TransactionFee inputTransactionFee = randomTransactionFee;
            TransactionFee storageTransactionFee = randomTransactionFee;
            TransactionFee expectedTransactionFee = randomTransactionFee;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionFeeByIdAsync(inputTransactionFeeId))
                    .ReturnsAsync(inputTransactionFee);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTransactionFeeAsync(inputTransactionFee))
                    .ReturnsAsync(storageTransactionFee);

            // when
            TransactionFee actualTransactionFee =
                await this.transactionFeeService.RemoveTransactionFeeByIdAsync(inputTransactionFeeId);

            // then
            actualTransactionFee.Should().BeEquivalentTo(expectedTransactionFee);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(inputTransactionFeeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTransactionFeeAsync(inputTransactionFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
