// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Auth
{
    public partial class AuthServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnLoginWithOTPWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            string randomCode = GetRandomString();
            string randomUsernameOrEmail = GetRandomEmailAddresses();
            var sqlException = GetSqlException();


            var failedAuthStorageException =
                new FailedAuthStorageException(sqlException);

            var expectedAuthDependencyException =
                new AuthDependencyException(failedAuthStorageException);

            this.userManagementBrokerMock.Setup(broker =>
               broker.FindByEmailAsync(It.IsAny<string>()))
                   .ThrowsAsync(sqlException);

            // when
            ValueTask<UserProfileDetailsApiResponse> registerAuthTask =
                this.authService.LoginWithOTPRequestAsync(randomCode, randomUsernameOrEmail);

            // then
            await Assert.ThrowsAsync<AuthDependencyException>(() =>
                registerAuthTask.AsTask());

            this.signInManagerBrokerMock.Verify(broker =>
                 broker.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAuthDependencyException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                        broker.FindByEmailAsync(It.IsAny<string>()),
                            Times.Once);



            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.signInManagerBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnLoginWithOTPWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            string randomCode = GetRandomString();
            string randomUsernameOrEmail = GetRandomEmailAddresses();
            var serviceException = new Exception();


            var failedAuthServiceException =
                new FailedAuthServiceException(serviceException);

            var expectedAuthServiceException =
                new AuthServiceException(failedAuthServiceException);

            this.userManagementBrokerMock.Setup(broker =>
               broker.FindByEmailAsync(It.IsAny<string>()))
                   .ThrowsAsync(failedAuthServiceException);

            // when
            ValueTask<UserProfileDetailsApiResponse> registerAuthTask =
                 this.authService.LoginWithOTPRequestAsync(randomCode, randomUsernameOrEmail);

            // then
            await Assert.ThrowsAsync<AuthServiceException>(() =>
                registerAuthTask.AsTask());

            this.userManagementBrokerMock.Verify(broker =>
           broker.FindByEmailAsync(It.IsAny<string>()),
               Times.Once);


            this.signInManagerBrokerMock.Verify(broker =>
                 broker.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuthServiceException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                        broker.FindByEmailAsync(It.IsAny<string>()),
                            Times.Once);


            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.signInManagerBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
