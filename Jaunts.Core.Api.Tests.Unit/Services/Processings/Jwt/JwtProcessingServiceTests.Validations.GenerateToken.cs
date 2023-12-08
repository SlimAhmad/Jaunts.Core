using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.Jwts.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Jwt
{
    public partial class JwtProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionGenerateTokenIfUserIsNullAsync()
        {
            // given
            ApplicationUser nullUser = null;
            int nullToken = default;

            var nullJwtProcessingException =
                new NullJwtProcessingException();

            var expectedJwtProcessingValidationException =
                new JwtProcessingValidationException(
                    message: "Jwt validation error occurred, please try again.",
                    nullJwtProcessingException);

            // when
            ValueTask<string> jwtTask =
                this.jwtProcessingService.GenerateJwtTokenAsync(nullUser, nullToken);

            JwtProcessingValidationException actualJwtProcessingValidationException =
                await Assert.ThrowsAsync<JwtProcessingValidationException>(
                    jwtTask.AsTask);

            // then
            actualJwtProcessingValidationException.Should()
                .BeEquivalentTo(expectedJwtProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJwtProcessingValidationException))),
                        Times.Once);

            this.jwtServiceMock.Verify(broker =>
                broker.GenerateJwtTokenRequestAsync(
                    It.IsAny<ApplicationUser>(),It.IsAny<int>()),
                        Times.Never);

            this.jwtServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
