// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Customers;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Customers
{
    public partial class CustomerServiceTests
    {
        [Fact]
        public async Task ShouldDeleteCustomerAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Customer randomCustomer = CreateRandomCustomer(dateTime);
            Guid inputCustomerId = randomCustomer.Id;
            Customer inputCustomer = randomCustomer;
            Customer storageCustomer = randomCustomer;
            Customer expectedCustomer = randomCustomer;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerByIdAsync(inputCustomerId))
                    .ReturnsAsync(inputCustomer);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteCustomerAsync(inputCustomer))
                    .ReturnsAsync(storageCustomer);

            // when
            Customer actualCustomer =
                await this.customerService.RemoveCustomerByIdAsync(inputCustomerId);

            // then
            actualCustomer.Should().BeEquivalentTo(expectedCustomer);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(inputCustomerId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCustomerAsync(inputCustomer),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
