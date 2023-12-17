using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.User.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
        
        [Fact]
        public async Task ShouldThrowValidationExceptionOnEnable2FAIfUserIdIsInvalidAndLogItAsync()
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
            ValueTask<ApplicationUser> retrievePermissionsTask =
                this.userProcessingService.EnableOrDisable2FactorAuthenticationAsync(userId);

            UserProcessingValidationException actualUserProcessingValidationException =
                await Assert.ThrowsAsync<UserProcessingValidationException>(
                    retrievePermissionsTask.AsTask);

            // then
            actualUserProcessingValidationException.Should()
                .BeEquivalentTo(expectedUserProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
              broker.LogError(It.Is(SameExceptionAs(
                  expectedUserProcessingValidationException))),
                      Times.Once);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(),
                    Times.Never);

            this.userServiceMock.Verify(service =>
                service.ModifyUserTwoFactorAsync(It.IsAny<ApplicationUser>(),It.IsAny<bool>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
        }
    }
}
