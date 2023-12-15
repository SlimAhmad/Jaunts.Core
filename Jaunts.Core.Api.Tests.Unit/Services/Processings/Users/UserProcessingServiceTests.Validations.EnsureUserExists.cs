using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.User.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {

        [Fact]
        public async Task ShouldThrowValidationExceptionOnEnsureUserExistIfUserIsNullAndLogItAsync()
        {
            // given
            ApplicationUser nullUser = null;

            var nullUserProcessingException =
                new NullUserProcessingException(
                    message: "User is null.");

            var expectedUserProcessingValidationException =
                new UserProcessingValidationException(
                    message: "User validation error occurred, please try again.",
                    innerException: nullUserProcessingException);

            // when
            ValueTask<bool> userExistTask =
                this.userProcessingService.EnsureUserExistAsync(
                    nullUser);

            UserProcessingValidationException actualUserProcessingValidationException =
                await Assert.ThrowsAsync<UserProcessingValidationException>(
                    userExistTask.AsTask);

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
