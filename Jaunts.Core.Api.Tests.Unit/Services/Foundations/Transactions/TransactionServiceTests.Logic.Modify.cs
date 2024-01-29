// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyTransactionAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Transaction randomTransaction = CreateRandomTransaction(randomInputDate);
            Transaction inputTransaction = randomTransaction;
            Transaction afterUpdateStorageTransaction = inputTransaction;
            Transaction expectedTransaction = afterUpdateStorageTransaction;
            Transaction beforeUpdateStorageTransaction = randomTransaction.DeepClone();
            inputTransaction.UpdatedDate = randomDate;
            Guid TransactionId = inputTransaction.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionByIdAsync(TransactionId))
                    .ReturnsAsync(beforeUpdateStorageTransaction);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTransactionAsync(inputTransaction))
                    .ReturnsAsync(afterUpdateStorageTransaction);

            // when
            Transaction actualTransaction =
                await this.transactionService.ModifyTransactionAsync(inputTransaction);

            // then
            actualTransaction.Should().BeEquivalentTo(expectedTransaction);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionByIdAsync(TransactionId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionAsync(inputTransaction),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
