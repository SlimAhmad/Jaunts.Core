// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;

namespace Jaunts.Core.Api.Services.Foundations.ShortLets
{
    public interface IShortLetService 
    {
        ValueTask<ShortLet> CreateShortLetAsync(ShortLet shortLet);
        IQueryable<ShortLet> RetrieveAllShortLets();
        ValueTask<ShortLet> RetrieveShortLetByIdAsync(Guid shortLetId);
        ValueTask<ShortLet> ModifyShortLetAsync(ShortLet shortLet);
        ValueTask<ShortLet> RemoveShortLetByIdAsync(Guid shortLetId);
    }
}