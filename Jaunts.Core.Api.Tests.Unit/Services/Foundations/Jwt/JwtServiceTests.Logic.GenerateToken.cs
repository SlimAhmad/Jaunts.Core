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

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Jwt
{
    public partial class JwtServiceTests
    {
        [Fact]
        public async Task ShouldGenerateJwtAsync()
        {
            // given
            ApplicationUser randomUser =CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            int randomNumber = GetRandomNumber();
            int inputNumber = randomNumber;

            // when
            string actualJwt =
                await this.jwtService.GenerateJwtTokenRequestAsync(inputUser,inputNumber);

            // then
            actualJwt.Should().NotBeNullOrEmpty();

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
