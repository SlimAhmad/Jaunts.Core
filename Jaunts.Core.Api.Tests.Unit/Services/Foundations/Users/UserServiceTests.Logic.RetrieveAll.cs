// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllUsers()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<ApplicationUser> randomUsers = CreateRandomUsers(dates: randomDateTime);
            IQueryable<ApplicationUser> storageUsers = randomUsers;
            IQueryable<ApplicationUser> expectedUsers = storageUsers;

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectAllUsers())
                    .Returns(storageUsers);

            // when
            IQueryable<ApplicationUser> actualUsers =
                this.userService.RetrieveAllUsers();

            // then
            actualUsers.Should().BeEquivalentTo(expectedUsers);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectAllUsers(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
