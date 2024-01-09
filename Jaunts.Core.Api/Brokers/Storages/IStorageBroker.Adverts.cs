using Jaunts.Core.Api.Models.Services.Foundations.Adverts;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Advert> InsertAdvertAsync(Advert advert);
        IQueryable<Advert> SelectAllAdverts();
        ValueTask<Advert> SelectAdvertByIdAsync(Guid advertId);
        ValueTask<Advert> UpdateAdvertAsync(Advert advert);
        ValueTask<Advert> DeleteAdvertAsync(Advert advert);
    }
}
