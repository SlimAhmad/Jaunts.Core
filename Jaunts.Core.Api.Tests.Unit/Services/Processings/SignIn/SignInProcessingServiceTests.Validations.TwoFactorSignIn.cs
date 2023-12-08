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
        public async Task ShouldThrowValidationExceptionOnTwoFactorSignInIfTwoFactorIsInvalidAndLogItAsync()
        {
            // given
            string randomCode = null;
            string inputCode = randomCode;
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            string randomProvider = null;
            string inputProvider = randomProvider;
            string expectedProvider = inputProvider;

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
                this.signInProcessingService.TwoFactorSignInAsync(inputProvider,inputCode,inputBoolean,inputBoolean);

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
                service.TwoFactorSignInRequestAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.signInServiceMock.VerifyNoOtherCalls();
        }
    }
}
