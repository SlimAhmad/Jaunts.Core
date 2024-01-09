using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Users
{
    public partial class UserOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldAddUserToRoleAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();
            string randomRole = GetRandomString();
            string inputRole = randomRole;

            this.userProcessingServiceMock.Setup(service =>
                service.AddToRoleAsync(inputUser,inputRole))
                    .ReturnsAsync(expectedUser);

            // when
            ApplicationUser actualUser = await this.userOrchestrationService
                .AddUserToRoleAsync(inputUser,inputRole);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userProcessingServiceMock.Verify(service =>
                service.AddToRoleAsync(inputUser,inputRole),
                    Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


    }
}
