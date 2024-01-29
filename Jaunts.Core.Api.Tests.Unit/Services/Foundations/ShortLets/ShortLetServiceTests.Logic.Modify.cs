// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLets
{
    public partial class ShortLetServiceTests
    {
        [Fact]
        public async Task ShouldModifyShortLetAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            ShortLet randomShortLet = CreateRandomShortLet(randomInputDate);
            ShortLet inputShortLet = randomShortLet;
            ShortLet afterUpdateStorageShortLet = inputShortLet;
            ShortLet expectedShortLet = afterUpdateStorageShortLet;
            ShortLet beforeUpdateStorageShortLet = randomShortLet.DeepClone();
            inputShortLet.UpdatedDate = randomDate;
            Guid ShortLetId = inputShortLet.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetByIdAsync(ShortLetId))
                    .ReturnsAsync(beforeUpdateStorageShortLet);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateShortLetAsync(inputShortLet))
                    .ReturnsAsync(afterUpdateStorageShortLet);

            // when
            ShortLet actualShortLet =
                await this.shortLetService.ModifyShortLetAsync(inputShortLet);

            // then
            actualShortLet.Should().BeEquivalentTo(expectedShortLet);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(ShortLetId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShortLetAsync(inputShortLet),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
