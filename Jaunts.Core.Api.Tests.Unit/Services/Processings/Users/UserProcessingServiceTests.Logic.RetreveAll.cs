using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System.Linq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllUsers()
        {
            // given
            IQueryable<ApplicationUser> randomUsers = CreateRandomUsers();
            IQueryable<ApplicationUser> storageUsers = randomUsers;
            IQueryable<ApplicationUser> expectedUsers = storageUsers;

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Returns(storageUsers);

            // when
            IQueryable<ApplicationUser> actualUsers =
                this.userProcessingService.RetrieveAllUsers();

            // then
            actualUsers.Should().BeEquivalentTo(expectedUsers);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(),
                    Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
