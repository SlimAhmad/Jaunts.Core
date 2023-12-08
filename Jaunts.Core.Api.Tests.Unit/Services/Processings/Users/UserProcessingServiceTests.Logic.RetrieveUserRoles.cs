using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveUserRolesAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();
            List<string> randomRole = CreateRandomStringList();
            List<string> inputRole = randomRole;
            List<string> expectedRole = inputRole;

            this.userServiceMock.Setup(service =>
                service.RetrieveUserRolesRequestAsync(inputUser))
                    .ReturnsAsync(expectedRole);

            // when
            List<string> actualUser = await this.userProcessingService
                .RetrieveUserRolesAsync(inputUser);

            // then
            actualUser.Should().BeEquivalentTo(expectedRole);

            this.userServiceMock.Verify(service =>
                service.RetrieveUserRolesRequestAsync(inputUser),
                    Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


    }
}
