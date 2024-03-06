using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Jaunts.Core.Api.Services.Foundations.Wallets;
using System.Linq.Expressions;

namespace Jaunts.Core.Api.Services.Processings.Wallets
{
    public partial class WalletProcessingService : IWalletProcessingService
    {
      
        private readonly IWalletService  walletService;
        private readonly ILoggingBroker loggingBroker;

        public WalletProcessingService(
            IWalletService  WalletService,
            ILoggingBroker loggingBroker

            )
        {
            this.walletService = WalletService;
            this.loggingBroker = loggingBroker;

        }

        public IQueryable<Wallet> RetrieveAllWallets() =>
        TryCatch(() => this.walletService.RetrieveAllWallets());
        public ValueTask<bool> RemoveWalletByIdAsync(
            Guid id) =>
        TryCatch(async () =>
        {
            ValidateWalletId(id);
            var wallet = await walletService.RemoveWalletByIdAsync(id);
            ValidateWallet(wallet);
            return true;
        });

        public ValueTask<Wallet> RetrieveWalletByIdAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateWalletId(id);
            var wallet = await walletService.RetrieveWalletByIdAsync(id);
            ValidateWallet(wallet);
            return wallet;
        });

        public ValueTask<Wallet> UpsertWalletAsync(
                Wallet  Wallet) =>
            TryCatch(async () =>
            {
                ValidateWallet(Wallet);
                Wallet maybeWallet = RetrieveMatchingWallet(Wallet);

                return maybeWallet switch
                {
                    null => await this.walletService.CreateWalletAsync(Wallet),
                    _ => await this.walletService.ModifyWalletAsync(Wallet)
                };
            });




        private Wallet RetrieveMatchingWallet(Wallet Wallet)
        {
            IQueryable<Wallet> Wallets =
                this.walletService.RetrieveAllWallets();

            return Wallets.FirstOrDefault(SameWalletAs(Wallet));
        }

        private static Expression<Func<Wallet, bool>> SameWalletAs(Wallet Wallet) =>
            retrieveWallet => retrieveWallet.Id == Wallet.Id;





    }
}
