using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Email
{
    public partial class EmailOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldSendResetPasswordMailAsync()
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
                service.PasswordResetTokenAsync(inputUser))
                    .ReturnsAsync(expectedToken);

            this.emailProcessingServiceMock.Setup(template =>
                template.SendForgetPasswordMailRequestAsync(
                    inputUser, inputToken))
                    .ReturnsAsync(sendEmailResponse);

            // when
            SendEmailResponse actualEmail =
                await this.emailOrchestrationService.PasswordResetMailAsync(inputUser);


            // then
            actualEmail.Should().BeEquivalentTo(expectedSendEmailResponse);

            this.userProcessingServiceMock.Verify(broker =>
             broker.PasswordResetTokenAsync(inputUser),
                 Times.Once);

            this.emailProcessingServiceMock.Verify(template =>
                template.SendForgetPasswordMailRequestAsync(
                     inputUser,inputToken),
                        Times.Once());


            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.emailProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        } 
    }
}
