using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ShortLet> ShortLet { get; set; }

        public async ValueTask<ShortLet> InsertShortLetAsync(ShortLet shortLet)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ShortLet> shortLetEntityEntry = await broker.ShortLet.AddAsync(entity: shortLet);
            await broker.SaveChangesAsync();

            return shortLetEntityEntry.Entity;
        }

        public IQueryable<ShortLet> SelectAllShortLets() => this.ShortLet;

        public async ValueTask<ShortLet> SelectShortLetByIdAsync(Guid shortLetId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await ShortLet.FindAsync(shortLetId);
        }

        public async ValueTask<ShortLet> UpdateShortLetAsync(ShortLet shortLet)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ShortLet> shortLetEntityEntry = broker.ShortLet.Update(entity: shortLet);
            await broker.SaveChangesAsync();

            return shortLetEntityEntry.Entity;
        }

        public async ValueTask<ShortLet> DeleteShortLetAsync(ShortLet shortLet)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ShortLet> shortLetEntityEntry = broker.ShortLet.Remove(entity: shortLet);
            await broker.SaveChangesAsync();

            return shortLetEntityEntry.Entity;
        }
    }
}
