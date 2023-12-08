using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.SignIns.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
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
        public async Task ShouldThrowDependencyValidationExceptionOnPasswordSignInIfValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string randomPassword = GetRandomString();
            string inputPassword = randomPassword;
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser expectedUser = inputUser;

            var expectedSignInProcessingDependencyValidationException =
                new SignInProcessingDependencyValidationException(
                message: "SignIn dependency validation error occurred, please try again.",
                        dependencyValidationException.InnerException as Xeption);

            this.signInServiceMock.Setup(service =>
                service.PasswordSignInRequestAsync(
                    It.IsAny<ApplicationUser>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<bool> actualSignInResponseTask =
                this.signInProcessingService.PasswordSignInAsync(inputUser,inputPassword,inputBoolean,inputBoolean);

            SignInProcessingDependencyValidationException
               actualSignInProcessingDependencyValidationException =
                   await Assert.ThrowsAsync<SignInProcessingDependencyValidationException>(
                       actualSignInResponseTask.AsTask);

            // then
            actualSignInProcessingDependencyValidationException.Should().BeEquivalentTo(
                expectedSignInProcessingDependencyValidationException);

            this.signInServiceMock.Verify(service =>
                service.PasswordSignInRequestAsync(
                    It.IsAny<ApplicationUser>(), It.IsAny<string>(),
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
        public async Task ShouldThrowDependencyExceptionOnPasswordSignInIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string randomPassword = GetRandomString();
            string inputPassword = randomPassword;
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser expectedUser = inputUser;

            var expectedSignInProcessingDependencyException =
                new SignInProcessingDependencyException(
                    message: "SignIn dependency error occurred, please contact support",
                    dependencyException.InnerException as Xeption);

            this.signInServiceMock.Setup(service =>
                service.PasswordSignInRequestAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()))
                        .Throws(dependencyException);

            // when
            ValueTask<bool> actualSignInResponseTask =
                this.signInProcessingService.PasswordSignInAsync(inputUser, inputPassword, inputBoolean, inputBoolean);


            SignInProcessingDependencyException
             actualSignInProcessingDependencyException =
                 await Assert.ThrowsAsync<SignInProcessingDependencyException>(
                     actualSignInResponseTask.AsTask);

            // then
            actualSignInProcessingDependencyException.Should().BeEquivalentTo(
                expectedSignInProcessingDependencyException);

            this.signInServiceMock.Verify(service =>
                service.PasswordSignInRequestAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(),
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
        public async Task ShouldThrowServiceExceptionOnPasswordSignInIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string randomPassword = GetRandomString();
            string inputPassword = randomPassword;
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser expectedUser = inputUser;

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
                service.PasswordSignInRequestAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()))
                    .Throws(serviceException);

            // when
            ValueTask<bool> actualSignInResponseTask =
                this.signInProcessingService.PasswordSignInAsync(inputUser,inputPassword,inputBoolean,inputBoolean);

            SignInProcessingServiceException
               actualSignInProcessingServiceException =
                   await Assert.ThrowsAsync<SignInProcessingServiceException>(
                       actualSignInResponseTask.AsTask);

            // then
            actualSignInProcessingServiceException.Should().BeEquivalentTo(
                 expectedSignInProcessingServiceException);

            this.signInServiceMock.Verify(service =>
                service.PasswordSignInRequestAsync(
                    It.IsAny<ApplicationUser>(), It.IsAny<string>(),
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
