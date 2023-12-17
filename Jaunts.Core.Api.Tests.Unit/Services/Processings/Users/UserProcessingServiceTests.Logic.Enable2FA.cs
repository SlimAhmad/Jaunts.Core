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
        public async Task ShouldEnable2FAuthenticationAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = true;


            IQueryable<ApplicationUser> randomUsers =
                CreateRandomUsers(inputUser);

            IQueryable<ApplicationUser> retrievedUsers =
                randomUsers;

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Returns(retrievedUsers);

            this.userServiceMock.Setup(service =>
                service.ModifyUserTwoFactorAsync(inputUser,inputBoolean))
                    .ReturnsAsync(expectedUser);

            // when
            ApplicationUser actualUser = await this.userProcessingService
                .EnableOrDisable2FactorAuthenticationAsync(inputUser.Id);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);


            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(),
                    Times.AtLeast(2));

            this.userServiceMock.Verify(service =>
                service.ModifyUserTwoFactorAsync(inputUser,inputBoolean),
                    Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldDisable2FAuthenticationAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            inputUser.TwoFactorEnabled = true;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = false;
            


            IQueryable<ApplicationUser> randomUsers =
                CreateRandomUsers(inputUser);

            IQueryable<ApplicationUser> retrievedUsers =
                randomUsers;

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Returns(retrievedUsers);

            this.userServiceMock.Setup(service =>
                service.ModifyUserTwoFactorAsync(inputUser, inputBoolean))
                    .ReturnsAsync(expectedUser);

            // when
            ApplicationUser actualUser = await this.userProcessingService
                .EnableOrDisable2FactorAuthenticationAsync(inputUser.Id);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);


            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(),
                    Times.AtLeast(2));

            this.userServiceMock.Verify(service =>
                service.ModifyUserTwoFactorAsync(inputUser, inputBoolean),
                    Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
