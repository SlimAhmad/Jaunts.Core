// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.TransactionFees
{
    public partial class TransactionFeeServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllTransactionFees()
        {
            // given
            IQueryable<TransactionFee> randomTransactionFees = CreateRandomTransactionFees();
            IQueryable<TransactionFee> storageTransactionFees = randomTransactionFees;
            IQueryable<TransactionFee> expectedTransactionFees = storageTransactionFees;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTransactionFees())
                    .Returns(storageTransactionFees);

            // when
            IQueryable<TransactionFee> actualTransactionFees =
                this.transactionFeeService.RetrieveAllTransactionFees();

            // then
            actualTransactionFees.Should().BeEquivalentTo(expectedTransactionFees);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTransactionFees(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
