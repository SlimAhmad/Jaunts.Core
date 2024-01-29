// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Transactions
{
    public partial class TransactionServiceTests
    {
        [Fact]
        public async Task ShouldDeleteTransactionAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Transaction randomTransaction = CreateRandomTransaction(dateTime);
            Guid inputTransactionId = randomTransaction.Id;
            Transaction inputTransaction = randomTransaction;
            Transaction storageTransaction = randomTransaction;
            Transaction expectedTransaction = randomTransaction;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionByIdAsync(inputTransactionId))
                    .ReturnsAsync(inputTransaction);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTransactionAsync(inputTransaction))
                    .ReturnsAsync(storageTransaction);

            // when
            Transaction actualTransaction =
                await this.transactionService.RemoveTransactionByIdAsync(inputTransactionId);

            // then
            actualTransaction.Should().BeEquivalentTo(expectedTransaction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(inputTransactionId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTransactionAsync(inputTransaction),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
