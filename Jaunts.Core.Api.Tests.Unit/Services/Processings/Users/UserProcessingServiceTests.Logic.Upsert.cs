using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
        [Fact]
        public async Task ShouldAddUserIfNotExistAsync()
        {
            // given
            IQueryable<ApplicationUser> randomUsers = CreateRandomUsers();

            IQueryable<ApplicationUser> retrievedUsers =
            randomUsers;

            string randomPassword = GetRandomPassword();
            string inputPassword = randomPassword;
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Returns(retrievedUsers);

            this.userServiceMock.Setup(service =>
                service.AddUserAsync(inputUser,inputPassword))
                    .ReturnsAsync(addedUser);

            // when
            ApplicationUser actualUser = await this.userProcessingService
                .UpsertUserAsync(inputUser,inputPassword);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(),
                    Times.Once);

            this.userServiceMock.Verify(service =>
                service.AddUserAsync(inputUser,inputPassword),
                    Times.Once);

            this.userServiceMock.Verify(service =>
                service.ModifyUserAsync(It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyUserIfUserExistAsync()
        {
            // given
            string randomPassword = GetRandomPassword();
            string inputPassword = randomPassword;
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser modifiedUser = inputUser;
            ApplicationUser expectedUser = modifiedUser.DeepClone();

            IQueryable<ApplicationUser> randomUsers =
                CreateRandomUsers(inputUser);

            IQueryable<ApplicationUser> retrievedUsers =
                randomUsers;

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Returns(retrievedUsers);

            this.userServiceMock.Setup(service =>
                service.ModifyUserAsync(inputUser))
                    .ReturnsAsync(modifiedUser);

            // when
            ApplicationUser actualUser = await this.userProcessingService
                .UpsertUserAsync(inputUser,inputPassword);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(),
                    Times.Once);

            this.userServiceMock.Verify(service =>
                service.ModifyUserAsync(inputUser),
                    Times.Once);

            this.userServiceMock.Verify(service =>
                service.AddUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                    Times.Never);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
