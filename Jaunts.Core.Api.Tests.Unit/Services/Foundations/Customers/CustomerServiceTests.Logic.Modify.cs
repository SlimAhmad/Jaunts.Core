// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyCustomerAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Customer randomCustomer = CreateRandomCustomer(randomInputDate);
            Customer inputCustomer = randomCustomer;
            Customer afterUpdateStorageCustomer = inputCustomer;
            Customer expectedCustomer = afterUpdateStorageCustomer;
            Customer beforeUpdateStorageCustomer = randomCustomer.DeepClone();
            inputCustomer.UpdatedDate = randomDate;
            Guid CustomerId = inputCustomer.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCustomerByIdAsync(CustomerId))
                    .ReturnsAsync(beforeUpdateStorageCustomer);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateCustomerAsync(inputCustomer))
                    .ReturnsAsync(afterUpdateStorageCustomer);

            // when
            Customer actualCustomer =
                await this.customerService.ModifyCustomerAsync(inputCustomer);

            // then
            actualCustomer.Should().BeEquivalentTo(expectedCustomer);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCustomerByIdAsync(CustomerId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCustomerAsync(inputCustomer),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
