using Jaunts.Core.Api.Models.Services.Foundations.Adverts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Advert> Adverts { get; set; }

        public async ValueTask<Advert> InsertAdvertAsync(Advert adverts)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Advert> advertsEntityEntry = await broker.Adverts.AddAsync(entity: adverts);
            await broker.SaveChangesAsync();

            return advertsEntityEntry.Entity;
        }

        public IQueryable<Advert> SelectAllAdverts() => this.Adverts;

        public async ValueTask<Advert> SelectAdvertByIdAsync(Guid advertsId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Adverts.FindAsync(advertsId);
        }

        public async ValueTask<Advert> UpdateAdvertAsync(Advert adverts)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Advert> advertsEntityEntry = broker.Adverts.Update(entity: adverts);
            await broker.SaveChangesAsync();

            return advertsEntityEntry.Entity;
        }

        public async ValueTask<Advert> DeleteAdvertAsync(Advert adverts)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Advert> advertsEntityEntry = broker.Adverts.Remove(entity: adverts);
            await broker.SaveChangesAsync();

            return advertsEntityEntry.Entity;
        }
    }
}
