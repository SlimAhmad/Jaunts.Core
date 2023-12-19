// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Jwt
{
    public partial class JwtOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveJwtAndAccountDetailsAsync()
        {
            // given
            ApplicationUser randomUser =CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            int randomPermissions = GetRandomNumber();
            int inputPermissions = randomPermissions;
            int expectedPermissions = randomPermissions;
            string someToken = GetRandomString();
            List<string> roles = CreateRandomStringList();
            UserAccountDetailsResponse expectedUserAccountDetails = 
                ConvertToAccountDetailsResponse(inputUser,someToken,roles);

            this.userProcessingServiceMock.Setup(service =>
               service.RetrieveUserRolesAsync(inputUser))
                   .ReturnsAsync(roles);

            this.roleProcessingServiceMock.Setup(service =>
               service.RetrievePermissions(roles))
                   .ReturnsAsync(expectedPermissions);

            this.jwtProcessingServiceMock.Setup(service =>
               service.GenerateJwtTokenAsync(inputUser,inputPermissions))
                   .ReturnsAsync(someToken);

            // when
            UserAccountDetailsResponse actualJwt =
                await this.jwtOrchestrationService.JwtAccountDetailsAsync(inputUser);

            // then
            actualJwt.Should().BeEquivalentTo(expectedUserAccountDetails);

            this.userProcessingServiceMock.Verify(service =>
                service.RetrieveUserRolesAsync(inputUser),
                    Times.Once);

            this.roleProcessingServiceMock.Verify(service =>
                 service.RetrievePermissions(roles),
                     Times.Once);

            this.jwtProcessingServiceMock.Verify(service =>
                  service.GenerateJwtTokenAsync(inputUser,inputPermissions),
                      Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.roleProcessingServiceMock.VerifyNoOtherCalls();
            this.jwtProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
