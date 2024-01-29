// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
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
        public async Task ShouldCreateShortLetAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            ShortLet randomShortLet = CreateRandomShortLet(dateTime);
            randomShortLet.UpdatedBy = randomShortLet.CreatedBy;
            randomShortLet.UpdatedDate = randomShortLet.CreatedDate;
            ShortLet inputShortLet = randomShortLet;
            ShortLet storageShortLet = randomShortLet;
            ShortLet expectedShortLet = randomShortLet;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertShortLetAsync(inputShortLet))
                    .ReturnsAsync(storageShortLet);

            // when
            ShortLet actualShortLet =
                await this.shortLetService.CreateShortLetAsync(inputShortLet);

            // then
            actualShortLet.Should().BeEquivalentTo(expectedShortLet);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAsync(inputShortLet),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
