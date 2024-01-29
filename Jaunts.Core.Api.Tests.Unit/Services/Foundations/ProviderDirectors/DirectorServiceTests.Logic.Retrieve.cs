// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectors
{
    public partial class ProvidersDirectorServiceTests
    {
        [Fact]
        async Task ShouldRetrieveProvidersDirectorById()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProvidersDirector randomProvidersDirector = CreateRandomProvidersDirector(dateTime);
            Guid inputProvidersDirectorId = randomProvidersDirector.Id;
            ProvidersDirector inputProvidersDirector = randomProvidersDirector;
            ProvidersDirector expectedProvidersDirector = randomProvidersDirector;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorByIdAsync(inputProvidersDirectorId))
                    .ReturnsAsync(inputProvidersDirector);

            // when
            ProvidersDirector actualProvidersDirector =
                await this.providersDirectorService.RetrieveProvidersDirectorByIdAsync(inputProvidersDirectorId);

            // then
            actualProvidersDirector.Should().BeEquivalentTo(expectedProvidersDirector);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(inputProvidersDirectorId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
