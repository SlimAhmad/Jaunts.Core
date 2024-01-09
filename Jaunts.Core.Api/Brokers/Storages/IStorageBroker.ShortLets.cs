using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ShortLet> InsertShortLetAsync(
            ShortLet shortLet);

        IQueryable<ShortLet> SelectAllShortLets();

        ValueTask<ShortLet> SelectShortLetByIdAsync(
            Guid customerId);

        ValueTask<ShortLet> UpdateShortLetAsync(
            ShortLet shortLet);

        ValueTask<ShortLet> DeleteShortLetAsync(
            ShortLet shortLet);
    }
}
