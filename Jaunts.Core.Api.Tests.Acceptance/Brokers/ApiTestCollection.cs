using Xunit;

namespace Jaunts.Core.Api.Tests.Acceptance.Brokers
{
    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<JauntsApiBroker>
    {
    }
}
