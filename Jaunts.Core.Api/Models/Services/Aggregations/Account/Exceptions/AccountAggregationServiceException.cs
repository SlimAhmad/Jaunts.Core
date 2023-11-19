// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Aggregation.Account.Exceptions
{
    public class AccountAggregationServiceException : Xeption
    {
        public AccountAggregationServiceException(Xeption innerException)
            : base(message: "Account service error occurred, please fix the errors and try again.",
                  innerException)
        { }
    }
}
