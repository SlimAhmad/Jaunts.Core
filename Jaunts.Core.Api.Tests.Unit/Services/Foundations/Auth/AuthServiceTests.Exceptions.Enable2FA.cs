// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Auth.LoginRegister;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Auth
{
    public partial class AuthServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnEnable2FAWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            Guid randomGuid = GetRandomGuid();
            var sqlException = GetSqlException();
       

            var failedAuthStorageException =
                new FailedAuthStorageException(sqlException);

            var expectedAuthDependencyException =
                new AuthDependencyException(failedAuthStorageException);

            this.userManagementBrokerMock.Setup(broker =>
                   broker.FindByIdAsync(It.IsAny<string>()))
                       .ReturnsAsync(randomUser);

            this.userManagementBrokerMock.Setup(broker =>
              broker.SetTwoFactorEnabledAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>()))
                  .ReturnsAsync(IdentityResult.Success);

            this.userManagementBrokerMock.Setup(broker =>
                broker.UpdateUserAsync(It.IsAny<ApplicationUser>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Enable2FAApiResponse> registerAuthTask =
                this.authService.EnableUser2FARequestAsync(randomGuid);

            // then
            await Assert.ThrowsAsync<AuthDependencyException>(() =>
                registerAuthTask.AsTask());

            this.userManagementBrokerMock.Verify(broker =>
                  broker.FindByIdAsync(It.IsAny<string>()),
                      Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SetTwoFactorEnabledAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAuthDependencyException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<ApplicationUser>()),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnEnable2FAWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            Guid randomGuid = GetRandomGuid();
            var serviceException = new Exception();
         

            var failedAuthServiceException =
                new FailedAuthServiceException(serviceException);

            var expectedAuthServiceException =
                new AuthServiceException(failedAuthServiceException);

            this.userManagementBrokerMock.Setup(broker =>
               broker.FindByIdAsync(It.IsAny<string>()))
                   .ReturnsAsync(randomUser);

            this.userManagementBrokerMock.Setup(broker =>
              broker.SetTwoFactorEnabledAsync(It.IsAny<ApplicationUser>(),It.IsAny<bool>()))
                  .ReturnsAsync(IdentityResult.Success);

            this.userManagementBrokerMock.Setup(broker =>
                broker.UpdateUserAsync(It.IsAny<ApplicationUser>()))
                    .ThrowsAsync(failedAuthServiceException);

            // when
            ValueTask<Enable2FAApiResponse> registerAuthTask =
                 this.authService.EnableUser2FARequestAsync(randomGuid);

            // then
            await Assert.ThrowsAsync<AuthServiceException>(() =>
                registerAuthTask.AsTask());

            this.userManagementBrokerMock.Verify(broker =>
               broker.FindByIdAsync(It.IsAny<string>()),
                   Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SetTwoFactorEnabledAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuthServiceException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<ApplicationUser>()),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
