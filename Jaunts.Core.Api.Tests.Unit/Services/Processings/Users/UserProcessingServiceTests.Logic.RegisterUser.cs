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
        public async Task ShouldRegisterUserAsync()
        {
            // given

            string randomPassword = GetRandomPassword();
            string inputPassword = randomPassword;
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();

            this.userServiceMock.Setup(service =>
                service.InsertUserRequestAsync(inputUser, inputPassword))
                    .ReturnsAsync(addedUser);

            // when
            ApplicationUser actualUser = await this.userProcessingService
                .CreateUserAsync(inputUser, inputPassword);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userServiceMock.Verify(service =>
                service.InsertUserRequestAsync(inputUser, inputPassword),
                    Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
