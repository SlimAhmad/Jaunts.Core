using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Tests.Acceptance.Brokers;
using Jaunts.Core.Models.Email;
using System.Diagnostics.Metrics;
using Tynamix.ObjectFiller;
using WireMock.Server;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.Roles
{
    [Collection(nameof(ApiTestCollection))]
    public partial class RolesApiTests
    {
        private readonly JauntsApiBroker jauntsApiBroker; 
        private readonly WireMockServer wireMockServer;

        public RolesApiTests(JauntsApiBroker jauntsApiBroker)
        {
            this.jauntsApiBroker = jauntsApiBroker;
            this.wireMockServer = WireMockServer.Start();
        }
          

        private static ApplicationRole CreateRandomRole() =>
            CreateRoleFiller().Create();

        private static int GetRandomNegativeNumber() =>
             -1 * new IntRange(min: 2, max: 10).GetValue();

        private static Filler<ApplicationRole> CreateRoleFiller()
        {
            var filler = new Filler<ApplicationRole>();

            filler.Setup()
                .OnProperty(x => x.NormalizedName).Use("TEST")
                .OnProperty(x => x.Name).Use("Test")
                .OnProperty(x => x.ConcurrencyStamp).Use(Guid.NewGuid().ToString)
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow);

            return filler;
        }
     

    }
}
