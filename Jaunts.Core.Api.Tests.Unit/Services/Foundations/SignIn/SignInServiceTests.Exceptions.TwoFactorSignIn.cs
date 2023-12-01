using FluentAssertions;
using Jaunts.Core.Api.Models.SignIn.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.SignIn
{
    public partial class SignInServiceTests
    {


        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            string randomProvider = GetRandomString();
            string inputProvider = randomProvider;
            var serviceException = new Exception();
            string randomCode = GetRandomString();
            string inputCode = randomCode;
            bool inputBoolean = GetRandomBoolean();

            var failedSignInServiceException =
                  new FailedSignInServiceException(
                      message: "Failed SignIn service occurred, please contact support",
                      innerException: serviceException);

            var expectedSignInServiceException =
                new SignInServiceException(
                    message: "SignIn service error occurred, contact support.",
                    innerException: failedSignInServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.signInBrokerMock.Setup(broker =>
                broker.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SignInResult> registerSignInTask =
                 this.signInService.TwoFactorSignInRequestAsync(inputProvider, inputCode,inputBoolean,inputBoolean);

            SignInServiceException actualSignInServiceException =
              await Assert.ThrowsAsync<SignInServiceException>(
                  registerSignInTask.AsTask);

            // then
            actualSignInServiceException.Should().BeEquivalentTo(
                expectedSignInServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSignInServiceException))),
                        Times.Once);

            this.signInBrokerMock.Verify(broker =>
                broker.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.signInBrokerMock.VerifyNoOtherCalls();
        }

    }
}
