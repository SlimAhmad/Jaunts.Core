// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.SignIn
{
    public partial class SignInServiceTests
    {
        [Fact]
        public async Task ShouldSignOutAsync()
        {
            // given
            this.signInBrokerMock.Setup(broker =>
                broker.SignOutAsync());

            // when
            await this.signInService.SignOutRequestAsync();

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.signInBrokerMock.Verify(broker =>
                broker.SignOutAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.signInBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
