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

        [Theory]
        [InlineData(null, null)]
        [InlineData("","")]
        public async Task ShouldThrowValidationExceptionOnConfirmEmailIfUserIdIsInvalidAndLogItAsync(string token,string email)
        {
            // given
            var invalidUserProcessingException =
                new InvalidUserProcessingException(
                    message: "Invalid user, Please correct the errors and try again.");

            invalidUserProcessingException.AddData(
                key: nameof(ApplicationUser),
                values: "Value is required");

            var expectedUserProcessingValidationException =
                new UserProcessingValidationException(
                    message: "User validation error occurred, please try again.",
                    innerException: invalidUserProcessingException);

            // when
            ValueTask<ApplicationUser> retrievePermissionsTask =
                this.userProcessingService.ConfirmEmailAsync(token,email);

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

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
        }
    }
}
