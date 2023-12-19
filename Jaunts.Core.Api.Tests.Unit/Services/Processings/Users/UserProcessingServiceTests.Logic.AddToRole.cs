using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
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

            this.userServiceMock.Setup(service =>
                service.AddUserRolesAsync(inputUser,inputRole))
                    .ReturnsAsync(expectedUser);

            // when
            ApplicationUser actualUser = await this.userProcessingService
                .AddToRoleAsync(inputUser,inputRole);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userServiceMock.Verify(service =>
                service.AddUserRolesAsync(inputUser,inputRole),
                    Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


    }
}
