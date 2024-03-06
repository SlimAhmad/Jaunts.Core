using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetRideReferences(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Ride>()
                .HasOne(ride => ride.Provider)
                .WithMany(ride => ride.Rides)
                .HasForeignKey(ride => ride.ProviderId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
