// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Providers
{
    public partial class ProviderServiceTests
    {
        [Fact]
        public async Task ShouldModifyProviderAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Provider randomProvider = CreateRandomProvider(randomInputDate);
            Provider inputProvider = randomProvider;
            Provider afterUpdateStorageProvider = inputProvider;
            Provider expectedProvider = afterUpdateStorageProvider;
            Provider beforeUpdateStorageProvider = randomProvider.DeepClone();
            inputProvider.UpdatedDate = randomDate;
            Guid ProviderId = inputProvider.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderByIdAsync(ProviderId))
                    .ReturnsAsync(beforeUpdateStorageProvider);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateProviderAsync(inputProvider))
                    .ReturnsAsync(afterUpdateStorageProvider);

            // when
            Provider actualProvider =
                await this.providerService.ModifyProviderAsync(inputProvider);

            // then
            actualProvider.Should().BeEquivalentTo(expectedProvider);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderByIdAsync(ProviderId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProviderAsync(inputProvider),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
