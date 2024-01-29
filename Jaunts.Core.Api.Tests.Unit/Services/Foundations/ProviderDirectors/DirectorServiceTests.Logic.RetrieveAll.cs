// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectors
{
    public partial class ProvidersDirectorServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllProvidersDirectors()
        {
            // given
            IQueryable<ProvidersDirector> randomProvidersDirectors = CreateRandomProvidersDirectors();
            IQueryable<ProvidersDirector> storageProvidersDirectors = randomProvidersDirectors;
            IQueryable<ProvidersDirector> expectedProvidersDirectors = storageProvidersDirectors;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProvidersDirectors())
                    .Returns(storageProvidersDirectors);

            // when
            IQueryable<ProvidersDirector> actualProvidersDirectors =
                this.providersDirectorService.RetrieveAllProvidersDirectors();

            // then
            actualProvidersDirectors.Should().BeEquivalentTo(expectedProvidersDirectors);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProvidersDirectors(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
