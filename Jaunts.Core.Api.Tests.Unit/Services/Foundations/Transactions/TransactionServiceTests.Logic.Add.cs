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
        public async Task ShouldCreateTransactionAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            Transaction randomTransaction = CreateRandomTransaction(dateTime);
            randomTransaction.UpdatedBy = randomTransaction.CreatedBy;
            randomTransaction.UpdatedDate = randomTransaction.CreatedDate;
            Transaction inputTransaction = randomTransaction;
            Transaction storageTransaction = randomTransaction;
            Transaction expectedTransaction = randomTransaction;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTransactionAsync(inputTransaction))
                    .ReturnsAsync(storageTransaction);

            // when
            Transaction actualTransaction =
                await this.transactionService.CreateTransactionAsync(inputTransaction);

            // then
            actualTransaction.Should().BeEquivalentTo(expectedTransaction);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTransactionAsync(inputTransaction),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
