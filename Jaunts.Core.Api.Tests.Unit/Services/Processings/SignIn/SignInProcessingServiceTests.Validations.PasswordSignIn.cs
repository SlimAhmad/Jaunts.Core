using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.SignIns.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.SignIn
{
    public partial class SignInProcessingServiceTests
    {

        [Fact]
        public async Task ShouldThrowValidationExceptionOnPasswordSignInIfUserIsNullAndLogItAsync()
        {
            // given
            string randomPassword = GetRandomString();
            string inputPassword = randomPassword;
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            ApplicationUser randomUser = null;
            ApplicationUser inputUser = randomUser;
            ApplicationUser expectedUser = inputUser;

            var nullSignInProcessingException =
                new NullSignInProcessingException(
                    message: "SignIn is null.");

            var expectedSignInProcessingValidationException =
                new SignInProcessingValidationException(
                    message: "SignIn validation error occurred, please try again.",
                    innerException: nullSignInProcessingException);

            // when
            ValueTask<bool> passwordSignInTask =
                this.signInProcessingService.PasswordSignInAsync(
                    inputUser,inputPassword,inputBoolean,inputBoolean);

            SignInProcessingValidationException actualSignInProcessingValidationException =
                await Assert.ThrowsAsync<SignInProcessingValidationException>(
                    passwordSignInTask.AsTask);

            // then
            actualSignInProcessingValidationException.Should()
                .BeEquivalentTo(expectedSignInProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSignInProcessingValidationException))),
                        Times.Once);

            this.signInServiceMock.Verify(service =>
                service.PasswordSignInRequestAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.signInServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnPasswordSignInIfPasswordIsInvalidAndLogItAsync()
        {
            // given
            string randomPassword = null;
            string inputPassword = randomPassword;
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser expectedUser = inputUser;

            var invalidSignInProcessingException =
                new InvalidSignInProcessingException(
                    message: "Invalid SignIn, Please correct the errors and try again.");

            invalidSignInProcessingException.AddData(
                key: nameof(ApplicationUser),
                values: "Text is required");

            var expectedSignInProcessingValidationException =
                new SignInProcessingValidationException(
                    message: "SignIn validation error occurred, please try again.",
                    innerException: invalidSignInProcessingException);

            // when
            ValueTask<bool> passwordSignInTask =
                this.signInProcessingService.PasswordSignInAsync(inputUser,inputPassword,inputBoolean,inputBoolean);

            SignInProcessingValidationException actualSignInProcessingValidationException =
                await Assert.ThrowsAsync<SignInProcessingValidationException>(
                    passwordSignInTask.AsTask);

            // then
            actualSignInProcessingValidationException.Should()
                .BeEquivalentTo(expectedSignInProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
              broker.LogError(It.Is(SameExceptionAs(
                  expectedSignInProcessingValidationException))),
                      Times.Once);

            this.signInServiceMock.Verify(service =>
                service.PasswordSignInRequestAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.signInServiceMock.VerifyNoOtherCalls();
        }
    }
}
