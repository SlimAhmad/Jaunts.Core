using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<WalletBalance> WalletBalance { get; set; }

        public async ValueTask<WalletBalance> InsertWalletBalanceAsync(WalletBalance walletBalance)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<WalletBalance> walletBalanceEntityEntry = await broker.WalletBalance.AddAsync(entity: walletBalance);
            await broker.SaveChangesAsync();

            return walletBalanceEntityEntry.Entity;
        }

        public IQueryable<WalletBalance> SelectAllWalletBalances() => this.WalletBalance
            .Include(x => x.Wallet)
            .ThenInclude(x => x.User)
            .AsQueryable();

        public async ValueTask<WalletBalance> SelectWalletBalanceByIdAsync(Guid walletBalanceId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await WalletBalance.FindAsync(walletBalanceId);
        }

        public async ValueTask<WalletBalance> UpdateWalletBalanceAsync(WalletBalance walletBalance)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<WalletBalance> walletBalanceEntityEntry = broker.WalletBalance.Update(entity: walletBalance);
            await broker.SaveChangesAsync();

            return walletBalanceEntityEntry.Entity;
        }

        public async ValueTask<WalletBalance> DeleteWalletBalanceAsync(WalletBalance walletBalance)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<WalletBalance> walletBalanceEntityEntry = broker.WalletBalance.Remove(entity: walletBalance);
            await broker.SaveChangesAsync();

            return walletBalanceEntityEntry.Entity;
        }
    }
}
