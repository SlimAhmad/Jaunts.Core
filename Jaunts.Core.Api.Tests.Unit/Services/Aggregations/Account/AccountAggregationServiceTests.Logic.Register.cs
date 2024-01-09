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
            UserCredentialsRequest userCredentials =
               CreateRegisterUserApiRequest();

            ApplicationUser randomUser = 
                MapToUserRequest(userCredentials);

            ApplicationUser inputUser = randomUser;
            ApplicationUser storageUser = inputUser.DeepClone();
        
            UserAccountDetailsResponse randomUserProfileDetails =
                CreateRegisterUserResponse(userCredentials);

            UserAccountDetailsResponse expectedUserProfileDetails =
                    randomUserProfileDetails;


            this.userOrchestrationMock.Setup(orchestration =>
                orchestration.RegisterUserAsync(It.IsAny<ApplicationUser>(),userCredentials.Password))
                    .ReturnsAsync(storageUser);

            this.jwtOrchestrationMock.Setup(orchestration =>
                orchestration.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()))
                        .ReturnsAsync(expectedUserProfileDetails);

            // when
            UserAccountDetailsResponse actualAuth =
                await this.accountAggregationService.RegisterUserRequestAsync(userCredentials);

            // then
            actualAuth.Should().BeEquivalentTo(expectedUserProfileDetails);

            this.userOrchestrationMock.Verify(orchestration =>
                orchestration.RegisterUserAsync(It.IsAny<ApplicationUser>(), userCredentials.Password),
                    Times.Once);

            this.jwtOrchestrationMock.Verify(orchestration =>
                  orchestration.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()),
                        Times.Once);
         
            this.userOrchestrationMock.VerifyNoOtherCalls();
            this.jwtOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

     
    }
}
