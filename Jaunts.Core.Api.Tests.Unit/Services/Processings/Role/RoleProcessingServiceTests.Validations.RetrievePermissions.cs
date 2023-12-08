using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Role
{
    public partial class RoleProcessingServiceTests
    {
        
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrievePermissionsIfRoleIdIsInvalidAndLogItAsync()
        {
            // given
            List<string> role = null;

            var invalidRoleProcessingException =
                new InvalidRoleProcessingException(
                    message: "Invalid Role, Please correct the errors and try again.");

            invalidRoleProcessingException.AddData(
                key: nameof(ApplicationRole),
                values: "Value is required");

            var expectedRoleProcessingValidationException =
                new RoleProcessingValidationException(
                    message: "Role validation error occurred, please try again.",
                    innerException: invalidRoleProcessingException);

            // when
            ValueTask<int> retrievePermissionsTask =
                this.roleProcessingService.RetrievePermissions(role);

            RoleProcessingValidationException actualRoleProcessingValidationException =
                await Assert.ThrowsAsync<RoleProcessingValidationException>(
                    retrievePermissionsTask.AsTask);

            // then
            actualRoleProcessingValidationException.Should()
                .BeEquivalentTo(expectedRoleProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
              broker.LogError(It.Is(SameExceptionAs(
                  expectedRoleProcessingValidationException))),
                      Times.Once);

            this.roleServiceMock.Verify(service =>
                service.RetrieveAllRoles(),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleServiceMock.VerifyNoOtherCalls();
        }
    }
}
