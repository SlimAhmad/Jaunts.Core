using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.SignIn
{
    public partial class SignInProcessingServiceTests
    {
        [Fact]
        public async Task ShouldSignOutAsync()
        {
            // given
            this.signInServiceMock.Setup(service =>
                service.SignOutRequestAsync());

            // when
            await this.signInProcessingService.SignOutAsync();

            // then
            this.signInServiceMock.Verify(service =>
                service.SignOutRequestAsync(),
                     Times.Once);

            this.signInServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
