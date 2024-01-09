using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Users
{
    public partial class UserOrchestrationServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllUsers()
        {
            // given
            IQueryable<ApplicationUser> randomUsers = CreateRandomUsers();
            IQueryable<ApplicationUser> storageUsers = randomUsers;
            IQueryable<ApplicationUser> expectedUsers = storageUsers;

            this.userProcessingServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Returns(storageUsers);

            // when
            IQueryable<ApplicationUser> actualUsers =
                this.userOrchestrationService.RetrieveAllUsers();

            // then
            actualUsers.Should().BeEquivalentTo(expectedUsers);

            this.userProcessingServiceMock.Verify(service =>
                service.RetrieveAllUsers(),
                    Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
