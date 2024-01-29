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
        public async Task ShouldCreateProvidersDirectorAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            ProvidersDirector randomProvidersDirector = CreateRandomProvidersDirector(dateTime);
            randomProvidersDirector.UpdatedBy = randomProvidersDirector.CreatedBy;
            randomProvidersDirector.UpdatedDate = randomProvidersDirector.CreatedDate;
            ProvidersDirector inputProvidersDirector = randomProvidersDirector;
            ProvidersDirector storageProvidersDirector = randomProvidersDirector;
            ProvidersDirector expectedProvidersDirector = randomProvidersDirector;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProvidersDirectorAsync(inputProvidersDirector))
                    .ReturnsAsync(storageProvidersDirector);

            // when
            ProvidersDirector actualProvidersDirector =
                await this.providersDirectorService.CreateProvidersDirectorAsync(inputProvidersDirector);

            // then
            actualProvidersDirector.Should().BeEquivalentTo(expectedProvidersDirector);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAsync(inputProvidersDirector),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
