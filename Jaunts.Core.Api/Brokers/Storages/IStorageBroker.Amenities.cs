using Jaunts.Core.Api.Models.Services.Foundations.Amenities;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Amenity> InsertAmenityAsync(
            Amenity amenity);

        IQueryable<Amenity> SelectAllAmenities();

        ValueTask<Amenity> SelectAmenityByIdAsync(
            Guid amenityId);

        ValueTask<Amenity> UpdateAmenityAsync(
            Amenity amenity);

        ValueTask<Amenity> DeleteAmenityAsync(
            Amenity amenity);
    }
}
