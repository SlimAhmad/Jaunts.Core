// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyTransactionFeeAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(randomInputDate);
            TransactionFee inputTransactionFee = randomTransactionFee;
            TransactionFee afterUpdateStorageTransactionFee = inputTransactionFee;
            TransactionFee expectedTransactionFee = afterUpdateStorageTransactionFee;
            TransactionFee beforeUpdateStorageTransactionFee = randomTransactionFee.DeepClone();
            inputTransactionFee.UpdatedDate = randomDate;
            Guid TransactionFeeId = inputTransactionFee.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionFeeByIdAsync(TransactionFeeId))
                    .ReturnsAsync(beforeUpdateStorageTransactionFee);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTransactionFeeAsync(inputTransactionFee))
                    .ReturnsAsync(afterUpdateStorageTransactionFee);

            // when
            TransactionFee actualTransactionFee =
                await this.transactionFeeService.ModifyTransactionFeeAsync(inputTransactionFee);

            // then
            actualTransactionFee.Should().BeEquivalentTo(expectedTransactionFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(TransactionFeeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTransactionFeeAsync(inputTransactionFee),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
