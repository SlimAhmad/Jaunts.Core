// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Jwt
{
    public partial class JwtProcessingServiceTests
    {
        [Fact]
        public async Task ShouldGenerateJwtAsync()
        {
            // given
            ApplicationUser randomUser =CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            int randomNumber = GetRandomNumber();
            int inputNumber = randomNumber;
            string someToken = GetRandomString();

            this.jwtServiceMock.Setup(service =>
               service.GenerateJwtTokenRequestAsync(inputUser,inputNumber))
                   .ReturnsAsync(someToken);

            // when
            string actualJwt =
                await this.jwtProcessingService.GenerateJwtTokenAsync(inputUser,inputNumber);

            // then
            actualJwt.Should().NotBeNullOrEmpty();

            this.jwtServiceMock.Verify(service =>
                service.GenerateJwtTokenRequestAsync(inputUser,inputNumber),
                    Times.Once);

            this.jwtServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
