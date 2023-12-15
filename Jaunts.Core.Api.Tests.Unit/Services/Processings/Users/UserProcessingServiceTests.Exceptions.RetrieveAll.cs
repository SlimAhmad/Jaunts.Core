using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.User.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Linq;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
       
        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveAllUsersIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            IQueryable<ApplicationUser> someUser = CreateRandomUsers();

            var expectedUserProcessingDependencyException =
                new UserProcessingDependencyException(
                    message: "User dependency error occurred, please contact support",
                    dependencyException.InnerException as Xeption);

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                        .Throws(dependencyException);

            // when
            Action actualUserResponseTask = () =>
                this.userProcessingService.RetrieveAllUsers();


            UserProcessingDependencyException
             actualUserProcessingDependencyException =
                  Assert.Throws<UserProcessingDependencyException>(
                     actualUserResponseTask);

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
   
    }
}
