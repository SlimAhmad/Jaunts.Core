using Jaunts.Core.Api.Models.Processings.WalletBalance.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;

namespace Jaunts.Core.Api.Services.Processings.WalletBalances
{
    public partial class WalletBalanceProcessingService
    {
        private static void ValidateWalletBalance(WalletBalance role)
        {
            ValidateWalletBalanceIsNotNull(role);

            Validate(
            (Rule: IsInvalid(role.Id),
                Parameter: nameof(WalletBalance.Id)));
        }
        private static void ValidateWalletBalanceIsNotNull(WalletBalance  walletBalance)
        {
            if (walletBalance is null)
            {
                throw new NullWalletBalanceProcessingException();
            }
        }

        public void ValidateWalletBalanceList(IList<string> roles) =>
           Validate((Rule: IsInvalid(roles), Parameter: nameof(WalletBalance)));
        public void ValidateWalletBalanceId(Guid roleId) =>
           Validate((Rule: IsInvalid(roleId), Parameter: nameof(WalletBalance.Id)));
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
            var invalidWalletBalanceProcessingException = new InvalidWalletBalanceProcessingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidWalletBalanceProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidWalletBalanceProcessingException.ThrowIfContainsErrors();
        }
    }
}