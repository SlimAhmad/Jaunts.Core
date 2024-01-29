using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Transaction> Transaction { get; set; }

        public async ValueTask<Transaction> InsertTransactionAsync(Transaction transaction)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Transaction> transactionEntityEntry = await broker.Transaction.AddAsync(entity: transaction);
            await broker.SaveChangesAsync();

            return transactionEntityEntry.Entity;
        }

        public IQueryable<Transaction> SelectAllTransactions() => this.Transaction;

        public async ValueTask<Transaction> SelectTransactionByIdAsync(Guid transactionId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Transaction.FindAsync(transactionId);
        }

        public async ValueTask<Transaction> UpdateTransactionAsync(Transaction transaction)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Transaction> transactionEntityEntry = broker.Transaction.Update(entity: transaction);
            await broker.SaveChangesAsync();

            return transactionEntityEntry.Entity;
        }

        public async ValueTask<Transaction> DeleteTransactionAsync(Transaction transaction)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Transaction> transactionEntityEntry = broker.Transaction.Remove(entity: transaction);
            await broker.SaveChangesAsync();

            return transactionEntityEntry.Entity;
        }
    }
}
