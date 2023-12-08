using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Role
{
    public partial class RoleProcessingServiceTests
    {
       
        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveAllRolesIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            IQueryable<ApplicationRole> someRole = CreateRandomRoles();

            var expectedRoleProcessingDependencyException =
                new RoleProcessingDependencyException(
                    message: "Role dependency error occurred, please contact support",
                    dependencyException.InnerException as Xeption);

            this.roleServiceMock.Setup(service =>
                service.RetrieveAllRoles())
                        .Throws(dependencyException);

            // when
            Action actualRoleResponseTask = () =>
                this.roleProcessingService.RetrieveAllRoles();


            RoleProcessingDependencyException
             actualRoleProcessingDependencyException =
                  Assert.Throws<RoleProcessingDependencyException>(
                     actualRoleResponseTask);

            // then
            actualRoleProcessingDependencyException.Should().BeEquivalentTo(
                expectedRoleProcessingDependencyException);

            this.roleServiceMock.Verify(service =>
                service.RetrieveAllRoles(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleProcessingDependencyException))),
                        Times.Once);

            this.roleServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
   
    }
}
