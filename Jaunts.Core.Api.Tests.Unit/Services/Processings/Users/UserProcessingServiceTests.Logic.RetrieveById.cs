using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveByIdUserAsync()
        {
            // given
            Guid randomUserId = GetRandomGuid();
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser storageUser = randomUser;
            ApplicationUser expectedUser = storageUser;

            this.userServiceMock.Setup(service =>
                service.RetrieveUserByIdRequestAsync(randomUserId))
                    .ReturnsAsync(storageUser);

            // when
            ApplicationUser actualUser =
                  await this.userProcessingService.RetrieveUserById(randomUserId);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userServiceMock.Verify(service =>
                service.RetrieveUserByIdRequestAsync(randomUserId),
                    Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
