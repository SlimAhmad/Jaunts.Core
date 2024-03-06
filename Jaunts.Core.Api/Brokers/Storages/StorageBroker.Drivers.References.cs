using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetDriverReferences(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Driver>()
                .HasOne(driver => driver.Provider)
                .WithMany(driver => driver.Drivers)
                .HasForeignKey(driver => driver.ProviderId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
