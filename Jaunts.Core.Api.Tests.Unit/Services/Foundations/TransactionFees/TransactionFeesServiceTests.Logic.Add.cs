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
        public async Task ShouldCreateTransactionFeeAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(dateTime);
            randomTransactionFee.UpdatedBy = randomTransactionFee.CreatedBy;
            randomTransactionFee.UpdatedDate = randomTransactionFee.CreatedDate;
            TransactionFee inputTransactionFee = randomTransactionFee;
            TransactionFee storageTransactionFee = randomTransactionFee;
            TransactionFee expectedTransactionFee = randomTransactionFee;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTransactionFeeAsync(inputTransactionFee))
                    .ReturnsAsync(storageTransactionFee);

            // when
            TransactionFee actualTransactionFee =
                await this.transactionFeeService.CreateTransactionFeeAsync(inputTransactionFee);

            // then
            actualTransactionFee.Should().BeEquivalentTo(expectedTransactionFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTransactionFeeAsync(inputTransactionFee),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
