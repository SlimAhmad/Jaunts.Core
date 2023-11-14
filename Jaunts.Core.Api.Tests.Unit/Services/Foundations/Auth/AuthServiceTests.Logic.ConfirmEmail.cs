// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Auth
{
    public partial class AuthServiceTests
    {
        [Fact]
        public async Task ShouldConfirmEmailAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            List<string> randomRoleList = CreateRandomStringList();
            IQueryable<ApplicationRole> randomRoles = CreateRandomRoles(dateTime,randomRoleList);
            ApplicationUser randomUser = CreateRandomUser(dates: dateTime);
            ApplicationUser inputUser = randomUser;
            ApplicationUser storageUser = randomUser;
            ApplicationUser expectedUser = storageUser;
            string token = GetRandomString();
            string email = GetRandomEmailAddresses();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.userManagementBrokerMock.Setup(broker =>
                broker.FindByEmailAsync(email))
                    .ReturnsAsync(storageUser);

            this.userManagementBrokerMock.Setup(broker =>
                broker.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                    .ReturnsAsync(IdentityResult.Success);

            this.userManagementBrokerMock.Setup(broker =>
                broker.GetRolesAsync(It.IsAny<ApplicationUser>()))
                    .ReturnsAsync(randomRoleList);

            this.roleManagerBrokerMock.Setup(broker =>
               broker.SelectAllRoles())
                   .Returns(randomRoles);

            // when
            UserProfileDetailsApiResponse actualAuth =
                await this.authService.ConfirmEmailRequestAsync(token,email);

            // then
            actualAuth.Should().BeEquivalentTo(expectedUser);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.FindByEmailAsync(email),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                  broker.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                      Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.GetRolesAsync(It.IsAny<ApplicationUser>()),
                    Times.Once);

            this.roleManagerBrokerMock.Verify(broker =>
                broker.SelectAllRoles(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
