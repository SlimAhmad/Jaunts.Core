using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Role
{
    public partial class RoleProcessingServiceTests
    {
        
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfRoleIdIsInvalidAndLogItAsync()
        {
            // given
            Guid roleId = Guid.Empty;

            var invalidRoleProcessingException =
                new InvalidRoleProcessingException(
                    message: "Invalid Role, Please correct the errors and try again.");

            invalidRoleProcessingException.AddData(
                key: nameof(ApplicationRole.Id),
                values: "Id is required");

            var expectedRoleProcessingValidationException =
                new RoleProcessingValidationException(
                    message: "Role validation error occurred, please try again.",
                    innerException: invalidRoleProcessingException);

            // when
            ValueTask<ApplicationRole> upsertRoleTask =
                this.roleProcessingService.RetrieveRoleById(roleId);

            RoleProcessingValidationException actualRoleProcessingValidationException =
                await Assert.ThrowsAsync<RoleProcessingValidationException>(
                    upsertRoleTask.AsTask);

            // then
            actualRoleProcessingValidationException.Should()
                .BeEquivalentTo(expectedRoleProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
              broker.LogError(It.Is(SameExceptionAs(
                  expectedRoleProcessingValidationException))),
                      Times.Once);

            this.roleServiceMock.Verify(service =>
                service.RemoveRoleByIdRequestAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleServiceMock.VerifyNoOtherCalls();
        }
    }
}
