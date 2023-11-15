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
        public async Task ShouldThrowDependencyExceptionOnConfirmEmailWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            string randomToken = GetRandomString();
            string randomEmail = GetRandomEmailAddresses();
            var sqlException = GetSqlException();


            var failedAuthStorageException =
                new FailedAuthStorageException(sqlException);

            var expectedAuthDependencyException =
                new AuthDependencyException(failedAuthStorageException);

            this.userManagementBrokerMock.Setup(broker =>
               broker.FindByEmailAsync(It.IsAny<string>()))
                   .ReturnsAsync(randomUser);

            this.userManagementBrokerMock.Setup(broker =>
                broker.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<UserProfileDetailsApiResponse> registerAuthTask =
                this.authService.ConfirmEmailRequestAsync(randomToken, randomEmail);

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
              broker.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                  Times.Once);


            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnConfirmEmailWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            string randomToken = GetRandomString();
            string randomEmail = GetRandomEmailAddresses();
            var serviceException = new Exception();


            var failedAuthServiceException =
                new FailedAuthServiceException(serviceException);

            var expectedAuthServiceException =
                new AuthServiceException(failedAuthServiceException);

            this.userManagementBrokerMock.Setup(broker =>
            broker.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(randomUser);

            this.userManagementBrokerMock.Setup(broker =>
                broker.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                    .ThrowsAsync(failedAuthServiceException);

            // when
            ValueTask<UserProfileDetailsApiResponse> registerAuthTask =
                 this.authService.ConfirmEmailRequestAsync(randomToken, randomEmail);

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
              broker.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                  Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
