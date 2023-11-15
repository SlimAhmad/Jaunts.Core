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
        public async Task ShouldThrowDependencyExceptionOnLoginWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser();
            LoginCredentialsApiRequest randomUserrRegistration = CreateLoginCredentialsApiRequest();
            LoginCredentialsApiRequest inputUser = randomUserrRegistration;
            var sqlException = GetSqlException();


            var failedAuthStorageException =
                new FailedAuthStorageException(sqlException);

            var expectedAuthDependencyException =
                new AuthDependencyException(failedAuthStorageException);



            this.userManagementBrokerMock.Setup(broker =>
                broker.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(randomUser);

            this.userManagementBrokerMock.Setup(broker =>
                broker.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<UserProfileDetailsApiResponse> loginAuthTask =
                this.authService.LogInRequestAsync(inputUser);

            // then
            await Assert.ThrowsAsync<AuthDependencyException>(() =>
                loginAuthTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAuthDependencyException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
              broker.FindByNameAsync(It.IsAny<string>()),
                  Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
              broker.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                  Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnLoginWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser();
            LoginCredentialsApiRequest randomUserRegistration = CreateLoginCredentialsApiRequest();
            LoginCredentialsApiRequest inputUser = randomUserRegistration;
            var serviceException = new Exception();


            var failedAuthServiceException =
                new FailedAuthServiceException(serviceException);

            var expectedAuthServiceException =
                new AuthServiceException(failedAuthServiceException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(randomUser);

            this.userManagementBrokerMock.Setup(broker =>
                broker.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<UserProfileDetailsApiResponse> loginAuthTask =
                 this.authService.LogInRequestAsync(inputUser);

            // then
            await Assert.ThrowsAsync<AuthServiceException>(() =>
                loginAuthTask.AsTask());


            this.userManagementBrokerMock.Verify(broker =>
              broker.FindByNameAsync(It.IsAny<string>()),
                  Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
              broker.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
             broker.LogError(It.Is(SameExceptionAs(
                 expectedAuthServiceException))),
                     Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
