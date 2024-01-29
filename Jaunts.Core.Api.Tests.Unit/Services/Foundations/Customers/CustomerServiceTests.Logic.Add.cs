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
        public async Task ShouldCreateCustomerAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            Customer randomCustomer = CreateRandomCustomer(dateTime);
            randomCustomer.UpdatedBy = randomCustomer.CreatedBy;
            randomCustomer.UpdatedDate = randomCustomer.CreatedDate;
            Customer inputCustomer = randomCustomer;
            Customer storageCustomer = randomCustomer;
            Customer expectedCustomer = randomCustomer;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCustomerAsync(inputCustomer))
                    .ReturnsAsync(storageCustomer);

            // when
            Customer actualCustomer =
                await this.customerService.CreateCustomerAsync(inputCustomer);

            // then
            actualCustomer.Should().BeEquivalentTo(expectedCustomer);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCustomerAsync(inputCustomer),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
