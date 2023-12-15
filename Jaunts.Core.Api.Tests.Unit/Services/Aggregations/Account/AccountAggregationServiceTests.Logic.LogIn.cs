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
            LoginCredentialsApiRequest loginCredentialsApiRequest =
               CreateLoginCredentialsApiRequest();
            string password = GetRandomString();
            string email = GetRandomString();


            ApplicationUser randomUser =
                    CreateRandomUser(dateTime);

            IQueryable<ApplicationRole> randomRoles =
                CreateRandomRoles(dateTime, randomRoleList);

            ApplicationUser inputUser = randomUser;
            ApplicationUser storageUser = inputUser;
        
            UserAccountDetailsApiResponse randomUserProfileDetails =
                CreateUserAccountDetailsApiResponse(storageUser);

            UserAccountDetailsApiResponse expectedUserProfileDetails =
                    randomUserProfileDetails;

            this.userOrchestrationMock.Setup(broker =>
                broker.RetrieveUserByEmailOrUserNameAsync(loginCredentialsApiRequest.UsernameOrEmail))
                    .ReturnsAsync(storageUser);

            this.userOrchestrationMock.Setup(broker =>
                broker.CheckPasswordValidityAsync(password,storageUser.Id))
                    .ReturnsAsync(true);

            this.jwtOrchestrationMock.Setup(broker =>
                broker.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()))
                        .ReturnsAsync(randomUserProfileDetails);

            // when
            UserAccountDetailsApiResponse actualAuth =
                await this.accountAggregationService.LogInRequestAsync(loginCredentialsApiRequest);

            // then
            actualAuth.Should().BeEquivalentTo(expectedUserProfileDetails);

            this.userOrchestrationMock.Verify(broker =>
                broker.RetrieveUserByEmailOrUserNameAsync(It.IsAny<string>()),
                    Times.Once);

            this.userOrchestrationMock.Verify(broker =>
                 broker.CheckPasswordValidityAsync(It.IsAny<string>(), It.IsAny<Guid>()),
                       Times.Once);

            this.jwtOrchestrationMock.Verify(broker =>
                  broker.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()),
                        Times.Once);

 
            this.userOrchestrationMock.VerifyNoOtherCalls();
            this.jwtOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldLoginWithOtpTokenAsync() 
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            List<string> randomRoleList = CreateRandomStringList();
            LoginCredentialsApiRequest loginCredentialsApiRequest =
               CreateLoginCredentialsApiRequest();
            SendEmailResponse sendEmailResponse = CreateSendEmailResponse();
            string password = GetRandomString();
            string email = GetRandomString();
            bool randomBoolean = GetRandomBoolean();


            ApplicationUser randomUser =
                    CreateRandomUser(dateTime);

            IQueryable<ApplicationRole> randomRoles =
                CreateRandomRoles(dateTime, randomRoleList);

            ApplicationUser inputUser = randomUser;
            ApplicationUser storageUser = inputUser;
            storageUser.TwoFactorEnabled = true;

            UserAccountDetailsApiResponse inputUserProfileDetails =
                CreateUserAccountDetailsApiResponse(storageUser);

            UserAccountDetailsApiResponse expectedUserProfileDetails =
                    inputUserProfileDetails;

            this.userOrchestrationMock.Setup(broker =>
             broker.RetrieveUserByEmailOrUserNameAsync(loginCredentialsApiRequest.UsernameOrEmail))
                 .ReturnsAsync(storageUser);

            this.signInOrchestrationMock.Setup(broker =>
                broker.SignOutAsync());

            this.signInOrchestrationMock.Setup(broker =>
               broker.PasswordSignInAsync(storageUser, password,randomBoolean,randomBoolean))
                   .ReturnsAsync(randomBoolean);

            this.emailOrchestrationMock.Setup(broker =>
                broker.TwoFactorMailAsync(storageUser))
                    .ReturnsAsync(inputUserProfileDetails);

            // when
            UserAccountDetailsApiResponse actualAuth =
                await this.accountAggregationService.LogInRequestAsync(loginCredentialsApiRequest);

            // then
            actualAuth.Should().BeEquivalentTo(expectedUserProfileDetails);

            this.userOrchestrationMock.Verify(broker =>
                broker.RetrieveUserByEmailOrUserNameAsync(It.IsAny<string>()),
                    Times.Once);

            this.signInOrchestrationMock.Verify(broker =>
             broker.SignOutAsync(),
                 Times.Once);

            this.signInOrchestrationMock.Verify(broker =>
             broker.PasswordSignInAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>(),It.IsAny<bool>(),It.IsAny<bool>()),
                 Times.Once);

            this.emailOrchestrationMock.Verify(broker =>
                 broker.TwoFactorMailAsync(It.IsAny<ApplicationUser>()),
                       Times.Once);

            this.userOrchestrationMock.VerifyNoOtherCalls();
            this.signInOrchestrationMock.VerifyNoOtherCalls();
            this.emailOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
