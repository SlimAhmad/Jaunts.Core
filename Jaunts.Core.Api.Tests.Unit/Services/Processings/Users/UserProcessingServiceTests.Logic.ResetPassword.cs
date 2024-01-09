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

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
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

            IQueryable<ApplicationUser> randomUsers =
                CreateRandomUsers(inputUser);

            IQueryable<ApplicationUser> retrievedUsers =
                randomUsers;

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Returns(retrievedUsers);

            this.userServiceMock.Setup(service =>
                service.ModifyUserPasswordAsync(
                    It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(expectedUser);

            // when
            bool actualUser = await this.userProcessingService.ResetUserPasswordByEmailAsync(
                                 resetPassword.Email, resetPassword.Token, resetPassword.Password);

            // then
            actualUser.Should().BeTrue();

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(),
                    Times.Once);

            this.userServiceMock.Verify(service =>
                service.ModifyUserPasswordAsync(
                    It.IsAny<ApplicationUser>(),It.IsAny<string>(),It.IsAny<string>()),
                    Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
