// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.Jwt.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Jwt
{
    public partial class JwtServiceTests
    {
        
        [Fact]
        public async void ShouldThrowValidationExceptionOnGenerateTokenInWhenUserIsNullAndLogItAsync()
        {
            // given
            ApplicationUser invalidUser = null;

            var nullJwtException = new NullJwtException();
            int inputPermissions = GetRandomNumber();
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;  

            var expectedJwtValidationException =
                new JwtValidationException(
                    message: "Jwt validation errors occurred, please try again.",
                    innerException: nullJwtException);

            // when
            ValueTask<string> createJwtTask =
                this.jwtService.GenerateJwtTokenRequestAsync(invalidUser, inputPermissions);

            JwtValidationException actualJwtValidationException =
              await Assert.ThrowsAsync<JwtValidationException>(
                  createJwtTask.AsTask);

            // then
            actualJwtValidationException.Should().BeEquivalentTo(
                expectedJwtValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJwtValidationException))),
                        Times.Once);


            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }     
    }
}
