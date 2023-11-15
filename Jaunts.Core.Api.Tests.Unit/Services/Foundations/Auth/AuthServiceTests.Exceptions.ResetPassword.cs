// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Auth.LoginRegister;
using Moq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Auth
{
    public partial class AuthServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnResetPasswordWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            ResetPasswordApiRequest randomUserReset = CreateResetPasswordApiRequest();
            ResetPasswordApiRequest inputUser = randomUserReset;
            var sqlException = GetSqlException();


            var failedAuthStorageException =
                new FailedAuthStorageException(sqlException);

            var expectedAuthDependencyException =
                new AuthDependencyException(failedAuthStorageException);

            this.userManagementBrokerMock.Setup(broker =>
               broker.FindByEmailAsync(It.IsAny<string>()))
                   .ReturnsAsync(randomUser);

            this.userManagementBrokerMock.Setup(broker =>
                broker.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ResetPasswordApiResponse> registerAuthTask =
                this.authService.ResetPasswordRequestAsync(inputUser);

            // then
            await Assert.ThrowsAsync<AuthDependencyException>(() =>
                registerAuthTask.AsTask());

            this.userManagementBrokerMock.Verify(broker =>
                 broker.FindByEmailAsync(It.IsAny<string>()),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAuthDependencyException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
              broker.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()),
                  Times.Once);


            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnResetPasswordWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            ResetPasswordApiRequest randomUserReset = CreateResetPasswordApiRequest();
            ResetPasswordApiRequest inputUser = randomUserReset;
            var serviceException = new Exception();


            var failedAuthServiceException =
                new FailedAuthServiceException(serviceException);

            var expectedAuthServiceException =
                new AuthServiceException(failedAuthServiceException);

            this.userManagementBrokerMock.Setup(broker =>
            broker.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(randomUser);

            this.userManagementBrokerMock.Setup(broker =>
                broker.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ThrowsAsync(failedAuthServiceException);

            // when
            ValueTask<ResetPasswordApiResponse> registerAuthTask =
                 this.authService.ResetPasswordRequestAsync(inputUser);

            // then
            await Assert.ThrowsAsync<AuthServiceException>(() =>
                registerAuthTask.AsTask());

            this.userManagementBrokerMock.Verify(broker =>
           broker.FindByEmailAsync(It.IsAny<string>()),
               Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuthServiceException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
              broker.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()),
                  Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
