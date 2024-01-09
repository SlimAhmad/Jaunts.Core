using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Users
{
    public partial class UserOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldRegisterUserAsync()
        {
            // given

            string randomPassword = GetRandomPassword();
            string inputPassword = randomPassword;
            string randomRole = GetRandomString();
            string inputRole = "User";
            string randomToken = GetRandomString();
            string inputToken = randomToken;
            string expectedToken = inputToken;
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();
            SendEmailResponse sendEmailResponse = CreateSendEmailResponse();

            this.userProcessingServiceMock.Setup(service =>
                service.CreateUserAsync(inputUser, inputPassword))
                    .ReturnsAsync(addedUser);

            this.userProcessingServiceMock.Setup(service =>
                service.AddToRoleAsync(inputUser, inputRole))
                    .ReturnsAsync(addedUser);

            this.userProcessingServiceMock.Setup(service =>
                service.EmailConfirmationTokenAsync(inputUser))
                    .ReturnsAsync(expectedToken);

            this.emailProcessingServiceMock.Setup(service =>
                service.VerificationMailRequestAsync(addedUser,inputToken))
                    .ReturnsAsync(sendEmailResponse);

            // when
            ApplicationUser actualUser = await this.userOrchestrationService
                .RegisterUserAsync(inputUser, inputPassword);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userProcessingServiceMock.Verify(service =>
                service.CreateUserAsync(inputUser, inputPassword),
                    Times.Once);

            this.userProcessingServiceMock.Verify(service =>
                service.AddToRoleAsync(inputUser, inputRole),
                    Times.Once);

            this.userProcessingServiceMock.Verify(service =>
                service.EmailConfirmationTokenAsync(inputUser),
                    Times.Once);

            this.emailProcessingServiceMock.Verify(service =>
                service.VerificationMailRequestAsync(addedUser, inputToken),
                    Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.emailProcessingServiceMock .VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
