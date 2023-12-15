using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Email
{
    public partial class EmailOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldSendVerificationMailAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            string randomToken = GetRandomString();
            string inputToken = randomToken;
            string expectedToken = inputToken;
            SendEmailResponse sendEmailResponse = CreateSendEmailResponse();

            SendEmailResponse expectedSendEmailResponse = sendEmailResponse;

            this.userProcessingServiceMock.Setup(service =>
                service.EmailConfirmationTokenAsync(inputUser))
                    .ReturnsAsync(expectedToken);

            this.emailProcessingServiceMock.Setup(template =>
                template.SendVerificationMailRequestAsync(
                    inputUser, inputToken))
                    .ReturnsAsync(sendEmailResponse);

            // when
            SendEmailResponse actualEmail =
                await this.emailOrchestrationService.VerificationMailAsync(inputUser);


            // then
            actualEmail.Should().BeEquivalentTo(expectedSendEmailResponse);

            this.userProcessingServiceMock.Verify(broker =>
             broker.EmailConfirmationTokenAsync(inputUser),
                 Times.Once);

            this.emailProcessingServiceMock.Verify(template =>
                template.SendVerificationMailRequestAsync(
                     inputUser,inputToken),
                        Times.Once());


            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.emailProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        } 
    }
}
