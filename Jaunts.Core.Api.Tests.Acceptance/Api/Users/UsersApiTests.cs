using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;
using WireMock.Server;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.Users
{
    [Collection(nameof(ApiTestCollection))]
    public partial class UsersApiTests
    {
        private readonly JauntsApiBroker jauntsApiBroker; 
        private readonly WireMockServer wireMockServer;

        public UsersApiTests(JauntsApiBroker jauntsApiBroker)
        {
            this.jauntsApiBroker = jauntsApiBroker;
            this.wireMockServer = WireMockServer.Start();
        }
          


        private static int GetRandomNegativeNumber() =>
             -1 * new IntRange(min: 2, max: 10).GetValue();
        private static DateTimeOffset GetCurrentDateTime() => DateTimeOffset.UtcNow;
        private static string GetRandomNames() => new RealNames().GetValue();
        private static string GetRandomString() => new MnemonicString().GetValue();
        private static string GetRandomEmailAddresses() => new EmailAddresses().GetValue();

        private static ApplicationUser CreateRandomUser(DateTimeOffset date)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = GetRandomEmailAddresses(),
                LastName = GetRandomNames(),
                FirstName = GetRandomNames(),
                UserName = GetRandomString(),
                PhoneNumber = GetRandomString(),
                CreatedDate = date,
                UpdatedDate = date
            };

            return user;
        }
    }
}
