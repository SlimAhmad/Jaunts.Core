using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.SignIns.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.SignIn
{
    public partial class SignInProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnTwoFactorSignInIfValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string randomCode = GetRandomString();
            string inputCode = randomCode;
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            string randomProvider = GetRandomString();
            string inputProvider = randomProvider;
            string expectedProvider = inputProvider;

            var expectedSignInProcessingDependencyValidationException =
                new SignInProcessingDependencyValidationException(
                message: "SignIn dependency validation error occurred, please try again.",
                        dependencyValidationException.InnerException as Xeption);

            this.signInServiceMock.Setup(service =>
                service.TwoFactorSignInRequestAsync(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<bool> actualSignInResponseTask =
                this.signInProcessingService.TwoFactorSignInAsync(inputProvider,inputCode,inputBoolean,inputBoolean);

            SignInProcessingDependencyValidationException
               actualSignInProcessingDependencyValidationException =
                   await Assert.ThrowsAsync<SignInProcessingDependencyValidationException>(
                       actualSignInResponseTask.AsTask);

            // then
            actualSignInProcessingDependencyValidationException.Should().BeEquivalentTo(
                expectedSignInProcessingDependencyValidationException);

            this.signInServiceMock.Verify(service =>
                service.TwoFactorSignInRequestAsync(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSignInProcessingDependencyValidationException))),
                        Times.Once);

            this.signInServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnTwoFactorSignInIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string randomCode = GetRandomString();
            string inputCode = randomCode;
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            string randomProvider = GetRandomString();
            string inputProvider = randomProvider;
            string expectedProvider = inputProvider;

            var expectedSignInProcessingDependencyException =
                new SignInProcessingDependencyException(
                    message: "SignIn dependency error occurred, please contact support",
                    dependencyException.InnerException as Xeption);

            this.signInServiceMock.Setup(service =>
                service.TwoFactorSignInRequestAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()))
                        .Throws(dependencyException);

            // when
            ValueTask<bool> actualSignInResponseTask =
                this.signInProcessingService.TwoFactorSignInAsync(inputProvider, inputCode, inputBoolean, inputBoolean);


            SignInProcessingDependencyException
             actualSignInProcessingDependencyException =
                 await Assert.ThrowsAsync<SignInProcessingDependencyException>(
                     actualSignInResponseTask.AsTask);

            // then
            actualSignInProcessingDependencyException.Should().BeEquivalentTo(
                expectedSignInProcessingDependencyException);

            this.signInServiceMock.Verify(service =>
                service.TwoFactorSignInRequestAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSignInProcessingDependencyException))),
                        Times.Once);

            this.signInServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnTwoFactorSignInIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string randomCode = GetRandomString();
            string inputCode = randomCode;
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            string randomProvider = GetRandomString();
            string inputProvider = randomProvider;
            string expectedProvider = inputProvider;

            var serviceException = new Exception();

            var failedSignInProcessingServiceException =
                new FailedSignInProcessingServiceException(
                    message: "Failed signIn service occurred, please contact support",
                    innerException: serviceException);

            var expectedSignInProcessingServiceException =
                new SignInProcessingServiceException(
                    message: "Failed signIn service occurred, please contact support",
                    innerException: failedSignInProcessingServiceException);

            this.signInServiceMock.Setup(service =>
                service.TwoFactorSignInRequestAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()))
                    .Throws(serviceException);

            // when
            ValueTask<bool> actualSignInResponseTask =
                this.signInProcessingService.TwoFactorSignInAsync(inputProvider,inputCode,inputBoolean,inputBoolean);

            SignInProcessingServiceException
               actualSignInProcessingServiceException =
                   await Assert.ThrowsAsync<SignInProcessingServiceException>(
                       actualSignInResponseTask.AsTask);

            // then
            actualSignInProcessingServiceException.Should().BeEquivalentTo(
                 expectedSignInProcessingServiceException);

            this.signInServiceMock.Verify(service =>
                service.TwoFactorSignInRequestAsync(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()),
                         Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSignInProcessingServiceException))),
                        Times.Once);

            this.signInServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
