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
     
        [Theory]
        [InlineData(null,null)]
        [InlineData("","")]
        [InlineData(" "," ")]
        public async Task ShouldThrowValidationExceptionOnTwoFactorSigInWhenTwoFactorIsInvalidAndLogItAsync(
            string invalidCode,string invalidProvider)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
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
                this.signInService.TwoFactorSignInRequestAsync(invalidProvider, invalidCode,inputBoolean,inputBoolean);

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
