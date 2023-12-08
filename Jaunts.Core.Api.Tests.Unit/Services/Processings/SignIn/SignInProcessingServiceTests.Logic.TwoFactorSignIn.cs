using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.SignIn
{
    public partial class SignInProcessingServiceTests
    {
        [Fact]
        public async Task ShouldTwoFactorSignInAsync()
        {
            // given
            string randomCode = GetRandomString();
            string inputCode = randomCode;
            bool randomBoolean =  GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            string randomProvider = GetRandomString();
            string inputProvider = randomProvider;
            string expectedProvider = inputProvider;
            var signInResult = new SignInResult();
            

            this.signInServiceMock.Setup(service =>
                service.TwoFactorSignInRequestAsync(
                    It.IsAny<string>(),It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()))
                        .ReturnsAsync(signInResult);

            // when
            bool actualProvider =
                  await this.signInProcessingService.TwoFactorSignInAsync(inputProvider,randomCode,randomBoolean,randomBoolean);

            // then
            actualProvider.Should().BeFalse();

            this.signInServiceMock.Verify(service =>
                service.TwoFactorSignInRequestAsync(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()),
                     Times.Once);

            this.signInServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
