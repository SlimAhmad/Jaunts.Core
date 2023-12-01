// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.SignIn.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.SignIn
{
    public partial class SignInServiceTests
    {
        
        [Fact]
        public async void ShouldThrowValidationExceptionOnPasswordSignInWhenSignInIsNullAndLogItAsync()
        {
            // given
            ApplicationUser invalidSignIn = null;

            var nullSignInException = new NullSignInException();
            string inputPassword = GetRandomPassword();
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;  

            var expectedSignInValidationException =
                new SignInValidationException(
                    message: "SignIn validation errors occurred, please try again.",
                    innerException: nullSignInException);

            // when
            ValueTask<SignInResult> createSignInTask =
                this.signInService.PasswordSignInRequestAsync(invalidSignIn, inputPassword,inputBoolean,inputBoolean);

            SignInValidationException actualSignInValidationException =
              await Assert.ThrowsAsync<SignInValidationException>(
                  createSignInTask.AsTask);

            // then
            actualSignInValidationException.Should().BeEquivalentTo(
                expectedSignInValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSignInValidationException))),
                        Times.Once);

            this.signInBrokerMock.Verify(broker =>
                broker.PasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.signInBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnPasswordSigInWhenPasswordIsInvalidAndLogItAsync(
            string invalidPassword)
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime(); 
            ApplicationUser randomSignIn = CreateRandomUser();
            ApplicationUser invalidSignIn = randomSignIn;
            string inputPassword = invalidPassword;
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;

            var invalidSignInException =
                new InvalidSignInException(
                    message: "Invalid SignIn. Please correct the errors and try again.");

            invalidSignInException.AddData(
               key: nameof(SignInResult),
               values: "Text is required");

            var expectedSignInValidationException =
                new SignInValidationException(
                    message: "SignIn validation errors occurred, please try again.",
                    innerException: invalidSignInException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<SignInResult> registerSignInTask =
                this.signInService.PasswordSignInRequestAsync(invalidSignIn, inputPassword,inputBoolean,inputBoolean);

            SignInValidationException actualSignInValidationException =
               await Assert.ThrowsAsync<SignInValidationException>(
                   registerSignInTask.AsTask);

            // then
            actualSignInValidationException.Should().BeEquivalentTo(
                expectedSignInValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSignInValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.signInBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

     
    }
}
