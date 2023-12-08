using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.SignIn
{
    public partial class SignInProcessingServiceTests
    {
        [Fact]
        public async Task ShouldPasswordSignInAsync()
        {
            // given
            string randomPassword = GetRandomString();
            string inputPassword = randomPassword;
            bool randomBoolean =  GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser expectedUser = inputUser;
            var signInResult = new SignInResult();
            

            this.signInServiceMock.Setup(service =>
                service.PasswordSignInRequestAsync(
                    It.IsAny<ApplicationUser>(),It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()))
                        .ReturnsAsync(signInResult);
            // when
            bool actualUser =
                  await this.signInProcessingService.PasswordSignInAsync(inputUser,randomPassword,randomBoolean,randomBoolean);

            // then
            actualUser.Should().BeFalse();

            this.signInServiceMock.Verify(service =>
                service.PasswordSignInRequestAsync(
                    It.IsAny<ApplicationUser>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()),
                     Times.Once);

            this.signInServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
