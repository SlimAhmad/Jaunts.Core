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
        public void ShouldRetrieveAllRoles()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<ApplicationRole> randomRoles = CreateRandomRoles(dates: randomDateTime);
            IQueryable<ApplicationRole> storageRoles = randomRoles;
            IQueryable<ApplicationRole> expectedRoles = storageRoles;

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectAllRoles())
                    .Returns(storageRoles);

            // when
            IQueryable<ApplicationRole> actualRoles =
                this.roleService.RetrieveAllRoles();

            // then
            actualRoles.Should().BeEquivalentTo(expectedRoles);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectAllRoles(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
