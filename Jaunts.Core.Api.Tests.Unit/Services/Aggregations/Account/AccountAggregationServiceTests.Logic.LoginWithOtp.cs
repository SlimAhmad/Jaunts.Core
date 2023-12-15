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
        private async Task ShouldLoginWithOtpAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser storageUser = inputUser;

            string provider = GetRandomString();
            string code = GetRandomString();
            bool isPersistent = GetRandomBoolean();
            bool rememberClient = GetRandomBoolean();
            bool randomBoolean = GetRandomBoolean();

            string randomEmail = GetRandomEmailAddresses();
            string InputEmail = randomEmail;
            string storageEmail = InputEmail;

            UserAccountDetailsResponse userAccountDetailsResponse =
                CreateUserAccountDetailsApiResponse(inputUser);

            UserAccountDetailsResponse expectedUserAccountDetailsResponse =
                  userAccountDetailsResponse;

            this.signInOrchestrationMock.Setup(broker =>
                broker.TwoFactorSignInAsync(provider,code,isPersistent,rememberClient))
                    .ReturnsAsync(randomBoolean);

            this.userOrchestrationMock.Setup(broker =>
                broker.RetrieveUserByEmailOrUserNameAsync(InputEmail))
                    .ReturnsAsync(storageUser);

            this.jwtOrchestrationMock.Setup(broker =>
               broker.JwtAccountDetailsAsync(storageUser))
                   .ReturnsAsync(userAccountDetailsResponse);

            // when
            UserAccountDetailsResponse actualAuth =
                await this.accountAggregationService.LoginWithOTPRequestAsync(code,InputEmail);

            // then
            actualAuth.Should().BeEquivalentTo(expectedUserAccountDetailsResponse);

            this.signInOrchestrationMock.Verify(broker =>
              broker.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()),
                  Times.Once);

            this.userOrchestrationMock.Verify(broker =>
                broker.RetrieveUserByEmailOrUserNameAsync(It.IsAny<string>()),
                    Times.Once);

            this.jwtOrchestrationMock.Verify(broker =>
              broker.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()),
                  Times.Once);

            this.signInOrchestrationMock.VerifyNoOtherCalls();
            this.userOrchestrationMock.VerifyNoOtherCalls();
            this.jwtOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
