// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Amenities;

namespace Jaunts.Core.Api.Services.Foundations.Amenities
{
    public interface IAmenityService
    {
        ValueTask<Amenity> CreateAmenityAsync(Amenity amenity);
        IQueryable<Amenity> RetrieveAllAmenities();
        ValueTask<Amenity> RetrieveAmenityByIdAsync(Guid amenityId);
        ValueTask<Amenity> ModifyAmenityAsync(Amenity amenity);
        ValueTask<Amenity> RemoveAmenityByIdAsync(Guid amenityId);
    }
}