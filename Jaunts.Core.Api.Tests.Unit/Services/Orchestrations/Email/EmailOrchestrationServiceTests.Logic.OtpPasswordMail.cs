using FluentAssertions;
using Jaunts.Core.Api.Models.Auth;
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
        public async Task ShouldSendOtpPasswordMailAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            string randomToken = GetRandomString();
            string inputToken = randomToken;
            string expectedToken = inputToken;
            SendEmailResponse sendEmailResponse = CreateSendEmailResponse();
            UserAccountDetailsResponse expectedAccountDetailsResponse = 
                ConvertToTwoFAResponse(inputUser);
    

            SendEmailResponse expectedSendEmailResponse = sendEmailResponse;

            this.userProcessingServiceMock.Setup(service =>
                service.RetrieveTwoFactorTokenAsync(inputUser))
                    .ReturnsAsync(expectedToken);

            this.emailProcessingServiceMock.Setup(template =>
                template.OtpVerificationMailRequestAsync(
                    inputUser, inputToken))
                    .ReturnsAsync(sendEmailResponse);

            // when
            UserAccountDetailsResponse actualEmail =
                await this.emailOrchestrationService.TwoFactorMailAsync(inputUser);

            // then
            actualEmail.Should().BeEquivalentTo(expectedAccountDetailsResponse);

            this.userProcessingServiceMock.Verify(broker =>
             broker.RetrieveTwoFactorTokenAsync(inputUser),
                 Times.Once);

            this.emailProcessingServiceMock.Verify(template =>
                template.OtpVerificationMailRequestAsync(
                     inputUser,inputToken),
                        Times.Once());

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.emailProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        } 
    }
}
