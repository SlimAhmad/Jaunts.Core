// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyProvidersDirectorAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            ProvidersDirector randomProvidersDirector = CreateRandomProvidersDirector(randomInputDate);
            ProvidersDirector inputProvidersDirector = randomProvidersDirector;
            ProvidersDirector afterUpdateStorageProvidersDirector = inputProvidersDirector;
            ProvidersDirector expectedProvidersDirector = afterUpdateStorageProvidersDirector;
            ProvidersDirector beforeUpdateStorageProvidersDirector = randomProvidersDirector.DeepClone();
            inputProvidersDirector.UpdatedDate = randomDate;
            Guid ProvidersDirectorId = inputProvidersDirector.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProvidersDirectorByIdAsync(ProvidersDirectorId))
                    .ReturnsAsync(beforeUpdateStorageProvidersDirector);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateProvidersDirectorAsync(inputProvidersDirector))
                    .ReturnsAsync(afterUpdateStorageProvidersDirector);

            // when
            ProvidersDirector actualProvidersDirector =
                await this.providersDirectorService.ModifyProvidersDirectorAsync(inputProvidersDirector);

            // then
            actualProvidersDirector.Should().BeEquivalentTo(expectedProvidersDirector);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(ProvidersDirectorId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProvidersDirectorAsync(inputProvidersDirector),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
