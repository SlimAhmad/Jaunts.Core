// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Transactions
{
    public partial class TransactionServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllTransactions()
        {
            // given
            IQueryable<Transaction> randomTransactions = CreateRandomTransactions();
            IQueryable<Transaction> storageTransactions = randomTransactions;
            IQueryable<Transaction> expectedTransactions = storageTransactions;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTransactions())
                    .Returns(storageTransactions);

            // when
            IQueryable<Transaction> actualTransactions =
                this.transactionService.RetrieveAllTransactions();

            // then
            actualTransactions.Should().BeEquivalentTo(expectedTransactions);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTransactions(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
