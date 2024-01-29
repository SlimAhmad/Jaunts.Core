// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Customers;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Customers
{
    public partial class CustomerServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllCustomers()
        {
            // given
            IQueryable<Customer> randomCustomers = CreateRandomCustomers();
            IQueryable<Customer> storageCustomers = randomCustomers;
            IQueryable<Customer> expectedCustomers = storageCustomers;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCustomers())
                    .Returns(storageCustomers);

            // when
            IQueryable<Customer> actualCustomers =
                this.customerService.RetrieveAllCustomers();

            // then
            actualCustomers.Should().BeEquivalentTo(expectedCustomers);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCustomers(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
