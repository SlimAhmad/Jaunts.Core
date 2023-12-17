using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.User.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
        

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveUserByIdIfUserIdIsInvalidAndLogItAsync()
        {
            // given
            Guid userId = Guid.Empty;

            var invalidUserProcessingException =
                new InvalidUserProcessingException(
                    message: "Invalid user, Please correct the errors and try again.");

            invalidUserProcessingException.AddData(
                key: nameof(ApplicationUser.Id),
                values: "Id is required");

            var expectedUserProcessingValidationException =
                new UserProcessingValidationException(
                    message: "User validation error occurred, please try again.",
                    innerException: invalidUserProcessingException);

            // when
            ValueTask<bool> upsertUserTask =
                this.userProcessingService.RemoveUserByIdAsync(userId);

            UserProcessingValidationException actualUserProcessingValidationException =
                await Assert.ThrowsAsync<UserProcessingValidationException>(
                    upsertUserTask.AsTask);

            // then
            actualUserProcessingValidationException.Should()
                .BeEquivalentTo(expectedUserProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
              broker.LogError(It.Is(SameExceptionAs(
                  expectedUserProcessingValidationException))),
                      Times.Once);

            this.userServiceMock.Verify(service =>
                service.RemoveUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
        }
    }
}
