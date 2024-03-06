using Jaunts.Core.Api.Models.Processings.Wallet.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;

namespace Jaunts.Core.Api.Services.Processings.Wallets
{
    public partial class WalletProcessingService
    {
        private static void ValidateWallet(Wallet role)
        {
            ValidateWalletIsNotNull(role);

            Validate(
            (Rule: IsInvalid(role.Id),
                Parameter: nameof(Wallet.Id)));
        }
        private static void ValidateWalletIsNotNull(Wallet  wallet)
        {
            if (wallet is null)
            {
                throw new NullWalletProcessingException();
            }
        }

        public void ValidateWalletList(IList<string> roles) =>
           Validate((Rule: IsInvalid(roles), Parameter: nameof(Wallet)));
        public void ValidateWalletId(Guid roleId) =>
           Validate((Rule: IsInvalid(roleId), Parameter: nameof(Wallet.Id)));
        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Value is required"
        };
        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };
        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidWalletProcessingException = new InvalidWalletProcessingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidWalletProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidWalletProcessingException.ThrowIfContainsErrors();
        }
    }
}