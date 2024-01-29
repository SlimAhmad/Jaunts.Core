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
        async Task ShouldRetrieveCustomerById()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Customer randomCustomer = CreateRandomCustomer(dateTime);
            Guid inputCustomerId = randomCustomer.Id;
            Customer inputCustomer = randomCustomer;
            Customer expectedCustomer = randomCustomer;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerByIdAsync(inputCustomerId))
                    .ReturnsAsync(inputCustomer);

            // when
            Customer actualCustomer =
                await this.customerService.RetrieveCustomerByIdAsync(inputCustomerId);

            // then
            actualCustomer.Should().BeEquivalentTo(expectedCustomer);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(inputCustomerId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
