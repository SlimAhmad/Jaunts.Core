using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Processings.User.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnEnsureUserExistIfValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();

            IQueryable<ApplicationUser> randomUsers =
                CreateRandomUsers(inputUser);

            IQueryable<ApplicationUser> retrievedUsers =
                randomUsers;

            var expectedUserProcessingDependencyValidationException =
                new UserProcessingDependencyValidationException(
                message: "User dependency validation error occurred, please try again.",
                        dependencyValidationException.InnerException as Xeption);


            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<bool> actualTokenTask =
                this.userProcessingService.EnsureUserExistAsync(inputUser);

            UserProcessingDependencyValidationException
               actualUserProcessingDependencyValidationException =
                   await Assert.ThrowsAsync<UserProcessingDependencyValidationException>(
                       actualTokenTask.AsTask);

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

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnEnsureUserExistIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();

            IQueryable<ApplicationUser> randomUsers =
                CreateRandomUsers(inputUser);

            IQueryable<ApplicationUser> retrievedUsers =
                randomUsers;

            var expectedUserProcessingDependencyException =
                new UserProcessingDependencyException(
                    message: "User dependency error occurred, please contact support",
                    dependencyException.InnerException as Xeption);

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Throws(dependencyException);

            // when
            ValueTask<bool> actualTokenTask =
                this.userProcessingService.EnsureUserExistAsync(inputUser);


            UserProcessingDependencyException
             actualUserProcessingDependencyException =
                 await Assert.ThrowsAsync<UserProcessingDependencyException>(
                     actualTokenTask.AsTask);

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

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnEnsureUserExistIfServiceErrorOccursAndLogItAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();

            IQueryable<ApplicationUser> randomUsers =
                CreateRandomUsers(inputUser);

            IQueryable<ApplicationUser> retrievedUsers =
                randomUsers;

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
            ValueTask<bool> actualTokenTask =
                this.userProcessingService.EnsureUserExistAsync(inputUser);

            UserProcessingServiceException
               actualUserProcessingServiceException =
                   await Assert.ThrowsAsync<UserProcessingServiceException>(
                       actualTokenTask.AsTask);

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

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
