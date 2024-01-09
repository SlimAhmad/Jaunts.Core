using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Auth.LoginRegister;
using Microsoft.AspNetCore.Identity.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Users
{
    public partial class UserOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldResetPasswordAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();
            Core.Models.Auth.LoginRegister.ResetPasswordRequest resetPassword = CreateRandomResetPasswordRequest(inputUser);

            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            bool expectedBoolean = inputBoolean;

            this.userProcessingServiceMock.Setup(service =>
                service.ResetUserPasswordByEmailAsync(resetPassword.Email,resetPassword.Token,resetPassword.Password))
                    .ReturnsAsync(expectedBoolean);

            // when
            bool actualUser = 
                await this.userOrchestrationService.ResetUserPasswordByEmailOrUserNameAsync(resetPassword);

            // then
            actualUser.Should().BeTrue();

            this.userProcessingServiceMock.Verify(service =>
                service.ResetUserPasswordByEmailAsync(resetPassword.Email,resetPassword.Token,resetPassword.Password),
                    Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
