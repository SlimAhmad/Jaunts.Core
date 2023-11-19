﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;
using Moq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Emails
{
    public partial class EmailServiceTests
    {
        [Fact]
        public async Task ShouldPostForgotPasswordWithForgotPasswordRequestAsync()
        {
            // given 
            var sendEmailDetails = CreateSendEmailDetailRequest();
            var sendEmailResponse = CreateSendEmailResponse();
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            SendEmailResponse expectedEmailResponse = sendEmailResponse.DeepClone();



            this.emailTemplateSender.Setup(broker =>
                broker.SendVerificationEmailAsync(It.IsAny<SendEmailDetails>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(sendEmailDetails);

            this.emailBrokerMock.Setup(broker =>
            broker.PostMailAsync(It.IsAny<SendEmailDetails>()))
                    .ReturnsAsync(sendEmailResponse);
            // when
            SendEmailResponse actualCreateForgotPassword =
               await this.emailService.PostMailRequestAsync(sendEmailDetails);

            // then
            actualCreateForgotPassword.Should().BeEquivalentTo(expectedEmailResponse);

            this.emailTemplateSender.Verify(broker =>
               broker.SendVerificationEmailAsync(It.IsAny<SendEmailDetails>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                   Times.Once);

            this.emailBrokerMock.Verify(broker =>
            broker.PostMailAsync(It.IsAny<SendEmailDetails>()),
                Times.Once);

            this.emailTemplateSender.VerifyNoOtherCalls();
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
