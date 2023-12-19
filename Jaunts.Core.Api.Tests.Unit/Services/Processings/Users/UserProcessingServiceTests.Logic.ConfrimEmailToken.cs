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
        public async Task ShouldConfirmEmailAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();
            string randomToken = GetRandomString();
            string inputToken = randomToken;


            IQueryable<ApplicationUser> randomUsers =
                CreateRandomUsers(inputUser);

            IQueryable<ApplicationUser> retrievedUsers =
                randomUsers;

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Returns(retrievedUsers);

            this.userServiceMock.Setup(service =>
                service.ValidateEmailTokenAsync(inputUser,inputToken))
                    .ReturnsAsync(expectedUser);

            // when
            ApplicationUser actualUser = await this.userProcessingService
                .ValidateEmailTokenAsync(inputToken,inputUser.Email);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);


            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(),
                    Times.Once);

            this.userServiceMock.Verify(service =>
                service.ValidateEmailTokenAsync(inputUser,inputToken),
                    Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


    }
}
