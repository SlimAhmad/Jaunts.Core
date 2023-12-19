// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        private async Task ShouldRegisterUserAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            List<string> randomRoleList = CreateRandomStringList();
            RegisterUserApiRequest registerUserApiRequest =
               CreateRegisterUserApiRequest(dateTime);
            ApplicationUser randomUser = 
                ConvertToUserRequest(registerUserApiRequest);
            ApplicationUser inputUser = randomUser;
            ApplicationUser storageUser = inputUser.DeepClone();
            SendEmailResponse sendEmailResponse = CreateSendEmailResponse();
            string password = GetRandomString();

            IQueryable<ApplicationRole> randomRoles =
                CreateRandomRoles(dateTime, randomRoleList);
        
            UserAccountDetailsResponse randomUserProfileDetails =
                CreateRegisterUserResponse(registerUserApiRequest);

            UserAccountDetailsResponse expectedUserProfileDetails =
                    randomUserProfileDetails;

            this.userOrchestrationMock.Setup(orchestration =>
                orchestration.RegisterUserAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()))
                    .ReturnsAsync(storageUser);

            this.userOrchestrationMock.Setup(orchestration =>
                orchestration.AddUserToRoleAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()))
                    .ReturnsAsync(storageUser);

            this.emailOrchestrationMock.Setup(orchestration =>
                 orchestration.VerificationMailAsync(It.IsAny<ApplicationUser>()))
                     .ReturnsAsync(sendEmailResponse);

            this.jwtOrchestrationMock.Setup(orchestration =>
                orchestration.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()))
                        .ReturnsAsync(randomUserProfileDetails);

            // when
            UserAccountDetailsResponse actualAuth =
                await this.accountAggregationService.RegisterUserRequestAsync(registerUserApiRequest);

            // then
            actualAuth.Should().BeEquivalentTo(expectedUserProfileDetails);

            this.userOrchestrationMock.Verify(orchestration =>
                orchestration.RegisterUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                    Times.Once);

            this.userOrchestrationMock.Verify(orchestration =>
                 orchestration.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                       Times.Once);

            this.emailOrchestrationMock.Verify(orchestration =>
             orchestration.VerificationMailAsync(It.IsAny<ApplicationUser>()),
                   Times.Once);

            this.jwtOrchestrationMock.Verify(orchestration =>
                  orchestration.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()),
                        Times.Once);
         
            this.userOrchestrationMock.VerifyNoOtherCalls();
            this.emailOrchestrationMock.VerifyNoOtherCalls();
            this.jwtOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

     
    }
}
