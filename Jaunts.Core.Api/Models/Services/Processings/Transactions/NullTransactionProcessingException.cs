// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Transaction.Exceptions
{
    public class NullTransactionProcessingException : Xeption
    {
        public NullTransactionProcessingException()
            : base(message: "Transaction is null.")
        { }
        public NullTransactionProcessingException(string message)
            : base(message)
        { }
    }
}
