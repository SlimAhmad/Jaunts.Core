// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Adverts;

namespace Jaunts.Core.Api.Services.Foundations.Adverts
{
    public interface IAdvertService
    {
        ValueTask<Advert> CreateAdvertAsync(Advert advert);
        IQueryable<Advert> RetrieveAllAdverts();
        ValueTask<Advert> RetrieveAdvertByIdAsync(Guid advertId);
        ValueTask<Advert> ModifyAdvertAsync(Advert advert);
        ValueTask<Advert> RemoveAdvertByIdAsync(Guid advertId);
    }
}