using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.User.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Threading.Tasks;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnUpsertIfValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            ApplicationUser someUser= CreateRandomUser();
            string randomPassword = GetRandomString();
            string somePassword = randomPassword;

            var expectedUserProcessingDependencyValidationException =
                new UserProcessingDependencyValidationException(
                message: "User dependency validation error occurred, please try again.",
                        dependencyValidationException.InnerException as Xeption);

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<ApplicationUser> actualUserResponseTask =
                this.userProcessingService.UpsertUserAsync(someUser,somePassword);

            UserProcessingDependencyValidationException
               actualUserProcessingDependencyValidationException =
                   await Assert.ThrowsAsync<UserProcessingDependencyValidationException>(
                       actualUserResponseTask.AsTask);

            // then
            actualUserProcessingDependencyValidationException.Should().BeEquivalentTo(
                expectedUserProcessingDependencyValidationException);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserProcessingDependencyValidationException))),
                        Times.Once);

            this.userServiceMock.Verify(service =>
                service.InsertUserRequestAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()),
                    Times.Never);

            this.userServiceMock.Verify(service =>
                service.ModifyUserRequestAsync(It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnUpsertIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            ApplicationUser someUser = CreateRandomUser();
            string randomPassword = GetRandomString();
            string somePassword = randomPassword;

            var expectedUserProcessingDependencyException =
                new UserProcessingDependencyException(
                    message: "User dependency error occurred, please contact support",
                    dependencyException.InnerException as Xeption);

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                        .Throws(dependencyException);

            // when
            ValueTask<ApplicationUser> actualUserResponseTask =
                this.userProcessingService.UpsertUserAsync(someUser,somePassword);


            UserProcessingDependencyException
             actualUserProcessingDependencyException =
                 await Assert.ThrowsAsync<UserProcessingDependencyException>(
                     actualUserResponseTask.AsTask);

            // then
            actualUserProcessingDependencyException.Should().BeEquivalentTo(
                expectedUserProcessingDependencyException);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserProcessingDependencyException))),
                        Times.Once);

            this.userServiceMock.Verify(service =>
                service.InsertUserRequestAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()),
                    Times.Never);

            this.userServiceMock.Verify(service =>
                service.ModifyUserRequestAsync(It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnUpsertIfServiceErrorOccursAndLogItAsync()
        {
            // given
            ApplicationUser someUser = CreateRandomUser();
            string randomPassword = GetRandomString();
            string somePassword = randomPassword;

            var serviceException = new Exception();

            var failedUserProcessingServiceException =
                new FailedUserProcessingServiceException(
                    message: "Failed user service occurred, please contact support",
                    innerException: serviceException);

            var expectedUserProcessingServiceException =
                new UserProcessingServiceException(
                    message: "Failed user service occurred, please contact support",
                    innerException: failedUserProcessingServiceException);

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Throws(serviceException);

            // when
            ValueTask<ApplicationUser> actualUserResponseTask =
                this.userProcessingService.UpsertUserAsync(someUser,somePassword);

            UserProcessingServiceException
               actualUserProcessingServiceException =
                   await Assert.ThrowsAsync<UserProcessingServiceException>(
                       actualUserResponseTask.AsTask);

            // then
            actualUserProcessingServiceException.Should().BeEquivalentTo(
                 expectedUserProcessingServiceException);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserProcessingServiceException))),
                        Times.Once);

            this.userServiceMock.Verify(service =>
                service.InsertUserRequestAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()),
                    Times.Never);

            this.userServiceMock.Verify(service =>
                service.ModifyUserRequestAsync(It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
