// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        private async Task ShouldThrowCriticalDependencyExceptionOnRegisterIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();

            RegisterUserApiRequest randomUser =
                CreateRegisterUserApiRequest(dateTime);

            RegisterUserApiRequest inputUser = randomUser;
            var sqlException = GetSqlException();
            string password = GetRandomString();

            var failedAuthStorageException =
                new FailedAuthStorageException(sqlException);

            var expectedAuthDependencyException =
                new AuthDependencyException(failedAuthStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.userManagementBrokerMock.Setup(broker =>
                broker.RegisterUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<RegisterResultApiResponse> registerAuthTask =
                this.authService.RegisterUserRequestAsync(inputUser);

            // then
            await Assert.ThrowsAsync<AuthDependencyException>(() =>
                registerAuthTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAuthDependencyException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
              broker.RegisterUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                  Times.Once);


            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        private async Task ShouldThrowServiceExceptionOnRegisterIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            RegisterUserApiRequest randomUser = CreateRegisterUserApiRequest(dateTime);
            RegisterUserApiRequest inputUser = randomUser;
            var serviceException = new Exception();

            var failedAuthServiceException =
                new FailedAuthServiceException(
                    message: "Failed Auth service occurred, please contact support",
                    innerException: serviceException);

            var expectedAuthServiceException =
                new AuthServiceException(
                    message: "Auth service error occurred, contact support.",
                    innerException: failedAuthServiceException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.RegisterUserAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                .Returns(dateTime);

            // when
            ValueTask<RegisterResultApiResponse> registerAuthTask =
                 this.authService.RegisterUserRequestAsync(inputUser);

            AuthServiceException actualAuthServiceException =
                await Assert.ThrowsAsync<AuthServiceException>(
                    registerAuthTask.AsTask);

            // then
            actualAuthServiceException.Should().BeEquivalentTo(
                expectedAuthServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuthServiceException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.RegisterUserAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
