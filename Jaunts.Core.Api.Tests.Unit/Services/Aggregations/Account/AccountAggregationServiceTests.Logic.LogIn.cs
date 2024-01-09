// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Aggregation.Account
{
    public partial class AccountAggregationServiceTests
    {
        [Fact]
        private async Task ShouldLoginAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            List<string> randomRoleList = CreateRandomStringList();
            LoginRequest loginRequest =
               CreateLoginRequest();
            string password = GetRandomString();
            string email = GetRandomString();


            ApplicationUser randomUser =
                    CreateRandomUser(dateTime);

            IQueryable<ApplicationRole> randomRoles =
                CreateRandomRoles(dateTime, randomRoleList);

            ApplicationUser inputUser = randomUser;
            ApplicationUser storageUser = inputUser;
        
            UserAccountDetailsResponse randomUserProfileDetails =
                CreateUserAccountDetailsApiResponse(storageUser);

            UserAccountDetailsResponse expectedUserProfileDetails =
                    randomUserProfileDetails;

            this.userOrchestrationMock.Setup(broker =>
                broker.RetrieveUserByEmailOrUserNameAsync(loginRequest.UsernameOrEmail))
                    .ReturnsAsync(storageUser);

            this.signInOrchestrationMock.Setup(broker =>
                broker.LoginRequestAsync(loginRequest.UsernameOrEmail,loginRequest.Password))
                    .ReturnsAsync(true);

            this.jwtOrchestrationMock.Setup(broker =>
                broker.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()))
                        .ReturnsAsync(randomUserProfileDetails);

            // when
            UserAccountDetailsResponse actualAuth =
                await this.accountAggregationService.LogInRequestAsync(loginRequest);

            // then
            actualAuth.Should().BeEquivalentTo(expectedUserProfileDetails);

            this.userOrchestrationMock.Verify(broker =>
                broker.RetrieveUserByEmailOrUserNameAsync(loginRequest.UsernameOrEmail),
                    Times.Once);

            this.signInOrchestrationMock.Verify(broker =>
                 broker.LoginRequestAsync(loginRequest.UsernameOrEmail,loginRequest.Password),
                       Times.Once);

            this.jwtOrchestrationMock.Verify(broker =>
                  broker.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()),
                        Times.Once);

 
            this.userOrchestrationMock.VerifyNoOtherCalls();
            this.jwtOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


    }
}
