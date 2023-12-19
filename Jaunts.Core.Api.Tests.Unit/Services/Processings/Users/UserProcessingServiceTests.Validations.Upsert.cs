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
        public async Task ShouldThrowValidationExceptionOnUpsertIfUserIsNullAndLogItAsync()
        {
            // given
            ApplicationUser nullUser = null;
            string somePassword = GetRandomMessage();

            var nullUserProcessingException =
                new NullUserProcessingException(
                    message: "User is null.");

            var expectedUserProcessingValidationException =
                new UserProcessingValidationException(
                    message: "User validation error occurred, please try again.",
                    innerException: nullUserProcessingException);

            // when
            ValueTask<ApplicationUser> upsertUserTask =
                this.userProcessingService.UpsertUserAsync(
                    nullUser,somePassword);

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
                service.RetrieveAllUsers(),
                    Times.Never);

            this.userServiceMock.Verify(service =>
                service.AddUserAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()),
                    Times.Never);

            this.userServiceMock.Verify(service =>
                service.ModifyUserAsync(It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnUpsertIfUserIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidUser = CreateRandomUser();
            string somePassword = string.Empty;

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
            ValueTask<ApplicationUser> upsertUserTask =
                this.userProcessingService.UpsertUserAsync(invalidUser,somePassword);

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
                service.RetrieveAllUsers(),
                    Times.Never);

            this.userServiceMock.Verify(service =>
                service.AddUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                    Times.Never);

            this.userServiceMock.Verify(service =>
                service.ModifyUserAsync(It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
        }
    }
}
