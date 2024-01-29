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
        public async Task ShouldDeleteShortLetAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ShortLet randomShortLet = CreateRandomShortLet(dateTime);
            Guid inputShortLetId = randomShortLet.Id;
            ShortLet inputShortLet = randomShortLet;
            ShortLet storageShortLet = randomShortLet;
            ShortLet expectedShortLet = randomShortLet;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShortLetByIdAsync(inputShortLetId))
                    .ReturnsAsync(inputShortLet);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteShortLetAsync(inputShortLet))
                    .ReturnsAsync(storageShortLet);

            // when
            ShortLet actualShortLet =
                await this.shortLetService.RemoveShortLetByIdAsync(inputShortLetId);

            // then
            actualShortLet.Should().BeEquivalentTo(expectedShortLet);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShortLetByIdAsync(inputShortLetId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShortLetAsync(inputShortLet),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
