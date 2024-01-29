// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Fleets;

namespace Jaunts.Core.Api.Services.Foundations.Fleets
{
    public interface IFleetService
    {
        ValueTask<Fleet> CreateFleetAsync(Fleet fleet);
        IQueryable<Fleet> RetrieveAllFleets();
        ValueTask<Fleet> RetrieveFleetByIdAsync(Guid fleetId);
        ValueTask<Fleet> ModifyFleetAsync(Fleet fleet);
        ValueTask<Fleet> RemoveFleetByIdAsync(Guid fleetId);
    }
}