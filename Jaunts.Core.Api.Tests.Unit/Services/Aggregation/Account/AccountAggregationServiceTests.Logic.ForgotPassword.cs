// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Auth.LoginRegister;
using Jaunts.Core.Models.Email;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Aggregation.Account
{
    public partial class AccountAggregationServiceTests
    {
        [Fact]
        private async Task ShouldForgetPasswordAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser storageUser = inputUser;
            ResetPasswordApiRequest resetPasswordApiRequest =
               CreateResetPasswordApiRequest();
             
            string randomEmail = GetRandomEmailAddresses();
            string InputEmail = randomEmail;
            string storageEmail = InputEmail;

            SendEmailResponse sendEmailResponse =
                CreateSendEmailResponse();

            this.userOrchestrationMock.Setup(broker =>
                broker.RetrieveUserByEmailOrUserNameAsync(InputEmail))
                    .ReturnsAsync(randomUser);

            this.emailOrchestrationBrokerMock.Setup(broker =>
                broker.PasswordResetMailAsync(storageUser))
                    .ReturnsAsync(sendEmailResponse);
            // when
            bool actualAuth =
                await this.accountAggregationService.ForgotPasswordRequestAsync(storageEmail);

            // then
            actualAuth.Should().BeTrue();

            this.userOrchestrationMock.Verify(broker =>
                broker.RetrieveUserByEmailOrUserNameAsync(It.IsAny<string>()),
                    Times.Once);

            this.emailOrchestrationBrokerMock.Verify(broker =>
              broker.PasswordResetMailAsync(It.IsAny<ApplicationUser>()),
                  Times.Once);

            this.userOrchestrationMock.VerifyNoOtherCalls();
            this.jwtOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
