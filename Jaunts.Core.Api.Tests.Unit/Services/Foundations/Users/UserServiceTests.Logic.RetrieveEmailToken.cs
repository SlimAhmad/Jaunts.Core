// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        private async Task ShouldRetrieveUserEmailAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            ApplicationUser randomUser = CreateRandomUser(dates: dateTime);
            ApplicationUser inputUser = randomUser;
            ApplicationUser storageUser = randomUser;
            ApplicationUser expectedUser = storageUser;
            string randomToken = GetRandomString();
            string inputToken = randomToken;
            string expectedToken = inputToken;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.userManagementBrokerMock.Setup(broker =>
                broker.GenerateEmailConfirmationTokenAsync(inputUser))
                    .ReturnsAsync(expectedToken);

            // when
            string actualToken =
                await this.userService.RetrieveUserEmailConfirmationTokenAsync(inputUser);

            // then
            actualToken.Should().BeEquivalentTo(expectedToken);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.userManagementBrokerMock.Verify(broker =>
                broker.GenerateEmailConfirmationTokenAsync(inputUser),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}