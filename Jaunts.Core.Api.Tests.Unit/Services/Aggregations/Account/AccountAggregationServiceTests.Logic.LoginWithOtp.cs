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

            string code = GetRandomString();
            bool randomBoolean = GetRandomBoolean();



            UserAccountDetailsResponse userAccountDetailsResponse =
                CreateUserAccountDetailsApiResponse(inputUser);

            UserAccountDetailsResponse expectedUserAccountDetailsResponse =
                  userAccountDetailsResponse;

            this.signInOrchestrationMock.Setup(orchestration =>
                orchestration.LoginOtpRequestAsync(code,inputUser.Email))
                    .ReturnsAsync(storageUser);

            this.jwtOrchestrationMock.Setup(orchestration =>
               orchestration.JwtAccountDetailsAsync(storageUser))
                   .ReturnsAsync(userAccountDetailsResponse);

            // when
            UserAccountDetailsResponse actualAuth =
                await this.accountAggregationService.OtpLoginRequestAsync(code,inputUser.Email);

            // then
            actualAuth.Should().BeEquivalentTo(expectedUserAccountDetailsResponse);

            this.signInOrchestrationMock.Verify(orchestration =>
              orchestration.LoginOtpRequestAsync(code,inputUser.Email),
                  Times.Once);

            this.jwtOrchestrationMock.Verify(orchestration =>
              orchestration.JwtAccountDetailsAsync(storageUser),
                  Times.Once);

            this.signInOrchestrationMock.VerifyNoOtherCalls();
            this.jwtOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
