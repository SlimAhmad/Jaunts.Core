// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
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
        private async Task ShouldConfirmEmailAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            List<string> randomRoleList = CreateRandomStringList();
            string token = GetRandomString();
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
                broker.ConfirmEmailAsync(token,email))
                    .ReturnsAsync(storageUser);

            this.jwtOrchestrationMock.Setup(broker =>
                broker.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()))
                        .ReturnsAsync(randomUserProfileDetails);

            // when
            UserAccountDetailsResponse actualAuth =
                await this.accountAggregationService.ConfirmEmailAsync(token, email);

            // then
            actualAuth.Should().BeEquivalentTo(expectedUserProfileDetails);

            this.userOrchestrationMock.Verify(broker =>
                broker.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()),
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
