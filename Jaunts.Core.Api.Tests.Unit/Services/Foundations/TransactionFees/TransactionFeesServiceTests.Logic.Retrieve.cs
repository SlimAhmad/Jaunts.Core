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
        async Task ShouldRetrieveTransactionFeeById()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            TransactionFee randomTransactionFee = CreateRandomTransactionFee(dateTime);
            Guid inputTransactionFeeId = randomTransactionFee.Id;
            TransactionFee inputTransactionFee = randomTransactionFee;
            TransactionFee expectedTransactionFee = randomTransactionFee;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTransactionFeeByIdAsync(inputTransactionFeeId))
                    .ReturnsAsync(inputTransactionFee);

            // when
            TransactionFee actualTransactionFee =
                await this.transactionFeeService.RetrieveTransactionFeeByIdAsync(inputTransactionFeeId);

            // then
            actualTransactionFee.Should().BeEquivalentTo(expectedTransactionFee);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTransactionFeeByIdAsync(inputTransactionFeeId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
