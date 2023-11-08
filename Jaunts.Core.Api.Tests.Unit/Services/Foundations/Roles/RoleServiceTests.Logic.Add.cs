// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{
    public partial class RoleServiceTests
    {
        [Fact]
        public async Task ShouldRegisterRoleAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            ApplicationRole randomRole = CreateRandomRole(dates: dateTime);
            ApplicationRole inputRole = randomRole;
            ApplicationRole storageRole = randomRole;
            ApplicationRole expectedRole = storageRole;
            string password = GetRandomPassword();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.InsertRoleAsync(inputRole))
                    .ReturnsAsync(storageRole);

            // when
            ApplicationRole actualRole =
                await this.roleService.RegisterRoleRequestAsync(inputRole);

            // then
            actualRole.Should().BeEquivalentTo(expectedRole);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.InsertRoleAsync(inputRole),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
