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

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Email
{
    public partial class EmailProcessingServiceTests
    {
        [Fact]
        public async Task ShouldSendVerificationMailAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            ApplicationUser inputUser = randomUser;
            string randomToken = GetRandomString();
            string inputToken = randomToken;
            string randomString = GetRandomString();
            SendEmailMessage sendEmailMessage = CreateSendEmailDetailRequest();
            SendEmailResponse sendEmailResponse = CreateSendEmailResponse();

            SendEmailResponse expectedSendEmailResponse = sendEmailResponse;

            this.emailTemplateSenderMock.Setup(template =>
                template.SendVerificationEmailAsync(
                    It.IsAny<SendEmailMessage>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(sendEmailMessage);

            this.emailServiceMock.Setup(service =>
                service.SendEmailRequestAsync(sendEmailMessage))
                    .ReturnsAsync(sendEmailResponse);

            // when
            SendEmailResponse actualEmail =
                await this.emailProcessingService.VerificationMailRequestAsync(inputUser, inputToken);

            // then
            actualEmail.Should().BeEquivalentTo(expectedSendEmailResponse);

            this.emailTemplateSenderMock.Verify(template =>
                template.SendVerificationEmailAsync(
                    It.IsAny<SendEmailMessage>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once());

            this.emailServiceMock.Verify(broker =>
                broker.SendEmailRequestAsync(sendEmailMessage),
                    Times.Once);

            this.emailTemplateSenderMock.VerifyNoOtherCalls();
            this.emailServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
