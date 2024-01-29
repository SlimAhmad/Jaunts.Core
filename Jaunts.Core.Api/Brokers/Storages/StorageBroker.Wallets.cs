using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Wallet> Wallet { get; set; }

        public async ValueTask<Wallet> InsertWalletAsync(Wallet wallet)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Wallet> walletEntityEntry = await broker.Wallet.AddAsync(entity: wallet);
            await broker.SaveChangesAsync();

            return walletEntityEntry.Entity;
        }

        public IQueryable<Wallet> SelectAllWallets() => this.Wallet;

        public async ValueTask<Wallet> SelectWalletByIdAsync(Guid walletId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Wallet.FindAsync(walletId);
        }

        public async ValueTask<Wallet> UpdateWalletAsync(Wallet wallet)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Wallet> walletEntityEntry = broker.Wallet.Update(entity: wallet);
            await broker.SaveChangesAsync();

            return walletEntityEntry.Entity;
        }

        public async ValueTask<Wallet> DeleteWalletAsync(Wallet wallet)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Wallet> walletEntityEntry = broker.Wallet.Remove(entity: wallet);
            await broker.SaveChangesAsync();

            return walletEntityEntry.Entity;
        }
    }
}
