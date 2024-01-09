// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Models.Auth.LoginRegister;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Aggregation.Account
{
    public partial class AccountAggregationServiceTests
    {
        [Fact]
        private async Task ShouldResetPasswordAsync()
        {
            // given
            ResetPasswordRequest resetPasswordApiRequest =
               CreateResetPasswordApiRequest();
             
            bool randomBoolean = GetRandomBoolean();
            bool InputBoolean = randomBoolean;
            bool storageBoolean = InputBoolean;

            this.userOrchestrationMock.Setup(broker =>
                broker.ResetUserPasswordByEmailOrUserNameAsync(resetPasswordApiRequest))
                    .ReturnsAsync(randomBoolean);
            // when
            bool actualAuth =
                await this.accountAggregationService.ResetPasswordRequestAsync(resetPasswordApiRequest);

            // then
            actualAuth.Should().Be(storageBoolean);

            this.userOrchestrationMock.Verify(broker =>
                broker.ResetUserPasswordByEmailOrUserNameAsync(It.IsAny<ResetPasswordRequest>()),
                    Times.Once);
 
            this.userOrchestrationMock.VerifyNoOtherCalls();
            this.jwtOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
