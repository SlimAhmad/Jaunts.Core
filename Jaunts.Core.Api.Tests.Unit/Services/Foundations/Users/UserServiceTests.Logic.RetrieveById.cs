// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Users
{

    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveUserById()
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: dateTime);
            Guid inputUserId = randomUser.Id;
            ApplicationUser inputUser = randomUser;
            ApplicationUser expectedUser = randomUser;

            this.userManagementBrokerMock.Setup(broker =>
                    broker.SelectUserByIdAsync(inputUserId))
                .ReturnsAsync(inputUser);

            //when 
            ApplicationUser actualUser = await this.userService.RetrieveUserByIdRequestAsync(inputUserId);

            //then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(inputUserId), Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
