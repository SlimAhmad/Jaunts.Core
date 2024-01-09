using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<TransactionFee> TransactionFee { get; set; }

        public async ValueTask<TransactionFee> InsertTransactionFeeAsync(TransactionFee fees)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<TransactionFee> feesEntityEntry = await broker.TransactionFee.AddAsync(entity: fees);
            await broker.SaveChangesAsync();

            return feesEntityEntry.Entity;
        }

        public IQueryable<TransactionFee> SelectAllTransactionFees() => this.TransactionFee;

        public async ValueTask<TransactionFee> SelectTransactionFeeByIdAsync(Guid feesId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await TransactionFee.FindAsync(feesId);
        }

        public async ValueTask<TransactionFee> UpdateTransactionFeeAsync(TransactionFee fees)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<TransactionFee> feesEntityEntry = broker.TransactionFee.Update(entity: fees);
            await broker.SaveChangesAsync();

            return feesEntityEntry.Entity;
        }

        public async ValueTask<TransactionFee> DeleteTransactionFeeAsync(TransactionFee fees)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<TransactionFee> feesEntityEntry = broker.TransactionFee.Remove(entity: fees);
            await broker.SaveChangesAsync();

            return feesEntityEntry.Entity;
        }
    }
}
